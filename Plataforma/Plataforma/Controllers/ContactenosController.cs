using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class ContactenosController : Controller
    {
        // GET: Contactenos
        public ActionResult EnviarEmail()
        {
            return View();
        }
        
    }
}