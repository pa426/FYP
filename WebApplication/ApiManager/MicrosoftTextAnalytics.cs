using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication.ApiManager
{
    public static class MicrosoftTextAnalytics
    {
        private const string BaseUrl = "https://westus.api.cognitive.microsoft.com/";


        public static async Task<string> MakeRequests(string text)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKeys.microsoftTextAnalitycsSubKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" +
                                                         "{\"id\":\"1\",\"text\":\"" + text + "\"},]}");
                // Detect sentiment:
                var uri = "text/analytics/v2.0/sentiment";
                var response = await CallEndpoint(client, uri, byteData);
                Debug.WriteLine("\n****Detect sentiment response:\n****" + response);
                return response;
            }
        }

        static async Task<String> CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}