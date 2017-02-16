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
    
    public partial class AspVideoDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspVideoDetail()
        {
            this.AspSoundAnalisysSegments = new HashSet<AspSoundAnalisysSegment>();
            this.AspTextAnalisysSegments = new HashSet<AspTextAnalisysSegment>();
            this.AspVideoAnalysisSegments = new HashSet<AspVideoAnalysisSegment>();
        }
    
        [Key]
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public Nullable<System.DateTime> PublishedAt { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspSoundAnalisysSegment> AspSoundAnalisysSegments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspTextAnalisysSegment> AspTextAnalisysSegments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspVideoAnalysisSegment> AspVideoAnalysisSegments { get; set; }
    }
}