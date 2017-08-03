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
    public class ventasController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/ventas
        [Authorize]
        public ActionResult Index()
        {
            var ventas = db.ventas.Include(v => v.tipos_pago).Include(v => v.transporte).Include(v => v.usuario).Include(v => v.estado_venta);
            return View(ventas.ToList());
        }

        // GET: PCD/ventas/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: PCD/ventas/Create
        [Authorize]
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
            }
            ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre");
            ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre");
            ViewBag.id_estado_venta = new SelectList(db.estado_venta, "id", "nombre");
            ViewBag.id_articulo = db.articulos.ToList();

            return View();
        }

        // POST: PCD/ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre_comprador,nombre_receptor,numero_comprador,numero_receptor,direccion,horario_preferencia,tiempo_estimado,id_transporte,id_tipo_pago,id_usuario, id_estado_venta")] venta venta, List<int> articulos)
        {

            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];


                if (ModelState.IsValid)
                {
                    //Agregar los articulos a la venta
                    if (articulos != null && articulos.Count != 0)
                    {
                        foreach (int articuloTemp in articulos)
                        {
                            if (articuloTemp > 0)
                            {
                                articulo articuloObj = db.articulos.Find(articuloTemp);
                                ventas_articulos articuloVenta = new ventas_articulos();
                                articuloVenta.articulo = articuloObj;
                                articuloVenta.venta = venta;
                                venta.ventas_articulos.Add(articuloVenta);
                            }
                        }
                    }
                    db.ventas.Add(venta);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre", venta.id_tipo_pago);
                ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre", venta.id_transporte);
                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
                ViewBag.id_estado_venta = new SelectList(db.estado_venta, "id", "nombre", venta.id_estado_venta);
                ViewBag.id_articulo = db.articulos.ToList();
            }
            return View(venta);

        }

        // GET: PCD/ventas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_estado_venta = new SelectList(db.estado_venta, "id", "nombre", venta.id_estado_venta);
            return View(venta);
        }

        // POST: PCD/ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, int? id_estado_venta)
        {
            if (id_estado_venta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            venta.id_estado_venta = id_estado_venta.GetValueOrDefault();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: PCD/ventas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: PCD/ventas/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            venta venta = db.ventas.Find(id);
            db.ventas.Remove(venta);
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
        [Authorize]
        public ActionResult CompraCliente(venta venta)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];

                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
                List<carrito> carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id).ToList();
                List<articulo> articulosCliente = new List<articulo>();
                if (ModelState.IsValid)
                {
                    //Agregar los articulos a la venta
                    if (carrito != null && carrito.Count != 0)
                    {
                        foreach (var carritoTemp in carrito)
                        {
                            if (carritoTemp.id_articulo > 0)
                            {
                                articulo articuloObj = db.articulos.Find(carritoTemp.id_articulo);
                                articulosCliente.Add(articuloObj);
                            }
                        }
                    }
                }
                ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre", venta.id_tipo_pago);
                ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre", venta.id_transporte);
                ViewBag.id_articulo = articulosCliente;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompraCliente([Bind(Include = "id,nombre_comprador,nombre_receptor,numero_comprador,numero_receptor,direccion,horario_preferencia,tiempo_estimado,id_transporte,id_tipo_pago,id_usuario, id_estado_venta")] venta venta, List<int> articulosCliente)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];


                if (ModelState.IsValid)
                {
                    //Agregar los articulos a la venta
                    if (articulosCliente != null && articulosCliente.Count != 0)
                    {
                        
                        foreach (int articuloTemp in articulosCliente)
                        {
                            if (articuloTemp > 0)
                            {
                                articulo articuloObj = db.articulos.Find(articuloTemp);//El articulo que viene en la lista
                                ventas_articulos articuloVenta = new ventas_articulos();//Se crea el objeto articuloVenta para guardar el id del articulo, el id del usuario y la cantidad de dicho articulo
                                carrito carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id && c.id_articulo == articuloObj.id).First();//Se busca el articulo y el usuario al que pertenece el articulo para extraer la cantidad
                                articuloVenta.articulo = articuloObj;//Se agrega el articulo a la venta
                                articuloVenta.venta = venta;//Se agrega la venta a la venta articulos
                                articuloVenta.cantidad = carrito.cantidad;//Se toma la cantidad del articulo del carrito de compras
                                venta.ventas_articulos.Add(articuloVenta);
                            }
                        }
                    }
                    venta.id_estado_venta = 1;
                    string s = "2017-01-01";
                    DateTime dt = DateTime.Parse(s);
                    venta.tiempo_estimado = dt;

                    db.ventas.Add(venta);
                    db.SaveChanges();
                    //return RedirectToAction("Index");                  
                }
                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
                ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre", venta.id_tipo_pago);
                ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre", venta.id_transporte);
                //ViewBag.id_articulo = articulosCliente;
            }
            return View();
        }

    }
}
