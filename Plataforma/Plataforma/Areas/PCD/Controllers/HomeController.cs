using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Areas.PCD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Acerca de..";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Información de Contacto.";

            return View();
        }
    }
}