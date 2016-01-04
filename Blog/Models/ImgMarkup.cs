using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Blog.DAL;

namespace Blog.Models
{
    public class ImgMarkup
    {
        /// <summary>
        /// Заменяет все вхождения @Img(Images.Id) на html-тег <img scr="Images.Name" />
        /// </summary>
        public static string Replace(string input)
        {
            foreach (Images image in Matches(input)) {
                string matchPattern = @"(\x40)Img(\x28)" + image.Id.ToString() + @"(\x29)";
                string matchReplacement = "<img src=\"" + MvcApplication.BaseUrl + image.Name.TrimStart('/') + "\" />";
                input = Regex.Replace(input, matchPattern, matchReplacement);
            }

            return input;
        }

        /// <summary>
        /// Ищет во входной строке все @Img(Images.Id)
        /// </summary>
        /// <returns>
        /// Возвращает коллекцию объектов Images, найденных при поиске.
        /// Если соответствующие объекты не найдены, метод возвращает пустой объект коллекции.
        /// </returns>
        public static List<Images> Matches(string input)
        {
            List<Images> images = new List<Images>();
            if (input == null) return images;

            ApplicationDbContext db = new ApplicationDbContext();

            foreach (Match match in Regex.Matches(input, @"(\x40)Img(\x28)(?<image>(\d)+)(\x29)")) {
                int imageId = Convert.ToInt32(Regex.Match(match.Value, @"(\d)+").Value);
                if (db.Images.Find(imageId) == null) {
                    continue;
                } else {
                    images.Add(db.Images.Find(imageId));
                }
            }

            db.Dispose();
            return images;
        }
    }
}