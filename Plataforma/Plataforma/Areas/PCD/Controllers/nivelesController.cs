using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;
using Plataforma.App_Start;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class nivelesController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: niveles
        public ActionResult Index( string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<nivele> niveles = new List<nivele>();
                    if (nombre != null)
                    {
                        niveles = db.niveles.Where(n => n.nivel.Contains(nombre)).ToList();
                    }
                    else
                    {
                        niveles = db.niveles.ToList();
                    }
                    return View(niveles);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=niveles");
        }

        // GET: niveles/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    nivele nivele = db.niveles.Find(id);
                    if (nivele == null)
                    {
                        return HttpNotFound();
                    }
                    return View(nivele);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=niveles");
        }

        // GET: niveles/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=niveles");
        }

        // POST: niveles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nivel")] nivele nivele)
        {
            if (ModelState.IsValid)
            {
                db.niveles.Add(nivele);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nivele);
        }

        // GET: niveles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    nivele nivele = db.niveles.Find(id);
                    if (nivele == null)
                    {
                        return HttpNotFound();
                    }
                    return View(nivele);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=niveles");
        }

        // POST: niveles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nivel")] nivele nivele)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivele).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivele);
        }

        // GET: niveles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    nivele nivele = db.niveles.Find(id);
                    if (nivele == null)
                    {
                        return HttpNotFound();
                    }
                    return View(nivele);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=niveles");
        }

        // POST: niveles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            nivele nivele = db.niveles.Find(id);
            db.niveles.Remove(nivele);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
