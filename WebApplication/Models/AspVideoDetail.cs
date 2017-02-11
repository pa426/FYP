namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspVideoDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspVideoDetail()
        {
            AspSoundAnalisysSegments = new HashSet<AspSoundAnalisysSegments>();
            AspTextAnalisysSegments = new HashSet<AspTextAnalisysSegments>();
            AspVideoAnalysisSegments = new HashSet<AspVideoAnalysisSegments>();
        }

        [Key]
        [StringLength(50)]
        public string VideoId { get; set; }

        [StringLength(50)]
        public string VideoTitle { get; set; }

        [StringLength(50)]
        public string ChannelId { get; set; }

        [StringLength(50)]
        public string ChannelTitle { get; set; }

        public DateTime? PublishedAt { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspSoundAnalisysSegments> AspSoundAnalisysSegments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspTextAnalisysSegments> AspTextAnalisysSegments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspVideoAnalysisSegments> AspVideoAnalysisSegments { get; set; }
    }
}
