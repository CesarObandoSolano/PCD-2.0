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
    public class carritoController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/carrito
        [Authorize]
        public ActionResult Index()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                List<carrito> carrito = new List<carrito>();
                carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id).ToList();
                return View(carrito);
            }
            return View();
        }

        [Authorize]
        public ActionResult EliminarDelCarrito(int idArticulo)
        {
            if (ModelState.IsValid)
            {
                if (Session["usuario"] != null)
                {
                    usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];

                    carrito carrito = db.carrito.Where(c => c.id_articulo == idArticulo && c.id_usuario == usuarioSesion.id).First();

                    db.carrito.Remove(carrito);
                    db.SaveChanges();
                }
            }

            return Json("exito", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ModificarCantidadArticulos(int idArticulo, int cantidad)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];

                carrito carrito = db.carrito.Where(c => c.id_articulo == idArticulo && c.id_usuario == usuarioSesion.id).First();
                carrito.cantidad = cantidad;
                db.SaveChanges();
                return Json("exito", JsonRequestBehavior.AllowGet);
            }
            return Json("fallo", JsonRequestBehavior.AllowGet);
        }

        // GET: PCD/carrito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carrito carrito = db.carrito.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // GET: PCD/carrito/Create
        public ActionResult Create()
        {
            ViewBag.id_articulo = new SelectList(db.articulos, "id", "nombre");
            ViewBag.id_usuario = new SelectList(db.usuarios, "id", "nombre");
            return View();
        }

        // POST: PCD/carrito/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,id_articulo,cantidad")] carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.carrito.Add(carrito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_articulo = new SelectList(db.articulos, "id", "nombre", carrito.id_articulo);
            ViewBag.id_usuario = new SelectList(db.usuarios, "id", "nombre", carrito.id_usuario);
            return View(carrito);
        }

        // GET: PCD/carrito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carrito carrito = db.carrito.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_articulo = new SelectList(db.articulos, "id", "nombre", carrito.id_articulo);
            ViewBag.id_usuario = new SelectList(db.usuarios, "id", "nombre", carrito.id_usuario);
            return View(carrito);
        }

        // POST: PCD/carrito/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,id_articulo,cantidad")] carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_articulo = new SelectList(db.articulos, "id", "nombre", carrito.id_articulo);
            ViewBag.id_usuario = new SelectList(db.usuarios, "id", "nombre", carrito.id_usuario);
            return View(carrito);
        }



        // GET: PCD/carrito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carrito carrito = db.carrito.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: PCD/carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            carrito carrito = db.carrito.Find(id);
            db.carrito.Remove(carrito);
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
