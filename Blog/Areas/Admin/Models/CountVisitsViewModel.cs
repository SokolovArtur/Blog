namespace Blog.Areas.Admin.Models
{
    public class CountVisitsViewModel
    {
        public string Url { get; set; }

        public int CountVisits { get; set; }

        public int CountUniqueVisits { get; set; }
    }
}