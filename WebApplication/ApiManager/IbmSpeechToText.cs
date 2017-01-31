using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication.ApiManager
{

    public class IbmSpeechToText
    {

        //static string file = @"c:\Users\Alexandru\Desktop\test2long.wav";

        static readonly Task CompletedTask = Task.FromResult(true);


        public static async Task SpeeechToText( string file)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                           ApiKeys.ibmSpeechToTextUsername + ":" + ApiKeys.ibmSpeechToTextPassword)));

                var content = new StreamContent(new FileStream(file, FileMode.Open));
                content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                var response = client.PostAsync("https://stream.watsonplatform.net/speech-to-text/api/v1/recognize?continuous=true", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var res = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(res);
                }
            }


        }
    }
}