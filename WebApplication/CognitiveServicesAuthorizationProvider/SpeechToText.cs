
namespace WebApplication.CognitiveServicesAuthorizationProvider
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Microsoft.Bing.Speech;

    /// <summary>
    /// This sample program shows how to use <see cref="SpeechClient"/> APIs to perform speech recognition.
    /// </summary>
    public class SpeechToText
    {
        private static readonly Uri ShortPhraseUrl = new Uri(@"wss://speech.platform.bing.com/api/service/recognition");

        private static readonly Uri LongDictationUrl =
            new Uri(@"wss://speech.platform.bing.com/api/service/recognition/continuous");

        private static readonly Task CompletedTask = Task.FromResult(true);

        private readonly CancellationTokenSource cts = new CancellationTokenSource();


        public Task OnPartialResult(RecognitionPartialResult args)
        {
            Debug.WriteLine("--- Partial result received by OnPartialResult ---");

            // Print the partial response recognition hypothesis.
            Debug.WriteLine(args.DisplayText);

            return CompletedTask;
        }


        public async Task OnRecognitionResult(RecognitionResult args)
        {
            var response = args;

            Debug.WriteLine("--- Phrase result received by OnRecognitionResult ---");

            // Print the recognition status.
            Debug.WriteLine("***** Phrase Recognition Status = [{0}] ***", response.RecognitionStatus);
            if (response.Phrases != null)
            {
                foreach (var result in response.Phrases)
                {
                    // Print the recognition phrase display text.
                    Debug.WriteLine("{0} (Confidence:{1})", result.DisplayText, result.Confidence);

                    //await TextAnalytics.MakeRequests(result.DisplayText);


                }
            }
        }


        public async Task SpeechToTextTransformation(string audioFile, string locale, string subscriptionKey)
        {
            // create the preferences object
            var preferences = new Preferences(locale, LongDictationUrl,
                new CognitiveServicesAuthorizationProvider(subscriptionKey));

            // Create a a speech client
            using (var speechClient = new SpeechClient(preferences))
            {
                speechClient.SubscribeToPartialResult(this.OnPartialResult);
                speechClient.SubscribeToRecognitionResult(this.OnRecognitionResult);

                // create an audio content and pass it a stream.
                using (var audio = new FileStream(audioFile, FileMode.Open, FileAccess.Read))
                {
                    var deviceMetadata = new DeviceMetadata(DeviceType.Near, DeviceFamily.Desktop, NetworkType.Ethernet,
                        OsName.Windows, "1607", "Dell", "T3600");
                    var applicationMetadata = new ApplicationMetadata("SpeechApi", "1.0.0");
                    var requestMetadata = new RequestMetadata(Guid.NewGuid(), deviceMetadata, applicationMetadata,
                        "SpeechApiService");

                    await speechClient.RecognizeAsync(new SpeechInput(audio, requestMetadata), this.cts.Token)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}