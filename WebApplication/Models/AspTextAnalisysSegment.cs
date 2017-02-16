//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspTextAnalisysSegment
    {
        [Key]
        public int TextSegmentId { get; set; }
        public string VideoId { get; set; }
        public Nullable<int> TextSegmentIndex { get; set; }
        public string TextFromSpeech { get; set; }
        public Nullable<double> Anger { get; set; }
        public Nullable<double> Disgust { get; set; }
        public Nullable<double> Fear { get; set; }
        public Nullable<double> Joy { get; set; }
        public Nullable<double> Sadness { get; set; }
    
        public virtual AspVideoDetail AspVideoDetail { get; set; }
    }
}