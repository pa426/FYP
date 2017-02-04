using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frapper;
using YoutubeExtractor;

namespace WebApplication.ApiManager
{
    public class YoutubeSoundAnalisys
    {
        public static async Task TextToSpeach(string id)
        {
            IEnumerable<VideoInfo> videoInfos =
                DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=" + id, false);

            //var video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);

            var path = DownloadAudioQuick(videoInfos);
            await Mp4ToWav(path);

         
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private static string DownloadAudioQuick(IEnumerable<VideoInfo> videoInfos)
        {
            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RemoveIllegalPathCharacters(video.Title) + ".mp4");
            var audioDownloader = new VideoDownloader(video, path);

            audioDownloader.Execute();

            return path;
        }


        private static async Task Mp4ToWav(string path)
        {
            FFMPEG ffmpeg = new FFMPEG();

            // Convert to wav.
            string wavPath = path.Replace(".mp4", ".wav");
            ffmpeg.RunCommand("-i \"" + path + "\" -acodec pcm_s16le -ac 1 -ar 16000 \"" + wavPath + "\"");

            //Video Analisys
            //var videoEmotions = await MicrosoftVideoEmotion.GetVideoEmotions(downloadUrl);

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
            //var isttTextList = await IbmSpeechToText.SpeeechToText(wavPath);

            ////Text analitycs IBM
            //string text = "";
            //foreach (var var in isttTextList)
            //{
            //    text += var.Transcript + " ";
            //}


            ////Debug.WriteLine("****Text due to analise = " + text);

            ////Text analitycs IBM
            //var ita = IbmTextAnalisys.MakeRequests(text);

            ////Debug.WriteLine(" *Anger:" + ita.Result.Anger.ToString());
            ////Debug.WriteLine(" *Disgust:" + ita.Result.Disgust.ToString());
            ////Debug.WriteLine(" *Fear:" + ita.Result.Fear.ToString());
            ////Debug.WriteLine(" *Joy:" + ita.Result.Joy.ToString());
            ////Debug.WriteLine(" *Sadness:" + ita.Result.Sadness.ToString());

            //Beyond verbal anlisys
            await BeyondVerbal.RunAnalisys(wavPath);

            // Cleanup.
            File.Delete(path);
            File.Delete(wavPath);
        }
    }
}