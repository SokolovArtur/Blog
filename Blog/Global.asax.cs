using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Blog.DAL;
using Blog.Models;

namespace Blog
{
    public class MvcApplication : HttpApplication
    {
        public static string BaseUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string sessionId = HttpContext.Current.Session.SessionID.ToString();
                if (db.Sessions.Find(sessionId) == null) {
                    Sessions session = new Sessions {
                        Id = sessionId,
                        Date = DateTime.Now
                    };

                    db.Sessions.Add(session);
                    db.SaveChanges();
                }
            }
        }
    }
}
