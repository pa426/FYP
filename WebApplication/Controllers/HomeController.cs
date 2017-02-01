using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.ApiManager;
using WebApplication.Models;



namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
     
        public ActionResult DashboardV0()
        {

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
        public async Task<ActionResult> DashboardV0(VideoModels model)
        {
            //Check if search text box is filled in
            if (!ModelState.IsValid)
            {
                return View();
            }

            var yl = new YoutubeList();
            var ytl = await yl.GetYoutubeList(model.VideoQuery, model.VideoNr, model.VideoDates);

            ViewBag.Videos = ytl;
            ModelState.Clear();
            return View();
        }


        
    }


}