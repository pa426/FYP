using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult DashboardV0()
        {
            SearchRun();
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

        //Search Youtube video
        public void SearchRun()
        {

            try
            {
                Run();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                }
            }

        }

        private async Task Run()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAFKde_KX47mFd7g2YkG3oa9RbmMztq74g",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = "Obama"; // Replace with your search term.
            searchListRequest.MaxResults = 2;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            VideoModels vid = new VideoModels();
            List<VideoModels> videos = new List<VideoModels>();
            

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        vid.videoName = (String.Format("{0}", searchResult.Snippet.Title));
                        vid.videoId = (String.Format("{0}",  searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        vid.channelName = (String.Format("{0}", searchResult.Snippet.Title));
                        vid.channelId = (String.Format("{0}", searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        vid.playlistName = (String.Format("{0}", searchResult.Snippet.Title));
                        vid.playlistId = (String.Format("{0}", searchResult.Id.PlaylistId));
                        break;

                }
                videos.Add(vid);
            }

            System.Diagnostics.Debug.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //System.Diagnostics.Debug.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //System.Diagnostics.Debug.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
            //Experimental for now
        }

    }
}