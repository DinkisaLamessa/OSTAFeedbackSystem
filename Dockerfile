# Build Stage
FROM ://mcr.microsoft.com AS build
WORKDIR /source
COPY . .
RUN dotnet publish -c Release -o /app

# Run Stage
FROM ://mcr.microsoft.com
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OstaFeedbackApp.dll"]