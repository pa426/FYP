using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{

    public class VideoModelList
    {
        public List<VideoModel> VidModList { get; set; }
    }

    
    public class VideoModel
    {

        [Display(Name = "Video description")]
        [Required]
        public string VideoQuery { get; set; }
        [Required]
        public string VideoNr { get; set; }
        public string VideoDates { get; set; }
        public string VideoTitle { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }
        public string VideoLocation { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Add video")]
        public bool AddVideoCb { get; set; }

    }
}