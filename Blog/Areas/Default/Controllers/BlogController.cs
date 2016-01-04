using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Blog.DAL;
using Blog.Models;
using Blog.Areas.Default.Models;

namespace Blog.Areas.Default.Controllers
{
    public class BlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? page)
        {
            // Ограничение по времени и сортировка.
            List<Posts> posts = db.Posts.Where(p => p.Date <= DateTime.Today).OrderByDescending(p => p.Date).ToList();
            // Постраничная навигация.
            IPagedList<Posts> pagedPosts = posts.ToPagedList((page ?? 1), 10);

            BlogViewModel viewModel = new BlogViewModel();
            viewModel.PostsList = pagedPosts;
            return View(viewModel);
        }

        public ActionResult Post(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts post = db.Posts.Find(id);
            if (post == null) {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post([Bind(Include = "Id,Date,Comment")] Comments form, int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts post = db.Posts.Find(id);
            if (post == null) {
                return HttpNotFound();
            }

            /*
            Вывод ошибки в общий блок.
            if (form.Comment.ToLower().Contains("admin")) {
                ModelState.AddModelError("", "В нашем блоге нельзя упоминать всевышнего.");
            }
            */

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            if (ModelState.IsValid) {
                Comments comment = new Comments() {
                    Date = DateTime.Now,
                    Comment = form.Comment,
                    User = userManager.FindByName(User.Identity.Name)
                };
                post.Comments.Add(comment);

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Post", new { id = post.Id });
            }

            return View(post);
        }

        /// <summary>
        /// Это частичное представление выводит список комментариев для поста.
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        public PartialViewResult _PartialCommentsToPost(int id)
        {
            ICollection<Comments> comments = db.Posts.Find(id).Comments;
            return PartialView(comments);
        }
    }
}