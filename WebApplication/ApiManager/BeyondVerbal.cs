using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebApplication.Models;

namespace WebApplication.ApiManager
{
    public class BeyondVerbal
    {
        private const string tokenUrl = "https://token.beyondverbal.com/token";
        private const string startUrl = "https://testapiv3.beyondverbal.com/v3/recording/";

        public static async Task<SoundEmotions> RunAnalisys(string postFilePath)
        {
            var sa = new SoundEmotions();

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

            }
            var recordingId = startResponseObj.recordingId.Value;

            ////analysis
            string analysisUrl = startUrl + recordingId;
            var bytes = File.ReadAllBytes(postFilePath);
            var analysisResponseString = CreateWebRequest(analysisUrl, bytes, token);

            var parsedJson = JObject.Parse(analysisResponseString);


            for (int i = 0; i < parsedJson["result"]["analysisSegments"].Count(); i++)
            {
                var ses = new SoundEmotionsSegment();

                ses.Offset = (float)parsedJson["result"]["analysisSegments"][i]["offset"];
                ses.Duration = (float)parsedJson["result"]["analysisSegments"][i]["duration"];
                ses.TemperVal =
                    (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Temper"]["Value"];
                ses.TemperMode =
                    (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Temper"]["Group"];
                ses.ValenceVal =
                    (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Valence"]["Value"];
                ses.ValenceMode =
                    (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Valence"]["Group"];
                ses.ArousalVal =
                    (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Arousal"]["Value"];
                ses.ArousalMode =
                    (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Arousal"]["Group"];
                ses.Gender =
                    (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Gender"]["Group"];
                ses.MoodPrimary =
                    (string)
                    parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Group11"]["Primary"]["Phrase"];
                ses.MoodSecondary =
                    (string)
                    parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Group11"]["Secondary"]["Phrase"];
                ses.CompositePrimary =
                    (string)
                    parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Composite"]["Primary"]["Phrase"];
                ses.CompositeSecondary =
                    (string)
                    parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Composite"]["Secondary"]["Phrase"];

                sa.SegmentsList.Add(ses);

            }


            sa.TemperMeanVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Temper"]["Mean"];
            sa.TemperMeanMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Temper"]["Mode"];
            sa.ValenceMeanVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Valence"]["Mean"];
            sa.ValenceMeanMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Valence"]["Mode"];
            sa.ArousalMeanVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Arousal"]["Mean"];
            sa.ArousalMeanMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Arousal"]["Mode"];

            return sa;


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