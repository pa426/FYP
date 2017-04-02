using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.VisualStudio.Threading;
using WebApplication.Models;
using YoutubeExtractor;

namespace WebApplication.ApiManager
{
    public class YoutubeSoundAnalisys
    {
        private VideoInfo _downloadUrl;
        private readonly DbModelDataContext db = new DbModelDataContext();

        public async Task TextToSpeach(VideoModel vidmod)
        {
            var videoInfos =
                DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=" + vidmod.VideoId, false);

            var path = DownloadAudioQuick(videoInfos);
            await Mp4ToWav(path, vidmod);
        }

        private string RemoveIllegalPathCharacters(string path)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private string DownloadAudioQuick(IEnumerable<VideoInfo> videoInfos)
        {
            _downloadUrl = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            var video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);
            var path = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),
                RemoveIllegalPathCharacters(video.Title) + ".mp4");
            var audioDownloader = new VideoDownloader(video, path);

            audioDownloader.Execute();

            return path;
        }


        private async Task Mp4ToWav(string path, VideoModel vidmod)
        {
            var wavPath = path.Replace(".mp4", ".wav");

            var psi = new ProcessStartInfo
            {
                FileName = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "ffmpeg.exe"),
                Arguments = "-i \"" + path + "\" -acodec pcm_s16le -ac 1 -ar 16000 \"" + wavPath + "\"",
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                RedirectStandardError = true
            };
            var proc = Process.Start(psi);
            await proc.WaitForExitAsync();
            File.Delete(path);  


            //Add Video Details used for DB
            var videoDetails = new AspVideoDetail
            {
                VideoId = vidmod.VideoId,
                VideoTitle = vidmod.VideoTitle,
                ChannelId = vidmod.ChannelId,
                ChannelTitle = vidmod.ChannelTitle,
                UserId = vidmod.UserId,
                VideoGroupID = 1,
                PublishedAt = Convert.ToDateTime(vidmod.PublishedAt),
                VideoLocation = vidmod.VideoLocation,
                Date = DateTime.Now
            };
            db.AspVideoDetails.InsertOnSubmit(videoDetails);
            db.SubmitChanges();

            //Start parallel threads for analisys
            var ve = new MicrosoftVideoEmotion();
            var te = new IbmSpeechToText();
            var se = new BeyondVerbal();

            var videoEmotion = ve.GetVideoEmotions(_downloadUrl.DownloadUrl);
            var isttTextList = te.SpeeechToText(wavPath);
            var bva = se.RunAnalisys(wavPath);
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
                db.AspVideoAnalysisSegments.InsertOnSubmit(v);
                db.SubmitChanges();
            }
            var videoEmotionsMean = new AspVideoAnalysisSegment();
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
            db.AspVideoAnalysisSegments.InsertOnSubmit(videoEmotionsMean);
            db.SubmitChanges();

            ////Text analitycs IBM
            var textAnalisysSegments = new List<AspTextAnalisysSegment>();
            var i = 0;
            foreach (var var in isttTextList.Result)
            {
                if (var != "")
                {
                    var ia = new IbmTextAnalisys();
                    var textAnalisysSegment = await ia.MakeRequests(var, i);

                    if (textAnalisysSegment.Anger != 0 && textAnalisysSegment.Disgust != 0 &&
                        textAnalisysSegment.Fear != 0 && textAnalisysSegment.Joy != 0 &&
                        textAnalisysSegment.Sadness != 0)
                    {
                        textAnalisysSegment.VideoId = vidmod.VideoId;
                        Debug.WriteLine("###Text Analisys REsponse Segment:" + textAnalisysSegment.TextSegmentIndex);
                        Debug.WriteLine("****Text due to analise ---> " + textAnalisysSegment.TextFromSpeech);
                        Debug.WriteLine("            ");
                        Debug.WriteLine("****Sentiments from text ");
                        Debug.WriteLine("Anger    --->" + textAnalisysSegment.Anger);
                        Debug.WriteLine("Disgust  --->" + textAnalisysSegment.Disgust);
                        Debug.WriteLine("Fear     --->" + textAnalisysSegment.Fear);
                        Debug.WriteLine("Joy      --->" + textAnalisysSegment.Joy);
                        Debug.WriteLine("Sadness  --->" + textAnalisysSegment.Sadness);
                        Debug.WriteLine("            ");
                        i++;
                        textAnalisysSegments.Add(textAnalisysSegment);
                        db.AspTextAnalisysSegments.InsertOnSubmit(textAnalisysSegment);
                        db.SubmitChanges();
                    }
                }

                Task.Delay(3000).Wait();
            }

            var textAnalisysMean = new AspTextAnalisysSegment();
            textAnalisysMean.TextSegmentIndex = -1;
            textAnalisysMean.VideoId = vidmod.VideoId;
            textAnalisysMean.Anger = textAnalisysSegments.Average(item => item.Anger);
            textAnalisysMean.Disgust = textAnalisysSegments.Average(item => item.Disgust);
            textAnalisysMean.Fear = textAnalisysSegments.Average(item => item.Fear);
            textAnalisysMean.Joy = textAnalisysSegments.Average(item => item.Joy);
            textAnalisysMean.Sadness = textAnalisysSegments.Average(item => item.Sadness);
            db.AspTextAnalisysSegments.InsertOnSubmit(textAnalisysMean);
            db.SubmitChanges();

            Debug.WriteLine("Mean Text Analisys Anger MeanMode  --->" + textAnalisysMean.Anger);
            Debug.WriteLine("Mean Text Analisys Disgust MeanVal   --->" + textAnalisysMean.Disgust);
            Debug.WriteLine("Mean Text Analisys Fear MeanMode   --->" + textAnalisysMean.Fear);
            Debug.WriteLine("Mean Text Analisys Joy aMeanVal    --->" + textAnalisysMean.Joy);
            Debug.WriteLine("Mean Text Analisys Sadness MeanMode  --->" + textAnalisysMean.Sadness);


            //Beyond verbal anlisys
            Debug.WriteLine("###Sound Analisys Response");
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
                db.AspSoundAnalisysSegments.InsertOnSubmit(s);
                db.SubmitChanges();
            }

            //Add main Sentiment for video
            var mainSent = GetMainSentiment(videoEmotionsMean, textAnalisysMean, bva.Result[0]);
            videoDetails.MainSentiment = mainSent;
            db.SubmitChanges();
            Debug.WriteLine("Main sentiment for video" + videoDetails.VideoTitle + " : " + videoDetails.MainSentiment);
            Debug.WriteLine("Analisys Finished for video: " + videoDetails.VideoTitle);

            //Push notification update
            var myHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            myHub.Clients.All.notify(videoDetails.VideoId);

            // Cleanup.
            File.Delete(wavPath);
        }

        private int GetMainSentiment(AspVideoAnalysisSegment x, AspTextAnalisysSegment y , AspSoundAnalisysSegment z)
        {
            var vidMean = 0;
            if ((x.Anger + x.Contempt + x.Disgust + x.Fear) > (x.Neutral + x.Sadness) && (x.Anger + x.Contempt + x.Disgust + x.Fear) > (x.Happiness + x.Surprise))
            {
                vidMean = 1;
            }
            else if ((x.Neutral + x.Sadness) > (x.Anger + x.Contempt + x.Disgust + x.Fear) && (x.Neutral + x.Sadness) > (x.Happiness + x.Surprise))
            {
                vidMean = 2;
            }
            else if ((x.Happiness + x.Surprise) > (x.Anger + x.Contempt + x.Disgust + x.Fear) && (x.Happiness + x.Surprise) > (x.Neutral + x.Sadness))
            {
                vidMean = 3;
            }


            var textMean = 0;
            if ((y.Anger + y.Disgust + y.Fear) > y.Sadness && (y.Anger + y.Disgust + y.Fear) > y.Joy)
            {
                textMean = 1;
            }
            else if (y.Sadness > (y.Anger + y.Disgust + y.Fear) && y.Sadness > y.Joy)
            {
                textMean = 2;
            }
            else if (y.Joy > (y.Anger + y.Disgust + y.Fear) && y.Joy > y.Sadness)
            {
                textMean = 3;
            }


            var soundMean = 0;
            if (z.TemperVal > z.ValenceVal && z.TemperVal > z.ArousalVal)
            {
                soundMean = 1;
            }
            else if (z.ValenceVal > z.TemperVal && z.ValenceVal > z.ArousalVal)
            {
                soundMean = 2;
            }
            else if (z.ArousalVal > z.TemperVal && z.ArousalVal > z.ValenceVal)
            {
                soundMean = 3;
            }

            var meanmean = 0;
            if ((vidMean + textMean + soundMean) <= 5)
            {
                meanmean = 0;
            }
            else if ((vidMean + textMean + soundMean) == 6)
            {
                meanmean = 1;
            }
            else if ((vidMean + textMean + soundMean) >= 7)
            {
                meanmean = 2;
            }

            return meanmean;
        }
    }
}


//Speech to Text Microsoft
//var mstt = new MicrosoftSpeechToText();
//var msttTextList = await mstt.SpeechToTextTransformation(wavPath, "en-US");

//Text analitycs Microsoft
//foreach (var v in msttTextList)
//{
//    //return a string with the result for each string sent
//    var sentiment = await MicrosoftTextAnalytics.MakeRequests(v);
//}