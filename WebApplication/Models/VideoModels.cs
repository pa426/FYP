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
        public string VideoTitle  { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }
        public bool checkBox { get; set; }

        public DateTime[] SeparateDates()
        {
            DateTime[] dates = new DateTime[2];

            var date1 = VideoDates.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);

            dates[0] = DateTime.ParseExact(date1[0], "dd/MM/yyyy", null);
            dates[1] = DateTime.ParseExact(date1[1], "dd/MM/yyyy", null);
            
            System.Diagnostics.Debug.WriteLine(date1[0]+dates[1]);
            System.Diagnostics.Debug.WriteLine(dates[0]);
            System.Diagnostics.Debug.WriteLine(dates[1]);

            return dates;
        }
       
    }

}