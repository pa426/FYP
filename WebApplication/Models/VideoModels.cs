using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace WebApplication.Models
{
    public class VideoModels
    {
       
        [Display (Name="Video description")]
        [Required]
        public string VideoQuery { get; set; }
        [Required]
        public string VideoNr { get; set; }
        public string VideoDates { get; set; }
        public string VideoTitle  { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }
        [Display(Name = "Add video")]
        public bool AddVideoCb { get; set; }

        
    }

}