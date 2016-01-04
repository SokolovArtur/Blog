using System.Web.Mvc;

namespace Blog.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Blog_default",
                "Default/{controller}/{action}/{id}",
                new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}