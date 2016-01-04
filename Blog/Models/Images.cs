using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Images
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }

        public Images()
        {
            Posts = new List<Posts>();
        }
    }
}