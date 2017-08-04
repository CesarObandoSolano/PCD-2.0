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
    public class estadoVentaController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/estadoVenta
        public ActionResult Index()
        {
            return View(db.estado_venta.ToList());
        }

        // GET: PCD/estadoVenta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_venta estado_venta = db.estado_venta.Find(id);
            if (estado_venta == null)
            {
                return HttpNotFound();
            }
            return View(estado_venta);
        }

        // GET: PCD/estadoVenta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PCD/estadoVenta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] estado_venta estado_venta)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        db.estado_venta.Add(estado_venta);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(estado_venta);
        }

        // GET: PCD/estadoVenta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_venta estado_venta = db.estado_venta.Find(id);
            if (estado_venta == null)
            {
                return HttpNotFound();
            }
            return View(estado_venta);
        }

        // POST: PCD/estadoVenta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] estado_venta estado_venta)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(estado_venta).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(estado_venta);
        }

        // GET: PCD/estadoVenta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estado_venta estado_venta = db.estado_venta.Find(id);
            if (estado_venta == null)
            {
                return HttpNotFound();
            }
            return View(estado_venta);
        }

        // POST: PCD/estadoVenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    estado_venta estado_venta = db.estado_venta.Find(id);
                    db.estado_venta.Remove(estado_venta);
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
