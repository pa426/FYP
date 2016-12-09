using System;
using System.Collections.Generic;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class VideoModels
    {
        public string videoName  { get; set; }
        public string videoId  { get; set; }
        public string channelName  { get; set; }
        public string channelId  { get; set; }
        public string playlistName  { get; set; }
        public string playlistId  { get; set; }
    }
}