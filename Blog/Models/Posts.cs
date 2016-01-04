using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Blog.DAL;

namespace Blog.Models
{
    public class Posts
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(720, MinimumLength = 3)]
        [Remote("CheckExistTitle", "Posts", "Admin", ErrorMessage = "Пост с таким заголовком уже существует.")]
        [BeginCapitalLetter]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [StringLength(4000)]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Display(Name = "Автор")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Комментарии")]
        public virtual ICollection<Comments> Comments { get; set; }

        [Display(Name = "Изображения")]
        public virtual ICollection<Images> Images { get; set; }

        [Display(Name = "Теги")]
        public virtual ICollection<Tags> Tags { get; set; }

        public Posts()
        {
            Comments = new List<Comments>();
            Images = new List<Images>();
            Tags = new List<Tags>();
        }
    }
}