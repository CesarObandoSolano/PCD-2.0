using System.Web.Mvc;

namespace Plataforma.Areas.PCD
{
    public class PCDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PCD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PCD_default",
                "PCD/{controller}/{action}/{id}",
                new { controller = "HomePCD", action = "Index", id = UrlParameter.Optional }
            );


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }


    }
}