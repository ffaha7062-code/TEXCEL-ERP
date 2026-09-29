using System;
using System.Net;
using System.Net.Mail;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// Email sending helper.
    /// </summary>
    public static class EmailHelper
    {
        public static bool SendEmail(string toEmail, string subject, string body, string attachmentPath = null)
        {
            try
            {
                var fromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"] ?? "noreply@texcel.com";
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"] ?? "";
                var smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"] ?? "587");

                using (var message = new MailMessage(fromEmail, toEmail))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        message.Attachments.Add(new Attachment(attachmentPath));
                    }

                    using (var smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(fromEmail, password);
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendPayslipEmail(string toEmail, string employeeName, string month, string attachmentPath)
        {
            var subject = $"Payslip for {month} - TEXCEL ERP";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #6c3cb0;'>TEXCEL ERP™</h2>
                    <p>Dear <strong>{employeeName}</strong>,</p>
                    <p>Please find attached your payslip for <strong>{month}</strong>.</p>
                    <br/>
                    <p>Best Regards,<br/>HR Department<br/>TEXCEL ERP™</p>
                </body>
                </html>";

            return SendEmail(toEmail, subject, body, attachmentPath);
        }
    }
}