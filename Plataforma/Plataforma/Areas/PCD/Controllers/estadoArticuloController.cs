using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;

namespace Plataforma.Areas.PCD.Controllers
{
    public class estadoArticuloController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/estadoArticulo
        public ActionResult Index()
        {
            return View(db.estado_articulo.ToList());
        }

        // GET: PCD/estadoArticulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_articulo estado_articulo = db.estado_articulo.Find(id);
            if (estado_articulo == null)
            {
                return HttpNotFound();
            }
            return View(estado_articulo);
        }

        // GET: PCD/estadoArticulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PCD/estadoArticulo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] estado_articulo estado_articulo)
        {
            if (ModelState.IsValid)
            {
                db.estado_articulo.Add(estado_articulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estado_articulo);
        }

        // GET: PCD/estadoArticulo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_articulo estado_articulo = db.estado_articulo.Find(id);
            if (estado_articulo == null)
            {
                return HttpNotFound();
            }
            return View(estado_articulo);
        }

        // POST: PCD/estadoArticulo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] estado_articulo estado_articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado_articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estado_articulo);
        }

        // GET: PCD/estadoArticulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_articulo estado_articulo = db.estado_articulo.Find(id);
            if (estado_articulo == null)
            {
                return HttpNotFound();
            }
            return View(estado_articulo);
        }

        // POST: PCD/estadoArticulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            estado_articulo estado_articulo = db.estado_articulo.Find(id);
            db.estado_articulo.Remove(estado_articulo);
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
