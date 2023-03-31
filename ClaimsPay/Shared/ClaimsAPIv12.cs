using System.Text;
using System.Xml;

namespace ClaimsPay.Shared
{
    class ClaimsAPIv12 : ClaimsAPI
    {
       
        private static string authToken = string.Empty;
        private static DateTime authTokenExpire = DateTime.MinValue;

        public override string BaseURI => AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimAPIURI"];

        protected override HttpRequestMessage CreateRequest(HttpMethod method, ILogger log, string uri, string authToken, string userID)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 5, 0);
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);

            if (string.IsNullOrEmpty(userID) == false)
                httpRequestMessage.Headers.Add("UserId", userID);
            else
                httpRequestMessage.Headers.Add("UserId", AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimAPIDefaultUser"]);

            httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimAPIKEY"]);

            //if (string.IsNullOrWhiteSpace(authToken))
            //    authToken = GetAuthToken(log);

            //HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);

            //httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);


            return httpRequestMessage;
        }
    }

}
