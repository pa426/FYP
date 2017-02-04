using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class AnalysisModels
    {
        public VideoEmotions ve { get; set; }
        public TextEmotions te { get; set; }
        public SoundEmotions se { get; set; }
    }

    public class VideoEmotions
    {
        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }
    }

    public class TextEmotions
    {
        public List<TextFromSpeech> TextFromSpeech { get; set; }
        public EmotionsFormSpeech EmotionsFormSpeech { get; set; }
    }

    public class TextFromSpeech
    {
        public string Transcript { get; set; }
        public decimal Confidence { get; set; }
    }

    public class EmotionsFormSpeech

    {
        public float Anger { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Joy { get; set; }
        public float Sadness { get; set; }
    }

    public class SoundEmotions
    {
        public List<SoundEmotionsSegment> SegmentsList { get; set; } = new List<SoundEmotionsSegment>();

        public float  TemperMeanVal    { get; set; }
        public string TemperMeanMode   { get; set; }
        public float  ValenceMeanVal   { get; set; }
        public string ValenceMeanMode   { get; set; }
        public float  ArousalMeanVal   { get; set; }
        public string ArousalMeanMode   { get; set; }
    }

    public class SoundEmotionsSegment
    {
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