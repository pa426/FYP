using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace WebApplication.ApiManager
{
    public class MicrosoftVideoEmotion
    {


        public async Task<VideoEmotionRecognitionOperation> VidAnalisys()
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(ApiKeys.microsoftEmotionSubKey);
            VideoEmotionRecognitionOperation videoOperation;

            var videoUrl =
                "https://d2v9y0dukr6mq2.cloudfront.net/video/preview/VGqRaYUogil0szy2p/sad-face-of-young-guy-man-cry-with-tears_rbrfjci8__PM.mp4";

            Debug.WriteLine("Video Analyse starting________________________________________________________");
            videoOperation = await emotionServiceClient.RecognizeInVideoAsync(videoUrl);
            return videoOperation;
        }

        public async Task<VideoOperationResult> VidAnalisysResult(VideoEmotionRecognitionOperation videoOperation)
        {
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
                ((VideoOperationInfoResult<VideoAggregateRecognitionResult>)operationResult).ProcessingResult;


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

    }
}