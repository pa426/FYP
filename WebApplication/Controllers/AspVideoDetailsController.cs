using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AspVideoDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AspVideoDetails
        public ActionResult Index()
        {
            return View(db.AspVideoDetails.ToList());
        }

        // GET: AspVideoDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspVideoDetails aspVideoDetail = db.AspVideoDetails.Find(id);
            if (aspVideoDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspVideoDetail);
        }

        // GET: AspVideoDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspVideoDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VideoId,VideoTitle,ChannelId,ChannelTitle,PublishedAt,UserId")] AspVideoDetails aspVideoDetails)
        {
            if (ModelState.IsValid)
            {
                db.AspVideoDetails.Add(aspVideoDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspVideoDetails);
        }

        // GET: AspVideoDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspVideoDetails aspVideoDetail = db.AspVideoDetails.Find(id);
            if (aspVideoDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspVideoDetail);
        }

        // POST: AspVideoDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,VideoTitle,ChannelId,ChannelTitle,PublishedAt,UserId")] AspVideoDetail aspVideoDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspVideoDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspVideoDetail);
        }

        // GET: AspVideoDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspVideoDetails aspVideoDetail = db.AspVideoDetails.Find(id);
            if (aspVideoDetail == null)
            {
                return HttpNotFound();
            }
            return View(aspVideoDetail);
        }

        // POST: AspVideoDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspVideoDetails aspVideoDetail = db.AspVideoDetails.Find(id);
            db.AspVideoDetails.Remove(aspVideoDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
