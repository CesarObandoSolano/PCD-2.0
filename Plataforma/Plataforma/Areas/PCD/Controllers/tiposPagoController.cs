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
    public class tiposPagoController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/tiposPago
        public ActionResult Index()
        {
            return View(db.tipos_pago.ToList());
        }

        // GET: PCD/tiposPago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipos_pago tipos_pago = db.tipos_pago.Find(id);
            if (tipos_pago == null)
            {
                return HttpNotFound();
            }
            return View(tipos_pago);
        }

        // GET: PCD/tiposPago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PCD/tiposPago/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] tipos_pago tipos_pago)
        {
            if (ModelState.IsValid)
            {
                db.tipos_pago.Add(tipos_pago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipos_pago);
        }

        // GET: PCD/tiposPago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipos_pago tipos_pago = db.tipos_pago.Find(id);
            if (tipos_pago == null)
            {
                return HttpNotFound();
            }
            return View(tipos_pago);
        }

        // POST: PCD/tiposPago/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] tipos_pago tipos_pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipos_pago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipos_pago);
        }

        // GET: PCD/tiposPago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipos_pago tipos_pago = db.tipos_pago.Find(id);
            if (tipos_pago == null)
            {
                return HttpNotFound();
            }
            return View(tipos_pago);
        }

        // POST: PCD/tiposPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipos_pago tipos_pago = db.tipos_pago.Find(id);
            db.tipos_pago.Remove(tipos_pago);
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
