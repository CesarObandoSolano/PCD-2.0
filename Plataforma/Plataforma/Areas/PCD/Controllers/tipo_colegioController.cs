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
    public class tipo_colegioController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: tipo_colegio
        public ActionResult Index()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    return View(db.tipo_colegio.ToList());
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_colegio");
        }

        // GET: tipo_colegio/Details/5
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
                    tipo_colegio tipo_colegio = db.tipo_colegio.Find(id);
                    if (tipo_colegio == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tipo_colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_colegio");
        }

        // GET: tipo_colegio/Create
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
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_colegio");
        }

        // POST: tipo_colegio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo_institucion")] tipo_colegio tipo_colegio)
        {
            if (ModelState.IsValid)
            {
                db.tipo_colegio.Add(tipo_colegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_colegio);
        }

        // GET: tipo_colegio/Edit/5
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
                    tipo_colegio tipo_colegio = db.tipo_colegio.Find(id);
                    if (tipo_colegio == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tipo_colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_colegio");
        }

        // POST: tipo_colegio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo_institucion")] tipo_colegio tipo_colegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_colegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_colegio);
        }

        // GET: tipo_colegio/Delete/5
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
                    tipo_colegio tipo_colegio = db.tipo_colegio.Find(id);
                    if (tipo_colegio == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tipo_colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_colegio");
        }

        // POST: tipo_colegio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_colegio tipo_colegio = db.tipo_colegio.Find(id);
            db.tipo_colegio.Remove(tipo_colegio);
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
