using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Umbraco.Core;


namespace Plataforma.App_Start
{
    public class MvcStartup : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}