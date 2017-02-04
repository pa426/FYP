    using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using WebApplication.Models;


namespace WebApplication.ApiManager
{
    public static class IbmTextAnalisys
    {
        private const string BaseUrl = "https://watson-api-explorer.mybluemix.net/alchemy-api/calls/text/TextGetEmotion";

        public static async Task<EmotionsFormSpeech> MakeRequests(string getEmo)
        {
            EmotionsFormSpeech efs = new EmotionsFormSpeech();  

            string getUrl = BaseUrl + "?apikey=" + ApiKeys.ibmTextAnalisysSubKey + "&text=" + getEmo;
            WebRequest request = WebRequest.Create(getUrl);
            request.Method = WebRequestMethods.Http.Get;
            request.PreAuthenticate = true;
            request.Timeout = 10000;


            // Get the response.
            var response = (HttpWebResponse) request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {

                        var myDoc = new XmlDocument();
                        myDoc.LoadXml(reader.ReadToEnd());

                        efs.Anger = float.Parse(myDoc["results"]["docEmotions"]["anger"].InnerText);
                        efs.Disgust = float.Parse(myDoc["results"]["docEmotions"]["disgust"].InnerText);
                        efs.Fear = float.Parse(myDoc["results"]["docEmotions"]["fear"].InnerText);
                        efs.Joy = float.Parse(myDoc["results"]["docEmotions"]["joy"].InnerText);
                        efs.Sadness = float.Parse(myDoc["results"]["docEmotions"]["sadness"].InnerText);

                    }
            }
            return efs;
        }
    }
}