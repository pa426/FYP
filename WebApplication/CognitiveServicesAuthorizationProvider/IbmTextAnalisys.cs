using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication.CognitiveServicesAuthorizationProvider
{
    public static class IbmTextAnalisys
    {

        private const string BaseUrl = "https://gateway-a.watsonplatform.net/calls/";
        private const string AccountKey = "18f89f43ce81f33be88e3c4067acc8cd895c3a6e";
        private const int NumLanguages = 1;


        public static async Task MakeRequests(string speechToText)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add("API_KEY", AccountKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("text\":\"" + speechToText + "\"}");
                // Detect sentiment:
                var uri = "text/TextGetEmotion";
                var response = await CallEndpoint(client, uri, byteData);
                Debug.WriteLine("\n****Detect emotions response:\n****" + response);
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