using System.Text;

namespace ClaimsPay.Shared
{
    public class AppHttpClient
    {
        private static readonly HttpClient httpClient = new();

        public async Task<string> HTTPPost(string url, string requestData, string mediaType = "application/j")
        {
            string result = string.Empty;

            try
            {
                var stringContent = new StringContent(requestData, Encoding.UTF8, mediaType);
                var response = await httpClient.PostAsync(url, stringContent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //Logger class to be implemented
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

    }
}
