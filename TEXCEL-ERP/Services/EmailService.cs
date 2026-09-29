using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for email operations.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public EmailService()
        {
            _fromEmail = ConfigurationManager.AppSettings["FromEmail"] ?? "noreply@texcel.com";
            _fromPassword = ConfigurationManager.AppSettings["EmailPassword"] ?? "";
            _smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
        }

        public void SendEmail(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                using (var message = new MailMessage(_fromEmail, toEmail))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    using (var smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error (later we'll add proper logging)
                System.Diagnostics.Debug.WriteLine($"Email send failed: {ex.Message}");
            }
        }

        public void SendForgotPasswordEmail(string toEmail, string callbackUrl)
        {
            var subject = "Reset Your Password - TEXCEL ERP™";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:0 auto; padding:20px;'>
                        <h2 style='color:#6c3cb0;'>TEXCEL ERP™</h2>
                        <p>Hello,</p>
                        <p>You have requested to reset your password. Please click the button below:</p>
                        <p style='text-align:center; margin:30px 0;'>
                            <a href='{callbackUrl}' style='background:linear-gradient(135deg,#6c3cb0,#8b5cf6); color:white; padding:14px 32px; text-decoration:none; border-radius:8px; font-weight:bold;'>
                                Reset Password
                            </a>
                        </p>
                        <p>If you didn't request this, please ignore this email.</p>
                        <br/>
                        <p style='color:#999; font-size:12px;'>© 2026 TEXCEL ERP™ — Textile & Garment ERP System</p>
                    </div>
                </body>
                </html>";

            SendEmail(toEmail, subject, body);
        }
    }
}