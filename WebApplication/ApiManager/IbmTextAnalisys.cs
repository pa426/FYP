using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using WebApplication.Models;


namespace WebApplication.ApiManager
{
    public static class IbmTextAnalisys
    {
        private const string BaseUrl = "https://watson-api-explorer.mybluemix.net/alchemy-api/calls/text/TextGetEmotion";

        public static async Task<AspTextAnalisysSegments> MakeRequests(string getEmo , int i)
        {
            var efs = new AspTextAnalisysSegments();  

            var getUrl = BaseUrl + "?apikey=" + ApiKeys.ibmTextAnalisysSubKey + "&text=" + getEmo;
            var request = WebRequest.Create(getUrl);
            request.Method = WebRequestMethods.Http.Get;
            request.PreAuthenticate = true;

            
            // Get the response.
            var response = (HttpWebResponse) request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return efs;
                using (var reader = new StreamReader(responseStream))
                {

                    var myDoc = new XmlDocument();
                    myDoc.LoadXml(reader.ReadToEnd());

                    efs.TextSegmentIndex = i;
                    efs.TextFromSpeech = getEmo;
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