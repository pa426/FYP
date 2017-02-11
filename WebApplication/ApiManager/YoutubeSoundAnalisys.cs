using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frapper;
using WebApplication.Models;
using YoutubeExtractor;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;



namespace WebApplication.ApiManager
{
    public class YoutubeSoundAnalisys
    {
        private static VideoInfo _downloadUrl;
        private ApplicationDbContext db = new ApplicationDbContext();
 
        public async Task TextToSpeach(VideoModel vidmod)
        {
            IEnumerable<VideoInfo> videoInfos =
                DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=" + vidmod.VideoId, false);

            var path = DownloadAudioQuick(videoInfos);
            await Mp4ToWav(path, vidmod);

        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private static string DownloadAudioQuick(IEnumerable<VideoInfo> videoInfos)
        {
            _downloadUrl = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RemoveIllegalPathCharacters(video.Title) + ".mp4");
            var audioDownloader = new VideoDownloader(video, path);

            audioDownloader.Execute();

            return path;
        }


        private async Task Mp4ToWav(string path, VideoModel vidmod)
        {
            FFMPEG ffmpeg = new FFMPEG();


            // Convert to wav.
            string wavPath = path.Replace(".mp4", ".wav");
            ffmpeg.RunCommand("-i \"" + path + "\" -acodec pcm_s16le -ac 1 -ar 16000 \"" + wavPath + "\"");

            //Add Video Detailsused for DB
            var videoDetails = new AspVideoDetails();
            videoDetails.VideoId = vidmod.VideoId;
            videoDetails.VideoTitle = vidmod.VideoTitle;
            videoDetails.ChannelId = vidmod.ChannelId;
            videoDetails.ChannelTitle = vidmod.VideoId;
            videoDetails.UserId = vidmod.UserId;
            videoDetails.PublishedAt = Convert.ToDateTime(vidmod.PublishedAt);
            
            db.AspVideoDetails.Add(videoDetails);
            db.SaveChanges();

            var videoEmotion = MicrosoftVideoEmotion.GetVideoEmotions(_downloadUrl.DownloadUrl);

            var isttTextList = IbmSpeechToText.SpeeechToText(wavPath);

            var bva = BeyondVerbal.RunAnalisys(wavPath);

            await Task.WhenAll(videoEmotion, isttTextList, bva);

            //Video Analisys
            var videoEmotionsSegments = videoEmotion.Result;
            Debug.WriteLine("###Video Analisys Response");
            foreach (var v in videoEmotionsSegments)
            {
              
                v.VideoId = vidmod.VideoId;
                Debug.WriteLine("***Segment number    --->" + v.VideoSegmentIndex);
                Debug.WriteLine("Anger from video     --->" + v.Anger);
                Debug.WriteLine("Contempt from video  --->" + v.Contempt);
                Debug.WriteLine("Disgust from video   --->" + v.Disgust);
                Debug.WriteLine("Fear from video      --->" + v.Fear);
                Debug.WriteLine("Happiness from video --->" + v.Happiness);
                Debug.WriteLine("Neutral from video   --->" + v.Neutral);
                Debug.WriteLine("Sadness from video   --->" + v.Sadness);
                Debug.WriteLine("Surprise from video  --->" + v.Surprise);
                Debug.WriteLine("            ");
                db.AspVideoAnalysisSegment.Add(v);
                db.SaveChanges();
            }
            var videoEmotionsMean = new AspVideoAnalysisSegments();
            videoEmotionsMean.VideoSegmentIndex = -1;
            videoEmotionsMean.VideoId = vidmod.VideoId;
            videoEmotionsMean.Anger = videoEmotionsSegments.Average(item => item.Anger);
            videoEmotionsMean.Contempt = videoEmotionsSegments.Average(item => item.Contempt);
            videoEmotionsMean.Disgust = videoEmotionsSegments.Average(item => item.Disgust);
            videoEmotionsMean.Fear = videoEmotionsSegments.Average(item => item.Fear);
            videoEmotionsMean.Happiness = videoEmotionsSegments.Average(item => item.Happiness);
            videoEmotionsMean.Neutral = videoEmotionsSegments.Average(item => item.Neutral);
            videoEmotionsMean.Sadness = videoEmotionsSegments.Average(item => item.Sadness);
            videoEmotionsMean.Surprise = videoEmotionsSegments.Average(item => item.Surprise);
            db.AspVideoAnalysisSegment.Add(videoEmotionsMean);
            db.SaveChanges();

            //Speech to Text Microsoft
            //var mstt = new MicrosoftSpeechToText();
            //var msttTextList = await mstt.SpeechToTextTransformation(wavPath, "en-US");

            //Text analitycs Microsoft
            //foreach (var v in msttTextList)
            //{
            //    //return a string with the result for each string sent
            //    var sentiment = await MicrosoftTextAnalytics.MakeRequests(v);
            //}

            ////Text analitycs IBM
            var textAnalisysSegments = new List<AspTextAnalisysSegments>();
            int i = 0;
            foreach (var var in isttTextList.Result)
            {
                if (var != "")
                {
                    var textAnalisysSegment = await IbmTextAnalisys.MakeRequests(var, i);
                    textAnalisysSegment.VideoId = vidmod.VideoId;
                    Debug.WriteLine("###Text Analisys REsponse Segment:" + textAnalisysSegment.TextSegmentIndex);
                    Debug.WriteLine("****Text due to analise ---> " + textAnalisysSegment.TextFromSpeech);
                    Debug.WriteLine("            ");
                    Debug.WriteLine("****Sentiments from text ");
                    Debug.WriteLine("Anger    --->" + textAnalisysSegment.Anger.ToString());
                    Debug.WriteLine("Disgust  --->" + textAnalisysSegment.Disgust.ToString());
                    Debug.WriteLine("Fear     --->" + textAnalisysSegment.Fear.ToString());
                    Debug.WriteLine("Joy      --->" + textAnalisysSegment.Joy.ToString());
                    Debug.WriteLine("Sadness  --->" + textAnalisysSegment.Sadness.ToString());
                    Debug.WriteLine("            ");
                    i++;
                    textAnalisysSegments.Add(textAnalisysSegment);
                    db.AspTextAnalisysSegment.Add(textAnalisysSegment);
                    db.SaveChanges();
                }

                Task.Delay(3000).Wait();
            }

            var textAnalisysMean = new AspTextAnalisysSegments();
            textAnalisysMean.TextSegmentIndex = -1;
            textAnalisysMean.VideoId = vidmod.VideoId;
            textAnalisysMean.Anger = textAnalisysSegments.Average(item => item.Anger);
            textAnalisysMean.Disgust = textAnalisysSegments.Average(item => item.Disgust);
            textAnalisysMean.Fear = textAnalisysSegments.Average(item => item.Fear);
            textAnalisysMean.Joy = textAnalisysSegments.Average(item => item.Joy);
            textAnalisysMean.Sadness = textAnalisysSegments.Average(item => item.Sadness);
            db.AspTextAnalisysSegment.Add(textAnalisysMean);
            db.SaveChanges();

            Debug.WriteLine("Mean Text Analisys Anger MeanMode  --->" + textAnalisysMean.Anger);
            Debug.WriteLine("Mean Text Analisys Disgust MeanVal   --->" + textAnalisysMean.Disgust);
            Debug.WriteLine("Mean Text Analisys Fear MeanMode   --->" + textAnalisysMean.Fear);
            Debug.WriteLine("Mean Text Analisys Joy aMeanVal    --->" + textAnalisysMean.Joy);
            Debug.WriteLine("Mean Text Analisys Sadness MeanMode  --->" + textAnalisysMean.Sadness);


            //Beyond verbal anlisys

            Debug.WriteLine("###Sound Analisys Response");

            i = 0;
            foreach (var s in bva.Result)
            {
                s.VideoId = vidmod.VideoId;
                Debug.WriteLine("***Segment number:" + s.SoundSegmentIndex);
                Debug.WriteLine("Segment sound analisys Duration            ---->" + s.Duration);
                Debug.WriteLine("Segment sound analisys Gender              ---->" + s.Gender);
                Debug.WriteLine("Segment sound analisys Offset              ---->" + s.Offset);
                Debug.WriteLine("Segment sound analisys ArousalMode         ---->" + s.ArousalMode);
                Debug.WriteLine("Segment sound analisys ArousalVal          ---->" + s.ArousalVal);
                Debug.WriteLine("Segment sound analisys ValenceMode         ---->" + s.ValenceMode);
                Debug.WriteLine("Segment sound analisys ValenceVal          ---->" + s.ValenceVal);
                Debug.WriteLine("Segment sound analisys TemperMode          ---->" + s.TemperMode);
                Debug.WriteLine("Segment sound analisys TemperVal           ---->" + s.TemperVal);
                Debug.WriteLine("Segment sound analisys MoodPrimary         ---->" + s.MoodPrimary);
                Debug.WriteLine("Segment sound analisys MoodSecondary       ---->" + s.MoodSecondary);
                Debug.WriteLine("Segment sound analisys CompositePrimary    ---->" + s.CompositePrimary);
                Debug.WriteLine("Segment sound analisys CompositeSecondary  ---->" + s.CompositeSecondary);
                Debug.WriteLine("            ");
                db.AspSoundAnalisysSegment.Add(s);
                db.SaveChanges();
            }


            // Cleanup.
            File.Delete(path);
            File.Delete(wavPath);

        }
    }
}