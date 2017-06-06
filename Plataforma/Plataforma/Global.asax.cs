using Plataforma.Areas.PCD.Controllers;
using Plataforma.Areas.PCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Win32;
using Umbraco.Web;

namespace Plataforma
{
    
    public class MvcApplication : UmbracoApplication
    {

        //public override void Init()
        //{
        //    ProgramadorTarea.Start();
        //}

        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            ProgramadorTarea.Start();
        }

        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //    AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        //    ProgramadorTarea.Start();
        //}


        //public void Session_Start(object sender, EventArgs e)
        //{
        //    if (Session["usuario"] != null)
        //    {
        //        usuario usuario = (usuario)Session["usuario"];
        //        Session["id"] = usuario.id;
        //    }
        //}

        public void Session_End(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                usuario usuario = (usuario)Session["usuario"];
                new AccountController().CerrarSesion(usuario.id);
            }

        }
    }
}
