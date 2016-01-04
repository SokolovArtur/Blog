using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Visits
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(400)]
        public string Url { get; set; }

        [Required]
        public virtual Sessions Sessions { get; set; }
    }
}