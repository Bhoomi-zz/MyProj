using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ncqrs;
using System.Reflection;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}.aspx/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Root",
               "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = "" }
              );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
              
            RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure();
           // ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            //log.Info("here we go!");
            //log.Debug("debug afasf");
        }
    }
}