using System;
using System.Linq;
using Blog.DAL;

namespace Blog.Areas.Admin.Models
{
    public class CountSessionsOnTimeViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int Day { get; set; }

        public int Week { get; set; }

        public int Month { get; set; }

        public int Quarter { get; set; }

        public int Year { get; set; }

        public int Total { get; set; }

        public CountSessionsOnTimeViewModel()
        {
            DateTime endDate = DateTime.Today.AddDays(1);

            DateTime dayAgo = DateTime.Today.AddDays(-1 + 1);
            Day = db.Sessions.Where(s => s.Date > dayAgo && s.Date < endDate).Count();

            DateTime weekAgo = DateTime.Today.AddDays(-7 + 1);
            Week = db.Sessions.Where(s => s.Date > weekAgo && s.Date < endDate).Count();

            DateTime monthAgo = DateTime.Today.AddDays(-30 + 1);
            Month = db.Sessions.Where(s => s.Date > monthAgo && s.Date < endDate).Count();

            DateTime quarterAgo = DateTime.Today.AddDays(-91 + 1);
            Quarter = db.Sessions.Where(s => s.Date > quarterAgo && s.Date < endDate).Count();

            DateTime yearAgo = DateTime.Today.AddDays(-365 + 1);
            Year = db.Sessions.Where(s => s.Date > yearAgo && s.Date < endDate).Count();

            Total = db.Sessions.Count();
        }
    }
}