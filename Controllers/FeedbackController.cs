using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OstaFeedbackApp.Data;
using OstaFeedbackApp.Models;
using QRCoder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OstaFeedbackApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // PUBLIC: CREATE FEEDBACK
        // =============================
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            try
            {
                feedback.CreatedAt = DateTime.UtcNow;

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ThankYou));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Error saving feedback: " + ex.InnerException?.Message);
                return View(feedback);
            }
        }

        // =============================
        // THANK YOU PAGE
        // =============================
        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }

        // =============================
        // 🔐 ADMIN DASHBOARD
        // =============================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();

            if (feedbacks.Count == 0)
                return View(feedbacks);

            ViewBag.AvgCommitment = feedbacks.Average(f => f.Commitment);
            ViewBag.AvgTransparency = feedbacks.Average(f => f.Transparency);
            ViewBag.AvgInnovation = feedbacks.Average(f => f.Innovation);
            ViewBag.AvgCommunityImpact = feedbacks.Average(f => f.CommunityImpact);
            ViewBag.AvgYouthOpportunity = feedbacks.Average(f => f.YouthOpportunity);

            return View(feedbacks);
        }

        // =============================
        // QR CODE GENERATION
        // =============================
        [AllowAnonymous]
        public IActionResult GenerateQR()
        {
            // 👉 Replace with your real IP or ngrok URL
            string feedbackUrl = "http://192.168.1.6:7222/Feedback/Create";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(feedbackUrl, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new BitmapByteQRCode(qrData);

                byte[] qrCodeImage = qrCode.GetGraphic(20);

                return File(qrCodeImage, "image/png");
            }
        }

        // =============================
        // QR PAGE
        // =============================
        [AllowAnonymous]
        public IActionResult QRPage()
        {
            return View();
        }
    }
}