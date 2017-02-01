using System;
using System.Collections.Generic;
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
        public async Task TextToSpeach()
        {
            IEnumerable<VideoInfo> videoInfos =
                DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=tTl_2hVUmbg", false);

            string path = DownloadAudioQuick(videoInfos);
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


            //Speech to Text Microsoft
            //var p = new SpeechToText();
            //await p.SpeechToTextTransformation(wavPath, "en-US", speechSubscriptionKey);


            ////Speech to Text IBM
            //await IbmSpeechToText.SpeeechToText(wavPath);

            //Beyond verbal anlisys
            //await BeyondVerbal.RunAnalisys();

            // Cleanup.
            File.Delete(path);
            File.Delete(wavPath);
        }
    }
}