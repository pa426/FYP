using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using WebApplication.ApiManager;
using WebApplication.Models;
using Microsoft.AspNet.Identity;


namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult DashboardV0()
        {
            return View();
        }

        public ActionResult DashboardV1(string videoId)
        {
            var rm = db.AspVideoDetails.Find(videoId);
            return View(rm);
        }

        public ActionResult DashboardV2()
        {
            var usrid = User.Identity.GetUserId();
            List<AspVideoDetail> rm = db.AspVideoDetails.Where(x => x.UserId.Equals(usrid)).ToList();
            return View(rm);
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
            ml.VidModList = await yl.GetYoutubeList(model.VidModList[0].VideoQuery, model.VidModList[0].VideoNr,
                model.VidModList[0].VideoDates);

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
                            var aspVideoDetail = db.AspVideoDetails.Find(m.VideoId);

                            if (aspVideoDetail == null)
                            {
                                Debug.WriteLine("***{0} Analise started for video {1}:", m.ChannelTitle, m.VideoId);
                                var yt = new YoutubeSoundAnalisys();
                                m.UserId = User.Identity.GetUserId();
                                //await yt.TextToSpeach(m);
                                HostingEnvironment.QueueBackgroundWorkItem(ct => yt.TextToSpeach(m));
                            }
                            else
                            {
                                Debug.WriteLine("####Analise for video {0}/{1} already in database!!!", m.ChannelTitle, m.VideoId);
                            }
                        }
                    }
                    break;
            }

            return View(newModel);
        }
    }
}