using System.ComponentModel.DataAnnotations;


namespace WebApplication.Models
{
    public class VideoModels
    {
       
        [Required] public string VideoQuery { get; set; }
        public string VideoNr { get; set; }
        public string VideoDateFrom { get; set; }
        public string VideoDateTo { get; set; }
        public string VideoName  { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }
      
       
    }

}