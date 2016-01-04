using PagedList;
using Blog.Models;

namespace Blog.Areas.Default.Models
{
    public class BlogViewModel
    {
        enum TileLengthLimit
        {
            Square = 20,
            Wide = 41,
            Large = 153,
            Big = 395,
            Super = 724
        }

        public IPagedList<Posts> PostsList { get; set; }

        /// <summary>
        /// В зависимости от длины текста помещаемый в блок tile возвращает соответствующий ему css-класс.
        /// Если текста мало – маленький блок, много – блок побольше.
        /// </summary>
        public string TileCssClass(int titleLength)
        {
            if (titleLength < (int)TileLengthLimit.Square) return "tile-square";
            if (titleLength < (int)TileLengthLimit.Wide) return "tile-wide";
            if (titleLength < (int)TileLengthLimit.Large) return "tile-large";
            if (titleLength < (int)TileLengthLimit.Big) return "tile-big";
            if (titleLength < (int)TileLengthLimit.Super) return "tile-super";
            return "tile-over";
        }
    }
}