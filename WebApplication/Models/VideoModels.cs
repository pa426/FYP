using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace WebApplication.Models
{
    public class VideoModels
    {
       
        [Required] public string VideoQuery { get; set; }
        [Required] public string VideoNr { get; set; }
        public string VideoDates { get; set; }
        public string VideoName  { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }

        public DateTime[] SeparateDates()
        {
  
            var date1 = VideoDates.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);

            DateTime[] dates = new DateTime[2];
            dates[0] = DateTime.ParseExact(date1[0], "MM/dd/yyyy", null);
            dates[1] = DateTime.ParseExact(date1[1], "MM/dd/yyyy", null);
            
            

            System.Diagnostics.Debug.WriteLine(date1[0]+dates[1]);
            System.Diagnostics.Debug.WriteLine(dates[0]);
            System.Diagnostics.Debug.WriteLine(dates[1]);
            return dates;
            }
       
    }

}