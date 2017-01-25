using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AlchemyAPIClient;
using AlchemyAPIClient.Requests;
using AlchemyAPIClient.Responses;

namespace WebApplication.CognitiveServicesAuthorizationProvider
{
    public class AlchemyLanguage
    {

        public async Task<AlchemySentimentResponse> GetSentiment(string text)
        {

            var client = new AlchemyClient("18f89f43ce81f33be88e3c4067acc8cd895c3a6e");

            var request = new AlchemyTextSentimentRequest(text, client)
            {
                ShowSourceText = true,
            };
            return await request.GetResponse();
           
        }

    }
}