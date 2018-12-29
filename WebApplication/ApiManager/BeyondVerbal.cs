using System.Collections.Generic;
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

        public async Task<List<AspSoundAnalisysSegment>> RunAnalisys(string postFilePath)
        {
            var sa = new List<AspSoundAnalisysSegment>();

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

            try
            {

                int i;
                for (i = 0; i < parsedJson["result"]["analysisSegments"].Count(); i++)
                {
                    var sesSegment = new AspSoundAnalisysSegment();

                    sesSegment.SoundSegmentIndex = i;
                    sesSegment.Offset = (float)parsedJson["result"]["analysisSegments"][i]["offset"];
                    sesSegment.Duration = (float)parsedJson["result"]["analysisSegments"][i]["duration"];
                    sesSegment.TemperVal =
                        (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Temper"]["Value"];
                    sesSegment.TemperMode =
                        (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Temper"]["Group"];
                    sesSegment.ValenceVal =
                        (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Valence"]["Value"];
                    sesSegment.ValenceMode =
                        (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Valence"]["Group"];
                    sesSegment.ArousalVal =
                        (float)parsedJson["result"]["analysisSegments"][i]["analysis"]["Arousal"]["Value"];
                    sesSegment.ArousalMode =
                        (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Arousal"]["Group"];
                    sesSegment.Gender =
                        (string)parsedJson["result"]["analysisSegments"][i]["analysis"]["Gender"]["Group"];
                    sesSegment.MoodPrimary =
                        (string)
                        parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Group11"]["Primary"]["Phrase"];
                    sesSegment.MoodSecondary =
                        (string)
                        parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Group11"]["Secondary"]["Phrase"];
                    sesSegment.CompositePrimary =
                        (string)
                        parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Composite"]["Primary"]["Phrase"];
                    sesSegment.CompositeSecondary =
                        (string)
                        parsedJson["result"]["analysisSegments"][i]["analysis"]["Mood"]["Composite"]["Secondary"]["Phrase"];

                    sa.Add(sesSegment);

                }

                if (i > 1)
                {
                    var sesMean = new AspSoundAnalisysSegment();
                    sesMean.SoundSegmentIndex = -1;
                    sesMean.TemperVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Temper"]["Mean"];
                    sesMean.TemperMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Temper"]["Mode"];
                    sesMean.ValenceVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Valence"]["Mean"];
                    sesMean.ValenceMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Valence"]["Mode"];
                    sesMean.ArousalVal = (float)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Arousal"]["Mean"];
                    sesMean.ArousalMode = (string)parsedJson["result"]["analysisSummary"]["AnalysisResult"]["Arousal"]["Mode"];
                   
                    sa.Add(sesMean);
                }
            }
            catch
            {
                var sesMean = new AspSoundAnalisysSegment();
                sesMean.SoundSegmentIndex = 0;
                sesMean.TemperVal = 0;
                sesMean.TemperMode = "The video is under 20 Second, we cant perform analysis";
                sesMean.ValenceVal = 0;
                sesMean.ValenceMode = "The video is under 20 Second, we cant perform analysis";
                sesMean.ArousalVal = 0;
                sesMean.ArousalMode = "The video is under 20 Second, we cant perform analysis";

                sesMean.MoodPrimary = "The video is under 20 Second, we cant perform analysis";
                sesMean.MoodSecondary = "The video is under 20 Second, we cant perform analysis";
                sesMean.CompositePrimary = "The video is under 20 Second, we cant perform analysis";
                sesMean.CompositeSecondary = "The video is under 20 Second, we cant perform analysis";
                sa.Add(sesMean);
            }


            return sa;

        }

        private string authRequest(string url, byte[] data)
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

        private string CreateWebRequest(string url, byte[] data, string token = null)
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
