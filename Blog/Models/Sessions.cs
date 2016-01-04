using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Sessions
    {
        [StringLength(24, MinimumLength = 24)]
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<Visits> Visits { get; set; }

        public Sessions()
        {
            Visits = new List<Visits>();
        }
    }
}