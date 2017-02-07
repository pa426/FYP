using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frapper;
using WebApplication.Models;
using YoutubeExtractor;

namespace WebApplication.ApiManager
{
    public class YoutubeSoundAnalisys
    {
        private static VideoInfo downloadUrl;

        public static async Task<AnalysisModels> TextToSpeach(string id)
        {
            IEnumerable<VideoInfo> videoInfos =
                DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=" + id, false);

            var path = DownloadAudioQuick(videoInfos);
            var am = await Mp4ToWav(path);
            return am;
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private static string DownloadAudioQuick(IEnumerable<VideoInfo> videoInfos)
        {
            downloadUrl = videoInfos
               .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RemoveIllegalPathCharacters(video.Title) + ".mp4");
            var audioDownloader = new VideoDownloader(video, path);

            audioDownloader.Execute();

            return path;
        }


        private static async Task<AnalysisModels> Mp4ToWav(string path)
        {
            FFMPEG ffmpeg = new FFMPEG();
            AnalysisModels am = new AnalysisModels();

            // Convert to wav.
            string wavPath = path.Replace(".mp4", ".wav");
            ffmpeg.RunCommand("-i \"" + path + "\" -acodec pcm_s16le -ac 1 -ar 16000 \"" + wavPath + "\"");

            //Video Analisys
            am.ve = await MicrosoftVideoEmotion.GetVideoEmotions(downloadUrl.DownloadUrl);

            Debug.WriteLine("###Video Analisys Response");

            int i = 0;
            foreach (var v in am.ve)
            {
                Debug.WriteLine("***Segment number    --->" + v.FrameIndex);
                Debug.WriteLine("Anger from video     --->" + v.Anger);
                Debug.WriteLine("Contempt from video  --->" + v.Contempt);
                Debug.WriteLine("Disgust from video   --->" + v.Disgust);
                Debug.WriteLine("Fear from video      --->" + v.Fear);
                Debug.WriteLine("Happiness from video --->" + v.Happiness);
                Debug.WriteLine("Neutral from video   --->" + v.Neutral);
                Debug.WriteLine("Sadness from video   --->" + v.Sadness);
                Debug.WriteLine("Surprise from video  --->" + v.Surprise);
                Debug.WriteLine("            ");
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


            ////Speech to Text IBM
            var isttTextList = await IbmSpeechToText.SpeeechToText(wavPath);

            am.te.TextFromSpeech = isttTextList;
            ////Text analitycs IBM
            string text = "";
            foreach (var var in isttTextList)
            {
                text += var.Transcript + " ";
            }


            Debug.WriteLine("###Text Analisys REsponse" );
            Debug.WriteLine("****Text due to analise ---> " + text);
            Debug.WriteLine("            ");

            am.te.EmotionsFormSpeech = await IbmTextAnalisys.MakeRequests(text);

            Debug.WriteLine("Anger    --->" + am.te.EmotionsFormSpeech.Anger.ToString());
            Debug.WriteLine("Disgust  --->" + am.te.EmotionsFormSpeech.Disgust.ToString());
            Debug.WriteLine("Fear     --->" + am.te.EmotionsFormSpeech.Fear.ToString());
            Debug.WriteLine("Joy      --->" + am.te.EmotionsFormSpeech.Joy.ToString());
            Debug.WriteLine("Sadness  --->" + am.te.EmotionsFormSpeech.Sadness.ToString());
            Debug.WriteLine("            ");

            //Beyond verbal anlisys
            am.se = await BeyondVerbal.RunAnalisys(wavPath);

            Debug.WriteLine("###Sound Analisys Response");

            i = 0;
            foreach (var s in am.se.SegmentsList)
            {
                Debug.WriteLine("***Segment number:" + i++);
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
            }

             Debug.WriteLine("Mean Sound Analisys ArousalMeanMode  --->" + am.se.ArousalMeanMode);
             Debug.WriteLine("Mean Sound Analisys ArousalMeanVal   --->" + am.se.ArousalMeanVal);
             Debug.WriteLine("Mean Sound Analisys TemperMeanMode   --->" + am.se.TemperMeanMode);
             Debug.WriteLine("Mean Sound Analisys TemperMeanVal    --->" + am.se.TemperMeanVal);
             Debug.WriteLine("Mean Sound Analisys ValenceMeanMode  --->" + am.se.ValenceMeanMode);
             Debug.WriteLine("Mean Sound Analisys ValenceMeanVal)  --->" + am.se.ValenceMeanVal);


            // Cleanup.
            File.Delete(path);
            File.Delete(wavPath);

            return am;
        }
    }
}