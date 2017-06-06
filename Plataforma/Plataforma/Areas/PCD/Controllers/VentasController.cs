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

    public class VentasController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/Ventas
        public ActionResult Index()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    var ventas = db.Ventas.Include(v => v.Articulo);
                    return View(ventas.ToList());
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=articulos");
        }

        // GET: PCD/Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    Venta venta = db.Ventas.Find(id);
                    if (venta == null)
                    {
                        return HttpNotFound();
                    }
                    return View(venta);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=articulos");
        }

        [AllowAnonymous]
        // GET: PCD/Ventas/Create
        public ActionResult Create()
        {
            ViewBag.id_articulo = new SelectList(db.Articulos, "id", "nombre");
            return View();
        }

        // POST: PCD/Ventas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_articulo,nombre_comprador,nombre_receptor,numero_comprador,numero_receptor,direccion,horario,cantidad,tipo_entrega,forma_pago")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Ventas.Add(venta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_articulo = new SelectList(db.Articulos, "id", "nombre", venta.id_articulo);
            return View(venta);
        }

        // GET: PCD/Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    Venta venta = db.Ventas.Find(id);
                    if (venta == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.id_articulo = new SelectList(db.Articulos, "id", "nombre", venta.id_articulo);
                    return View(venta);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=articulos");
        }

        // POST: PCD/Ventas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_articulo,nombre_comprador,nombre_receptor,numero_comprador,numero_receptor,direccion,horario,cantidad,tipo_entrega,forma_pago")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_articulo = new SelectList(db.Articulos, "id", "nombre", venta.id_articulo);
            return View(venta);
        }

        // GET: PCD/Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    Venta venta = db.Ventas.Find(id);
                    if (venta == null)
                    {
                        return HttpNotFound();
                    }
                    return View(venta);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=articulos");
        }

        // POST: PCD/Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venta venta = db.Ventas.Find(id);
            db.Ventas.Remove(venta);
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
