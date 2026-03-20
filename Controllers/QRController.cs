using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.IO;

namespace OstaFeedbackApp.Controllers
{
    public class QRController : Controller
    {
        // GET: /QR/Generate
        public IActionResult Generate()
        {
            // The URL to your feedback form
            //string feedbackUrl = Url.Action("Create", "Feedback", null, Request.Scheme);
            string feedbackUrl = "http://192.168.1.6:7222/Feedback/Create";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(feedbackUrl, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new BitmapByteQRCode(qrData);
                byte[] qrCodeImage = qrCode.GetGraphic(20); // pixels per module

                return File(qrCodeImage, "image/png");
            }
        }

        // Optional: Page to display QR code and instructions
        public IActionResult QRPage()
        {
            return View();
        }
    }
}