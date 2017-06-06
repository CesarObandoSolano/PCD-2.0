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
    public class permisosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: permisos
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<permiso> permisos = new List<permiso>();
                    if (nombre != null)
                    {
                        permisos = db.permisos.Where(p => p.descripcion.Contains(nombre)).ToList();
                    }
                    else
                    {
                        permisos = db.permisos.ToList();
                    }
                    return View(permisos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=permisos");
        }

        // GET: permisos/Details/5
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
                    permiso permiso = db.permisos.Find(id);
                    if (permiso == null)
                    {
                        return HttpNotFound();
                    }
                    return View(permiso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=permisos");
        }

        // GET: permisos/Create
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
            return RedirectToAction("../Account/Login/ReturnUrl=permisos");

        }

        // POST: permisos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] permiso permiso)
        {
            if (ModelState.IsValid)
            {
                db.permisos.Add(permiso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permiso);
        }

        // GET: permisos/Edit/5
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
                    permiso permiso = db.permisos.Find(id);
                    if (permiso == null)
                    {
                        return HttpNotFound();
                    }
                    return View(permiso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=permisos");
        }

        // POST: permisos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] permiso permiso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permiso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permiso);
        }


        // GET: permisos/Delete/5
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
                    permiso permiso = db.permisos.Find(id);
                    if (permiso == null)
                    {
                        return HttpNotFound();
                    }
                    return View(permiso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=permisos");
        }

        // POST: permisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            permiso permiso = db.permisos.Find(id);
            db.permisos.Remove(permiso);
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
