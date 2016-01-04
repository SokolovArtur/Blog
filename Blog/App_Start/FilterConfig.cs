using System;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Models;

namespace Blog
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new VisitsFilter());
        }
    }

    public class VisitsFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string url = filterContext.RequestContext.HttpContext.Request.RawUrl.ToString();
                string sessionId = filterContext.HttpContext.Session.SessionID.ToString();

                Visits visit = new Visits
                {
                    Date = DateTime.Now,
                    Url = url,
                    Sessions = db.Sessions.Find(sessionId)
                };

                db.Visits.Add(visit);
                db.SaveChanges();
            }
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // throw new NotImplementedException();
        }
    }
}
