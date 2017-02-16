using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using WebApplication.Models;

namespace WebApplication.ApiManager
{
    public class MicrosoftVideoEmotion
    {
        public async Task<List<AspVideoAnalysisSegment>> GetVideoEmotions(string videoUrl)
        {
            var upload = await VidAnalisys(videoUrl);
            return await VidAnalisysResult(upload);
        }

        public async Task<VideoEmotionRecognitionOperation> VidAnalisys(string videoUrl)
        {
            var emotionServiceClient = new EmotionServiceClient(ApiKeys.microsoftEmotionSubKey);
            Debug.WriteLine("Video Analyse starting________________________________________________________");
            var videoOperation = await emotionServiceClient.RecognizeInVideoAsync(videoUrl);
            return videoOperation;
        }

        public async Task<List<AspVideoAnalysisSegment>> VidAnalisysResult(VideoEmotionRecognitionOperation videoOperation)
        {
            var videoEmotionsList = new List<AspVideoAnalysisSegment>();
           
            var emotionServiceClient = new EmotionServiceClient(ApiKeys.microsoftEmotionSubKey);
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
                    Debug.WriteLine("Analisys still running please wait_");
                }

                Task.Delay(30000).Wait(); //(0.5 min)
            }


            var operationResultemotionRecognitionJsonString =
                ((VideoOperationInfoResult<VideoAggregateRecognitionResult>) operationResult).ProcessingResult;


            var i = 0;

            foreach (var frag in operationResultemotionRecognitionJsonString.Fragments)
            {
                if (frag.Events == null) continue;
                foreach (var x in frag.Events)
                {
                    videoEmotionsList.AddRange(x.Select(y => new AspVideoAnalysisSegment
                    {
                        VideoSegmentIndex = i++, Anger = float.Parse(y.WindowMeanScores.Anger.ToString()), Contempt = float.Parse(y.WindowMeanScores.Contempt.ToString()), Disgust = float.Parse(y.WindowMeanScores.Disgust.ToString()), Fear = float.Parse(y.WindowMeanScores.Fear.ToString()), Happiness = float.Parse(y.WindowMeanScores.Happiness.ToString()), Neutral = float.Parse(y.WindowMeanScores.Neutral.ToString()), Sadness = float.Parse(y.WindowMeanScores.Sadness.ToString()), Surprise = float.Parse(y.WindowMeanScores.Surprise.ToString())
                    }).Where(videoEmotions => videoEmotions.Anger != 0 && videoEmotions.Contempt != 0 && videoEmotions.Disgust != 0 && videoEmotions.Fear != 0 && videoEmotions.Happiness != 0 && videoEmotions.Neutral != 0 && videoEmotions.Sadness != 0 && videoEmotions.Surprise != 0));
                }
            }

            return videoEmotionsList;
        }
    }
}