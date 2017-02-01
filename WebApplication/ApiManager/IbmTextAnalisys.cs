using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace WebApplication.ApiManager
{
    public static class IbmTextAnalisys
    {
        private const string BaseUrl = "https://watson-api-explorer.mybluemix.net/alchemy-api/calls/text/TextGetEmotion";

        public static async Task MakeRequests(string getEmo)
        {
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
                        var JsonResponce = reader.ReadToEnd();
                        Debug.WriteLine(JsonResponce);
                    }
            }
        }
    }
}