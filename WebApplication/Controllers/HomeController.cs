﻿using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.ApiManager;
using WebApplication.Models;



namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {

        public async Task<ActionResult> DashboardV0()
        {
            var path = @"C:\Users\Alexandru\Desktop\test4.wav";
            var speeToText = IbmSpeechToText.SpeeechToText(path);
            return View();
        }

        public ActionResult DashboardV1()
        {
            return View();
        }

        public ActionResult DashboardV2()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DashboardV0(VideoModelList model)
        {
            //Check if search text box is filled in
            if (!ModelState.IsValid)
            {
                return View();
            }

            var yl = new YoutubeList();
            var ml = new VideoModelList();
            ml.VidModList = await yl.GetYoutubeList(model.VidModList[0].VideoQuery, model.VidModList[0].VideoNr, model.VidModList[0].VideoDates);

            ModelState.Clear();
            return View(ml);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> _SearchVideo(VideoModelList model, string analyzeBtn)
        {
            var newModel = model;
            switch (analyzeBtn)
            {
                case "checkAllBtn":
                    foreach (var m in newModel.VidModList)
                    {
                        m.AddVideoCb = true;
                    }
                    break;
                case "removeAllBtn":
                    foreach (var m in model.VidModList)
                    {
                        m.AddVideoCb = false;
                    }
                    break;
                case "analiseBtn":
                    foreach (var m in model.VidModList)
                    {
                        if (m.AddVideoCb == true)
                        {
                            Debug.WriteLine("***{0} Analise started for video {1}:", m.ChannelTitle, m.VideoId);
                            await YoutubeSoundAnalisys.TextToSpeach(m.VideoId);


                        }
                    }
                    break;
            }

            return View(newModel);
        }

    }
}