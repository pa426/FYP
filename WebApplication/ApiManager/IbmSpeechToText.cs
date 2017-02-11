using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebApplication.Models;


namespace WebApplication.ApiManager
{
    public class IbmSpeechToText
    {
        //static string file = @"c:\Users\Alexandru\Desktop\test2long.wav";

        public static async Task<List<string>> SpeeechToText(string file)
        {
            var responseList = new List<string>();
            string text = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            ApiKeys.ibmSpeechToTextUsername + ":" + ApiKeys.ibmSpeechToTextPassword)));

                var content = new StreamContent(new FileStream(file, FileMode.Open));
                content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                using (
                    var response =
                        client.PostAsync(
                                "https://stream.watsonplatform.net/speech-to-text/api/v1/recognize?continuous=true", content)
                            .Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var res = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        int i = 0;
                        
                        foreach (var v in res["results"]) 
                        {
                            if (i % 5 == 0 && i != 0)
                            {
                                text += (string) v["alternatives"][0]["transcript"] + " ";
                                responseList.Add(text);
                                text = "";
                            }
                            else
                            {
                                text += (string) v["alternatives"][0]["transcript"] + " ";
                            }

                            i++;

                        }

                        responseList.Add(text);
                    }
                }
            }
            return responseList;
        }
    }
}