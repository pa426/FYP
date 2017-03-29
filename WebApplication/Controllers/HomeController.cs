using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication.ApiManager;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbModelDataContext db = new DbModelDataContext();

        public ActionResult DashboardV0()
        {
            return View();
        }

        public ActionResult DashboardV1(string videoId)
        {
            var rm = db.AspVideoDetails.FirstOrDefault(x => x.VideoId == videoId);
            return View(rm);
        }

        public ActionResult DashboardV2()
        {
            var usrid = User.Identity.GetUserId();
            var rm = db.AspVideoDetails.Where(x => x.UserId.Equals(usrid)).OrderByDescending(x => x.Date).ToList();
            return View(rm);
        }

        [HttpPost]
        [ActionName("DashboardV2")]
        [AllowAnonymous]
        public ActionResult DeleteVideo(string videoId)
        {
            var vid = db.AspVideoDetails.FirstOrDefault(x => x.VideoId == videoId);
            db.AspVideoAnalysisSegments.DeleteAllOnSubmit(vid.AspVideoAnalysisSegments);
            db.AspSoundAnalisysSegments.DeleteAllOnSubmit(vid.AspSoundAnalisysSegments);
            db.AspTextAnalisysSegments.DeleteAllOnSubmit(vid.AspTextAnalisysSegments);
            db.AspVideoDetails.DeleteOnSubmit(vid);
            db.SubmitChanges();

            var usrid = User.Identity.GetUserId();
            var rm = db.AspVideoDetails.Where(x => x.UserId.Equals(usrid)).OrderByDescending(x => x.Date).ToList();
            return View(rm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DashboardV0(VideoModelList model)
        {
            //Check if search text box is filled in
            if (!ModelState.IsValid)
                return View();

            var yl = new YoutubeList();
            var ml = new VideoModelList();
            ml.VidModList = await yl.GetYoutubeList(model.VidModList[0].VideoQuery, model.VidModList[0].VideoNr,
                model.VidModList[0].VideoDates);

            ModelState.Clear();
            return View(ml);
        }

        [HttpPost]
        [AllowAnonymous]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<ActionResult> _SearchVideo(VideoModelList model, string analyzeBtn)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var newModel = model;
            switch (analyzeBtn)
            {
                case "checkAllBtn":
                    foreach (var m in newModel.VidModList)
                        m.AddVideoCb = true;
                    break;
                case "removeAllBtn":
                    foreach (var m in model.VidModList)
                        m.AddVideoCb = false;
                    break;
                case "analiseBtn":
                    foreach (var m in model.VidModList)
                        if (m.AddVideoCb)
                        {
                            var aspVideoDetail = db.AspVideoDetails.FirstOrDefault(x => x.VideoId == m.VideoId);

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
                                Debug.WriteLine("####Analise for video {0}/{1} already in database!!!", m.ChannelTitle,
                                    m.VideoId);
                            }
                        }
                    break;
            }

            Response.Write(
                "<script>alert('You will be notified as soon as the analysis is ready, it takes around 10 minutes for each minute submited for analisys!');</script>");

            return View(newModel);
        }
    }
}