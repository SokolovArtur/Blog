using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Tags
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Remote("CheckExistTag", "Tags", "Admin", ErrorMessage = "Тег с таким названием уже существует.")]
        [BeginCapitalLetter]
        [Display(Name = "Тег")]
        public string Name { get; set; }

        [Display(Name = "Посты")]
        public virtual ICollection<Posts> Posts { get; set; }

        public Tags()
        {
            Posts = new List<Posts>();
        }
    }
}