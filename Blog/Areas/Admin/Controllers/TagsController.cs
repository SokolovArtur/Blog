using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "editor, admin")]
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Tags
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }

        // GET: Admin/Tags/Details/0
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tags tags = db.Tags.Find(id);
            if (tags == null) {
                return HttpNotFound();
            }
            return View(tags);
        }

        // GET: Admin/Tags/Create
        public ActionResult Create()
        {
            Session["CheckForUniqueness"] = true;
            return View();
        }

        // POST: Admin/Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Tags tags)
        {
            if (ModelState.IsValid) {
                db.Tags.Add(tags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tags);
        }

        // GET: Admin/Tags/Edit/0
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tags tags = db.Tags.Find(id);
            if (tags == null) {
                return HttpNotFound();
            }

            Session["CheckForUniqueness"] = false;

            if (!User.IsInRole("admin")) {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования тега.");
            }

            return View(tags);
        }

        // POST: Admin/Tags/Edit/0
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Tags tags)
        {
            // Редактировать тег может только администратор.
            if (User.IsInRole("admin")) {
                if (ModelState.IsValid) {
                    db.Entry(tags).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            } else {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования тега.");
            }
            
            return View(tags);
        }

        // GET: Admin/Tags/Delete/0
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tags tags = db.Tags.Find(id);
            if (tags == null) {
                return HttpNotFound();
            }
            
            if (tags.Posts.Count > 0) {
                ModelState.AddModelError("", "Тег используется. Его нельзя удалить.");
            }

            return View(tags);
        }

        // POST: Admin/Tags/Delete/0
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tags tags = db.Tags.Find(id);

            // Если тег используется, то его нельзя удалить.
            if (tags.Posts.Count > 0) {
                ModelState.AddModelError("", "Тег используется. Его нельзя удалить.");
                return View("Delete", tags);
            } else {
                db.Tags.Remove(tags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public JsonResult CheckExistTag(string name)
        {
            bool result = true;
            if ((bool)Session["CheckForUniqueness"]) {
                Tags tag = db.Tags.FirstOrDefault(t => t.Name == name);
                result = (tag == null) ? true : false;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
