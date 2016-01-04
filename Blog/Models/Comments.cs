using System;
using System.ComponentModel.DataAnnotations;
using Blog.DAL;

namespace Blog.Models
{
    public class Comments
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        [Display(Name = "Дата и время")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(720, MinimumLength = 3)]
        [BeginCapitalLetter]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Пост")]
        public virtual Posts Post { get; set; }

        [Display(Name = "Автор")]
        public virtual ApplicationUser User { get; set; }
    }
}