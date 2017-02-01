using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using WebApplication.Models;

namespace WebApplication.ApiManager
{
    public class YoutubeList
    {

        public async Task <List<VideoModels>> GetYoutubeList(string videoQuery, string videoNr, string videoDates)
        {
            List<VideoModels> videos = new List<VideoModels>();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ApiKeys.youtubeSubscriptionKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = videoQuery; // Replace with your search term.
            searchListRequest.MaxResults = Convert.ToInt32(videoNr);

            if (videoDates != null)
            {
                var dates = SeparateDates(videoDates);
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

            return videos;
        }

        private DateTime[] SeparateDates(string videoDates)
        {
            DateTime[] dates = new DateTime[2];

            var date1 = videoDates.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);

            dates[0] = DateTime.ParseExact(date1[0], "dd/MM/yyyy", null);
            dates[1] = DateTime.ParseExact(date1[1], "dd/MM/yyyy", null);

            Debug.WriteLine(date1[0] + dates[1]);
            Debug.WriteLine(dates[0]);
            Debug.WriteLine(dates[1]);

            return dates;
        }
    }
}