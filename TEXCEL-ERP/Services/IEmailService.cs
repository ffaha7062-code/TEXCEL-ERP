namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for email operations.
    /// </summary>
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body, bool isHtml = true);
        void SendForgotPasswordEmail(string toEmail, string callbackUrl);
    }
}