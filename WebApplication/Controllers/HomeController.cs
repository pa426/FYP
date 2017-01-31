using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using WebApplication.Models;



namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public const string youtubeSubscriptionKey = "AIzaSyAFKde_KX47mFd7g2YkG3oa9RbmMztq74g";
       

        public async Task<ActionResult> DashboardV0()
        {

            //Video Emotions Analisys
            //var x = await VidAnalisys(); // uploading video 
            //var y = await VidAnalisysResult(x); // getting video analisys

            //DOwnloading from youtube
            //YoutubeSound its = new YoutubeSound();
            //await its.TextToSpeach();

            //IBM Alchemy API 
            //string text = "Testing the product is the most important stage of a project because here you can see if the result matches the specifications. Based on which project methodology techniques you are going to apply which in this case is Agile the testing is similar only the moment of testing will fluctuate (for example if you are using Waterfall technique you perform the same sets of tests only that at the end of development). \r\nIn this project testing will be done all along the development and all sets of tests will be noted and saved, because in this way you can see the flow of the project the improvements and will help you in the final analyze. \r\nThis project is tested by respecting the standard procedures which are presented in (Luo) document and follows the examples from MSDN (Network, 2016) : \r\na)\tUnit testing: the lowest level of testing where every unit of software will be tested. \r\nb)\tIntegration testing: many units will be tested together to check the structure of the system.\r\nc)\tSystem testing: is the stage when you check the final condition of the system where the non- functional attributes of the system are tested. \r\nd)\tAcceptance testing: the system will be tested with some potential users to check the acceptance of those. \r\n\r\nTo make sure that all these methodologies are respected and the project can accede a good standard a Quality Assurance Plan on the project have been made. It can be found in Annex \r\n";
            //await IbmTextAnalisys.MakeRequests(text);


            //Beyond verbal anlisys
            //await BeyondVerbal.RunAnalisys();


            //IbmSpeechToText
            //await IbmSpeechToText.SpeeechToText();

            return View();
        }

        public ActionResult DashboardV1()
        {
            return View();
        }

        public ActionResult DashboardV2()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DashboardV0(VideoModels model)
        {
            List<VideoModels> videos = new List<VideoModels>();

            if (ModelState.IsValid == false)
            {
                return View();
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = youtubeSubscriptionKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = model.VideoQuery; // Replace with your search term.
            searchListRequest.MaxResults = Convert.ToInt32(model.VideoNr);

            if (model.VideoDates != null)
            {
                var dates = model.SeparateDates();
                searchListRequest.PublishedAfter = dates[0];
                searchListRequest.PublishedBefore = dates[1];
            }

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();


            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                VideoModels vid = new VideoModels();

                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        vid.VideoTitle = (String.Format("{0}", searchResult.Snippet.Title));
                        vid.VideoId = (String.Format("{0}", searchResult.Id.VideoId));
                        vid.ChannelTitle = (String.Format("{0}", searchResult.Snippet.ChannelTitle));
                        vid.PublishedAt = (String.Format("{0}", searchResult.Snippet.PublishedAt));
                        vid.ChannelId = (String.Format("{0}", searchResult.Snippet.ChannelId));
                        videos.Add(vid);
                        break;

                    // Further development stages
                    //case "youtube#channel":
                    //    vid.channelName = (String.Format("{0}", searchResult.Snippet.Title));
                    //    vid.channelId = (String.Format("{0}", searchResult.Id.ChannelId));
                    //    break;

                    //case "youtube#playlist":
                    //    vid.playlistName = (String.Format("{0}", searchResult.Snippet.Title));
                    //    vid.playlistId = (String.Format("{0}", searchResult.Id.PlaylistId));
                    //    break;
                }
            }

            ViewBag.Videos = videos;
            ModelState.Clear();
            return View();
        }


        
    }


}