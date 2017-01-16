using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.ProjectOxford.Common.Contract;
using WebApplication.Models;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Newtonsoft.Json;
using YoutubeExtractor;


namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public const string youtubeSubscriptionKey = "AIzaSyAFKde_KX47mFd7g2YkG3oa9RbmMztq74g";
        public const string emotionSubscriptionKey = "2cc6f73902d74e08b3d7e0e32af04a3f";

        public async Task<ActionResult> DashboardV0()
        {
            //var x = await vidAnalisys(); // uploading video 
            //var y = await vidAnalisysResult(x); // getting video analisys
            var z = await downloadSound();

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


        public async Task<VideoEmotionRecognitionOperation> vidAnalisys()
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(emotionSubscriptionKey);
            VideoEmotionRecognitionOperation videoOperation;

            var videoUrl =
                "https://d2v9y0dukr6mq2.cloudfront.net/video/preview/VGqRaYUogil0szy2p/sad-face-of-young-guy-man-cry-with-tears_rbrfjci8__PM.mp4";

            Debug.WriteLine("Video Analyse starting________________________________________________________");
            videoOperation = await emotionServiceClient.RecognizeInVideoAsync(videoUrl);
            return videoOperation;
        }

        public async Task<VideoOperationResult> vidAnalisysResult(VideoEmotionRecognitionOperation videoOperation)
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(emotionSubscriptionKey);
            VideoOperationResult operationResult;

            Debug.WriteLine("Uploaded Analisys starting________________________________________________________");
            while (true)
            {
                operationResult = await emotionServiceClient.GetOperationResultAsync(videoOperation);
                if (operationResult.Status == VideoOperationStatus.Succeeded ||
                    operationResult.Status == VideoOperationStatus.Failed)
                {
                    break;
                }
                else if (operationResult.Status == VideoOperationStatus.Running)
                {
                    Debug.WriteLine("Analisys still running please wait");
                }

                Task.Delay(30000).Wait(); //(0.5 min)
            }

            var operationResultemotionRecognitionJsonString =
                ((VideoOperationInfoResult<VideoAggregateRecognitionResult>) operationResult).ProcessingResult;


            Debug.WriteLine("Sentiments :" + operationResultemotionRecognitionJsonString);

            foreach (var frag in operationResultemotionRecognitionJsonString.Fragments)
            {
                if (frag.Events != null)
                {
                    foreach (var x in frag.Events)
                    {
                        foreach (var y in x)
                        {
                            Debug.WriteLine("Anger " + y.WindowMeanScores.Anger);
                            Debug.WriteLine("Contempt " + y.WindowMeanScores.Contempt);
                            Debug.WriteLine("Disgust " + y.WindowMeanScores.Disgust);
                            Debug.WriteLine("Fear " + y.WindowMeanScores.Fear);
                            Debug.WriteLine("Happiness " + y.WindowMeanScores.Happiness);
                            Debug.WriteLine("Neutral " + y.WindowMeanScores.Neutral);
                            Debug.WriteLine("Sadness " + y.WindowMeanScores.Sadness);
                            Debug.WriteLine("Surprise " + y.WindowMeanScores.Surprise);
                        }
                    }
                }
            }

            return operationResult;
        }

        public async Task<AudioDownloader> downloadSound()
        {
            AudioDownloader audioDownloader = null;

            try
            {
                string link = "https://www.youtube.com/watch?v=arqTu9Ay4Ig";

                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

               
                /*
                 * We want the first extractable video with the highest audio quality.
                 */
                VideoInfo videox = videoInfos
                        .Where(info => info.CanExtractAudio)
                        .OrderByDescending(info => info.AudioBitrate)
                        .First();

                /*
                 * If the video has a decrypted signature, decipher it
                 */
                if (videox.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(videox);
                }

                /*
                 * Create the audio downloader.
                 * The first argument is the video where the audio should be extracted from.
                 * The second argument is the path to save the audio file.
                 */
                audioDownloader = new AudioDownloader(videox, Path.Combine("‪C:/Users/Alexandru/Desktop", videox.Title + videox.AudioExtension));

                // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
                // because the download will take much longer than the audio extraction.
                audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
                audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

                /*
                 * Execute the audio downloader.
                 * For GUI applications note, that this method runs synchronously.
                 */
                audioDownloader.Execute();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            return audioDownloader;

        }
    }
}