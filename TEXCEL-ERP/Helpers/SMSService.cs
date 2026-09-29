using System;
using System.Net;
using System.IO;

namespace TEXCEL_ERP.Helpers
{
    /// <summary>
    /// SMS sending helper.
    /// </summary>
    public static class SMSService
    {
        public static bool SendSMS(string mobileNumber, string message)
        {
            try
            {
                var apiKey = System.Configuration.ConfigurationManager.AppSettings["SMSApiKey"] ?? "";
                var senderId = System.Configuration.ConfigurationManager.AppSettings["SMSSenderId"] ?? "TEXCEL";

                var url = $"https://api.smsprovider.com/send?api_key={apiKey}&sender={senderId}&number={mobileNumber}&message={Uri.EscapeDataString(message)}";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}