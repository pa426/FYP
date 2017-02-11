using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ResultModels
    {
        public AspVideoDetail VideoDetail { get; set; }
        public List<AspVideoAnalysisSegment> VideoAnalysisSegments { get; set; }
        public List<AspTextAnalisysSegment> TextAnalisysSegments { get; set; }
        public List<AspSoundAnalisysSegment> SoundAnalisysSegments { get; set; }

    }
}