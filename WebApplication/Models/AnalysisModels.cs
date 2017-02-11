using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication.Models
{
    public class AnalysisModels
    {
        public VideoModel VideoDetails { get; set; } = new VideoModel();
        public List<VideoEmotions> VideoEmotion { get; set; } = new List<VideoEmotions>();
        public VideoEmotions VideoEmotionMean { get; set; } = new VideoEmotions();
        public List<EmotionsFormSpeech> TextEmotion { get; set; } = new List<EmotionsFormSpeech>();
        public EmotionsFormSpeech TextEmotionMean { get; set; } = new EmotionsFormSpeech();
        public List<SoundEmotions> SoundEmotion { get; set; } = new List<SoundEmotions>();
        public SoundEmotions SoundEmotionMean { get; set; } = new SoundEmotions();

    }

    public class VideoEmotions
    {
        public int VideoSegmentIndex { get; set; }
        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }

        public static explicit operator VideoEmotions(Task v)
        {
            throw new NotImplementedException();
        }
    }

    public class EmotionsFormSpeech

    {
        public int SpeechSegmentIndex { get; set; }
        public string TextFromSpeech { get; set; }
        public float Anger { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Joy { get; set; }
        public float Sadness { get; set; }
    }

    public class SoundEmotions
    {
        public int SoundSegmentIndex { get; set; }
        public float Offset { get; set; }
        public float Duration { get; set; }
        public float TemperVal { get; set; }
        public string TemperMode { get; set; }
        public float ValenceVal { get; set; }
        public string ValenceMode { get; set; }
        public float ArousalVal { get; set; }
        public string ArousalMode { get; set; }
        public string Gender { get; set; }
        public string MoodPrimary { get; set; }
        public string MoodSecondary { get; set; }
        public string CompositePrimary { get; set; }
        public string CompositeSecondary { get; set; }
    }
}
