using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;


namespace WebApplication.ApiManager
{
    public class BeyondVerbal
    {
        private const string tokenUrl = "https://token.beyondverbal.com/token";
        private const string startUrl = "https://testapiv3.beyondverbal.com/v3/recording/";
        private const string postFilePath = @"C:\Users\Alexandru\Desktop\test3.wav";

        public static async Task RunAnalisys()
        {
            var requestData = "apiKey=" + ApiKeys.beyondVerbalSubscriptionKey + "&grant_type=client_credentials";
            //auth
            var token = authRequest(tokenUrl, Encoding.UTF8.GetBytes(requestData));

            //start
            var startResponseString = CreateWebRequest(startUrl + "start",
                Encoding.UTF8.GetBytes("{ dataFormat: { type: \"WAV\" } }"), token);

            var startResponseObj = JsonConvert.DeserializeObject<dynamic>(startResponseString);
            if (startResponseObj.status != "success")
            {
                Debug.WriteLine("Response Status: " + startResponseObj.status);
                return;
            }
            var recordingId = startResponseObj.recordingId.Value;

            ////analysis
            string analysisUrl = startUrl + recordingId;
            var bytes = File.ReadAllBytes(postFilePath);
            var analysisResponseString = CreateWebRequest(analysisUrl, bytes, token);
            Debug.WriteLine(analysisResponseString);

            dynamic parsedJson = JsonConvert.DeserializeObject(analysisResponseString);
            string jstring = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            Debug.WriteLine(jstring);
        }

        private static string authRequest(string url, byte[] data)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ServicePoint.SetTcpKeepAlive(false, 0, 0);
            request.ServicePoint.UseNagleAlgorithm = false;
            request.ReadWriteTimeout = 1000000;
            request.Timeout = 10000000;
            request.SendChunked = false;
            request.AllowWriteStreamBuffering = true;
            request.AllowReadStreamBuffering = false;
            request.KeepAlive = true;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8))
            {
                var res = streamReader.ReadToEnd();
                dynamic responceContent = JsonConvert.DeserializeObject(res, jsonSerializerSettings);
                return responceContent.access_token;
            }
        }

        private static string CreateWebRequest(string url, byte[] data, string token = null)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            request.KeepAlive = true;
            request.ServicePoint.SetTcpKeepAlive(true, 10000, 10000);

            request.Timeout = 10000000;
            request.SendChunked = false;
            request.AllowWriteStreamBuffering = true;
            request.AllowReadStreamBuffering = false;
            if (string.IsNullOrEmpty(token) == false)
                request.Headers.Add("Authorization", "Bearer " + token);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}