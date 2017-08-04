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
    public class categoriaArticuloController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/categoriaArticulo
        public ActionResult Index()
        {
            return View(db.categoria_articulo.ToList());
        }

        // GET: PCD/categoriaArticulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoria_articulo categoria_articulo = db.categoria_articulo.Find(id);
            if (categoria_articulo == null)
            {
                return HttpNotFound();
            }
            return View(categoria_articulo);
        }

        // GET: PCD/categoriaArticulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PCD/categoriaArticulo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] categoria_articulo categoria_articulo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        db.categoria_articulo.Add(categoria_articulo);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(categoria_articulo);
        }

        // GET: PCD/categoriaArticulo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoria_articulo categoria_articulo = db.categoria_articulo.Find(id);
            if (categoria_articulo == null)
            {
                return HttpNotFound();
            }
            return View(categoria_articulo);
        }

        // POST: PCD/categoriaArticulo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] categoria_articulo categoria_articulo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(categoria_articulo).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(categoria_articulo);
        }

        // GET: PCD/categoriaArticulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoria_articulo categoria_articulo = db.categoria_articulo.Find(id);
            if (categoria_articulo == null)
            {
                return HttpNotFound();
            }
            return View(categoria_articulo);
        }

        // POST: PCD/categoriaArticulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    categoria_articulo categoria_articulo = db.categoria_articulo.Find(id);
                    db.categoria_articulo.Remove(categoria_articulo);
                    db.SaveChanges();
                }
            }
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
