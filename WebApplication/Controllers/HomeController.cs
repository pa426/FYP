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
        public ActionResult DashboardV0()
        {
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
        public async Task<ActionResult> _SearchVideo(VideoModels model)
        {
            List<VideoModels> videos = new List<VideoModels>();

            if (ModelState.IsValid == false)
            {
                return View("DashboardV0");
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAFKde_KX47mFd7g2YkG3oa9RbmMztq74g",
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
            return View("DashboardV0");
        }


        [HttpPost]
        [AllowAnonymous]
        public void CheckAll()
        {
            if (Request.Form["checkAllBtn"] != null)
            {
                Debug.WriteLine( "test check all");
            }
            else if (Request.Form["removeAllBtn"] != null)
            {
                Debug.WriteLine("test check all");
            }
            else if (Request.Form["analiseBtn"] != null)
            {
                Debug.WriteLine("test check all");
            }

        }
    }
}