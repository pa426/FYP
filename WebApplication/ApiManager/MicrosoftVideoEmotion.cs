using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace WebApplication.ApiManager
{
    public class MicrosoftVideoEmotion
    {
        public static async Task<List<Models.VideoEmotions>> GetVideoEmotions(string videoUrl)
        {
            var upload = await VidAnalisys(videoUrl);
            return await VidAnalisysResult(upload);
        }

        public static async Task<VideoEmotionRecognitionOperation> VidAnalisys(string videoUrl)
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(ApiKeys.microsoftEmotionSubKey);
            VideoEmotionRecognitionOperation videoOperation;
            Debug.WriteLine("Video Analyse starting________________________________________________________");
            videoOperation = await emotionServiceClient.RecognizeInVideoAsync(videoUrl);
            return videoOperation;
        }

        public static async Task<List<Models.VideoEmotions>> VidAnalisysResult(VideoEmotionRecognitionOperation videoOperation)
        {
            var videoEmotionsList = new List<Models.VideoEmotions>();
           
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(ApiKeys.microsoftEmotionSubKey);
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


            int i = 0;

            foreach (var frag in operationResultemotionRecognitionJsonString.Fragments)
            {
                if (frag.Events != null)
                {
                    foreach (var x in frag.Events)
                    {
                        foreach (var y in x)
                        {
                            var videoEmotions = new Models.VideoEmotions();
                            videoEmotions.FrameIndex = i++;
                            videoEmotions.Anger = float.Parse(y.WindowMeanScores.Anger.ToString());
                            videoEmotions.Contempt = float.Parse(y.WindowMeanScores.Contempt.ToString());
                            videoEmotions.Disgust = float.Parse(y.WindowMeanScores.Disgust.ToString());
                            videoEmotions.Fear = float.Parse(y.WindowMeanScores.Fear.ToString());
                            videoEmotions.Happiness = float.Parse(y.WindowMeanScores.Happiness.ToString());
                            videoEmotions.Neutral = float.Parse(y.WindowMeanScores.Neutral.ToString());
                            videoEmotions.Sadness = float.Parse(y.WindowMeanScores.Sadness.ToString());
                            videoEmotions.Surprise = float.Parse(y.WindowMeanScores.Surprise.ToString());
                            if (videoEmotions.Anger != 0 && videoEmotions.Contempt != 0 && videoEmotions.Disgust != 0 && videoEmotions.Fear != 0 && videoEmotions.Happiness != 0 && videoEmotions.Neutral != 0 && videoEmotions.Sadness != 0 && videoEmotions.Surprise != 0)
                            {
                                videoEmotionsList.Add(videoEmotions);
                            }
                           
                        }
                    }
                }
            }

            return videoEmotionsList;
        }
    }
}