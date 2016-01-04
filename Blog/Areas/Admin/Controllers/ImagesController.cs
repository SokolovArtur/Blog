using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "editor, admin")]
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string imagesDir = "/Images";

        // GET: Admin/Images
        public ActionResult Index()
        {
            return View(db.Images.ToList());
        }

        // GET: Admin/Images/Load
        public ActionResult Load()
        {
            return View();
        }

        // POST: Admin/Images/Load
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Load(HttpPostedFileBase file)
        {
            Images images = new Images();

            if (file != null) {
                string expansion = System.IO.Path.GetExtension(file.FileName).TrimStart('.');

                if (!(expansion == "bmp" || expansion == "dib" || expansion == "rle" ||
                    expansion == "jpg" || expansion == "jfif" || expansion == "jpe" || expansion == "jpeg" ||
                    expansion == "gif" || expansion == "png" ||
                    expansion == "tiff" || expansion == "tif")) {
                    ModelState.AddModelError("", "Файл изображения должен иметь расширение bmp, jpg, gif, png или tiff.");
                }

                #region
                try
                {
                    // Поле Images.Id применяется как уникальное имя файла на сервере.
                    // Чтобы получить Id нужно добавить запись в таблицу.
                    images.Name = expansion;
                    db.Images.Add(images);
                    db.SaveChanges();
                }
                catch (System.Data.DataException)
                {
                    ModelState.AddModelError("", "Возникла ошибка при сохранении информации о файле в таблицу.");
                    return View(images);
                }

                Images savedImage = db.Images.Find(images.Id);
                string fileName = savedImage.Id.ToString() + "." + expansion;

                // Загрузить файл на сервер. При ошибке удалить запись из таблицы.
                try
                {
                    file.SaveAs(Server.MapPath("~/" + imagesDir.Trim('/') + "/" + fileName));
                }
                catch (System.IO.IOException)
                {
                    db.Images.Remove(savedImage);
                    db.SaveChanges();

                    ModelState.AddModelError("", "Возникла ошибка при загрузке файла на сервер.");
                    return View(images);
                }

                // Обновить запись. В поле Images.Name указать название загруженного файла.
                try
                {
                    savedImage.Name = "/" + imagesDir.Trim('/') + "/" + fileName;
                    db.SaveChanges();
                }
                catch (System.Data.DataException)
                {
                    ModelState.AddModelError("", "Возникла ошибка при сохранении информации о файле в таблицу.");
                    return View(images);
                }
                #endregion

                return RedirectToAction("Index");
            } else {
                ModelState.AddModelError("", "Выберите файл.");
            }

            return View(images);
        }

        // GET: Admin/Images/Delete/0
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Images images = db.Images.Find(id);
            if (images == null) {
                return HttpNotFound();
            }

            if (images.Posts.Count > 0) {
                ModelState.AddModelError("", "Изображение используется. Его нельзя удалить.");
            }

            return View(images);
        }

        // POST: Admin/Images/Delete/0
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Images images = db.Images.Find(id);

            // Если изображение используется, то его нельзя удалить.
            if (images.Posts.Count > 0) {
                ModelState.AddModelError("", "Изображение используется. Его нельзя удалить.");
                return View("Delete", images);
            } else {
                // Удалить файл с сервера.
                try
                {
                    string filePath = Server.MapPath("~/" + images.Name.TrimStart('/'));
                    if (System.IO.File.Exists(filePath)) {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch (System.IO.IOException)
                {
                    ModelState.AddModelError("", "Возникла ошибка при удалении файла с сервера.");
                    return View("Delete", images);
                }

                // Удалить запись из таблицы.
                db.Images.Remove(images);
                db.SaveChanges();

                return RedirectToAction("Index");
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
