using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Blog.DAL;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "editor, admin")]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Comments
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        // GET: Admin/Comments/Post/0
        public ActionResult Post(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Comments> comments = db.Comments.Where(c => c.Post.Id == id).ToList();
            if (comments.Count == 0) {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Admin/Comments/Details/0
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null) {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Admin/Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Comment")] Comments comments)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            if (ModelState.IsValid) {
                comments.User = userManager.FindByName(User.Identity.Name);
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comments);
        }

        // GET: Admin/Comments/Edit/0
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null) {
                return HttpNotFound();
            }

            if (comments.User.Id != User.Identity.GetUserId()) {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования комментария.");
            }

            return View(comments);
        }

        // POST: Admin/Comments/Edit/0
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Comment")] Comments comments)
        {
            Comments currentComment = db.Comments.Find(comments.Id);
            if (currentComment == null) {
                return HttpNotFound();
            }

            // Редактировать комментарий может только его автор.
            if (currentComment.User.Id == User.Identity.GetUserId()) {
                if (ModelState.IsValid) {
                    db.Entry(comments).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            } else {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования комментария.");
            }

            return View(comments);
        }

        // GET: Admin/Comments/Delete/0
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null) {
                return HttpNotFound();
            }

            if (!User.IsInRole("admin")) {
                ModelState.AddModelError("", "У вас недостаточно прав для удаления комментария.");
            }

            return View(comments);
        }

        // POST: Admin/Comments/Delete/0
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.Comments.Find(id);

            // Удалить комментарий может только администратор.
            if (User.IsInRole("admin")) {
                db.Comments.Remove(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ModelState.AddModelError("", "У вас недостаточно прав для удаления комментария.");
                return View("Delete", comments);
            }
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
