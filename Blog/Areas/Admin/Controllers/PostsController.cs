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
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Admin/Posts/Details/0
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null) {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            Session["CheckForUniqueness"] = true;
            return View();
        }

        // POST: Admin/Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Title,Text")] Posts posts, List<int> tags)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            if (ModelState.IsValid) {
                posts.User = userManager.FindByName(User.Identity.Name);

                // Связь поста с изображениями.
                List<Images> images = ImgMarkup.Matches(posts.Text);
                if (images?.Count > 0) {
                    foreach (var image in images) {
                        posts.Images.Add(db.Images.Find(image.Id));
                    }
                }

                // Связь поста с тегами.
                if (tags?.Count > 0) {
                    foreach (var id in tags) {
                        Tags tag = db.Tags.Find(id);
                        if (tag == null) {
                            ModelState.AddModelError("", "Выбран несуществующий тег.");
                            return View(posts);
                        }
                        posts.Tags.Add(tag);
                    }
                }
                
                db.Posts.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        // GET: Admin/Posts/Edit/0
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null) {
                return HttpNotFound();
            }

            Session["CheckForUniqueness"] = false;

            if (!User.IsInRole("admin") && (User.IsInRole("editor") && (posts.User.Id != User.Identity.GetUserId()))) {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования поста.");
            }

            return View(posts);
        }

        // POST: Admin/Posts/Edit/0
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Title,Text")] Posts posts, List<int> tags)
        {
            Posts currentPost = db.Posts.Find(posts.Id);
            if (currentPost == null) {
                return HttpNotFound();
            }

            // Редактор может редактировать только свой пост, а администратор все.
            if ((User.IsInRole("editor") && (currentPost.User.Id == User.Identity.GetUserId())) || User.IsInRole("admin")) {
                if (ModelState.IsValid) {
                    #region
                    // Обновленные данные.
                    currentPost.Date = posts.Date;
                    currentPost.Title = posts.Title;
                    currentPost.Text = posts.Text;

                    // Удалить предшествующие изображения и теги.
                    currentPost.Images.Clear();
                    currentPost.Tags.Clear();

                    // Связь поста с изображениями.
                    List<Images> images = ImgMarkup.Matches(posts.Text);
                    if (images?.Count > 0) {
                        foreach (var image in images) {
                            currentPost.Images.Add(db.Images.Find(image.Id));
                        }
                    }

                    // Связь поста с тегами.
                    if (tags?.Count > 0) {
                        foreach (var id in tags) {
                            Tags tag = db.Tags.Find(id);
                            if (tag == null) {
                                ModelState.AddModelError("", "Выбран несуществующий тег.");
                                return View(posts);
                            }
                            currentPost.Tags.Add(tag);
                        }
                    }

                    // Обновление.
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    #endregion
                }
            }
            else {
                ModelState.AddModelError("", "У вас недостаточно прав для редактирования поста.");
            }

            return View(posts);
        }

        // GET: Admin/Posts/Delete/0
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null) {
                return HttpNotFound();
            }

            if (!User.IsInRole("admin") && (User.IsInRole("editor") && (posts.User.Id != User.Identity.GetUserId()))) {
                ModelState.AddModelError("", "У вас недостаточно прав для удаления поста.");
            }

            return View(posts);
        }

        // POST: Admin/Posts/Delete/0
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.Posts.Find(id);

            // Редактор может удалить только свой пост, а администратор все.
            if ((User.IsInRole("editor") && (posts.User.Id == User.Identity.GetUserId())) || User.IsInRole("admin")) {
                db.Posts.Remove(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ModelState.AddModelError("", "У вас недостаточно прав для удаления поста.");
                return RedirectToAction("Delete", posts);
            }
        }

        public JsonResult CheckExistTitle(string title)
        {
            bool result = true;
            if ((bool)Session["CheckForUniqueness"]) {
                Posts post = db.Posts.FirstOrDefault(p => p.Title == title);
                result = (post == null) ? true : false;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _PartialSelectTags(List<Tags> selectedTags)
        {
            List<SelectListItem> selectOptions = new List<SelectListItem>();
            foreach (var item in db.Tags) {
                selectOptions.Add(new SelectListItem() {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                    Selected = (selectedTags?.FirstOrDefault(t => t.Id == item.Id) == null) ? false : true
                });
            }

            return PartialView(selectOptions);
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
