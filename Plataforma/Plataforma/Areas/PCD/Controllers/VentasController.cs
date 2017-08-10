using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;
using GreenPay.Core.Model;

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
        public ActionResult CompraCliente()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];

                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
                List<carrito> carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id).ToList();

                ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre");
                ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre");
                ViewBag.carrito = carrito;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompraCliente([Bind(Include = "id,nombre_comprador,nombre_receptor,numero_comprador,numero_receptor,direccion,horario_preferencia,tiempo_estimado,id_transporte,id_tipo_pago,id_usuario, id_estado_venta")] venta venta)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                List<carrito> carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id).ToList();

                if (ModelState.IsValid)
                {

                    venta.id_estado_venta = 1;
                    string s = "2017-01-01";
                    DateTime dt = DateTime.Parse(s);
                    venta.tiempo_estimado = dt;
                    venta.total = 0;


                    for (int i = carrito.Count - 1; i >= 0; i--)
                    {
                        ventas_articulos ventaTemp = new ventas_articulos();
                        ventaTemp.id_articulo = carrito[i].id_articulo;
                        ventaTemp.id_venta = venta.id;
                        ventaTemp.cantidad = carrito[i].cantidad;

                        if (usuarioSesion.categoria_precio == null || usuarioSesion.categoria_precio == 1)
                        {
                            venta.total += carrito[i].cantidad * carrito[i].articulo.precio1;
                        }
                        else if (usuarioSesion.categoria_precio == 2)
                        {
                            venta.total += carrito[i].cantidad * carrito[i].articulo.precio2;
                        }
                        else if (usuarioSesion.categoria_precio == 3)
                        {
                            venta.total += carrito[i].cantidad * carrito[i].articulo.precio3;
                        }

                        db.ventas_articulos.Add(ventaTemp);
                        db.carrito.Remove(carrito[i]);
                    }
                    db.ventas.Add(venta);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.id_usuario = usuarioSesion.id;
                ViewBag.nombre_usuario = usuarioSesion.nombre + " " + usuarioSesion.apellidos;
                ViewBag.id_tipo_pago = new SelectList(db.tipos_pago, "id", "nombre", venta.id_tipo_pago);
                ViewBag.id_transporte = new SelectList(db.transportes, "id", "nombre", venta.id_transporte);
                ViewBag.carrito = carrito;
            }
            return View();
        }

        [Authorize]
        public ActionResult PagoCliente()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                usuarioSesion = db.usuarios.Find(usuarioSesion.id);

                Pagos pagos = new Pagos();
                string bearer = pagos.Login();

                GPCreditCardList tarjetasCliente = new GPCreditCardList();
                tarjetasCliente = pagos.ListCreditCards(bearer,usuarioSesion.id_gp.Value);

                if(tarjetasCliente == null || tarjetasCliente.List.Count == 0)
                {
                    return RedirectToAction("AgregarTarjeta");
                }
                ViewBag.tarjetas = tarjetasCliente.List;
                return View();
            }
            return RedirectToAction("../Account/Login/");
        }

        [Authorize]
        [HttpPost]
        public ActionResult PagoCliente(int idTarjeta)
        {
            if (Session["usuario"] != null)
            {
                
            }
            return RedirectToAction("../Account/Login/");
        }

        [Authorize]
        public ActionResult AgregarCliente()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                usuarioSesion = db.usuarios.Find(usuarioSesion.id);
                /*if(usuarioSesion.id_gp == null)
                {
                    return RedirectToAction("PagoCliente");
                }*/
            }
            return View();
        }

        
        [Authorize]
        [HttpPost]
        public ActionResult AgregarCliente(string identificacion, int tipoIdentificacion, int pin)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                usuarioSesion = db.usuarios.Find(usuarioSesion.id);

                Pagos pagos = new Pagos();
                string bearer = pagos.Login();
                long id_gp = pagos.CreateCustomer(bearer, usuarioSesion, identificacion, tipoIdentificacion, pin);
                if(id_gp != 0)
                {
                   usuarioSesion.id_gp = id_gp;
                    db.SaveChanges();
                }
                RedirectToAction("PagoCliente");
            }
            return RedirectToAction("../Account/Login/");
        }

        [Authorize]
        public ActionResult AgregarTarjeta()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AgregarTarjeta(string nombreTarjeta, string numeroTarjeta, int tipoTarjeta, string ccv, string fechaCaducidad, bool? favorita, string nickname)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                usuarioSesion = db.usuarios.Find(usuarioSesion.id);

                Pagos pagos = new Pagos();
                string bearer = pagos.Login();

                if(favorita == null)
                {
                    favorita = false;
                }
                pagos.CreateCreditCard(bearer, usuarioSesion.id_gp.Value, nombreTarjeta, numeroTarjeta, tipoTarjeta, ccv, fechaCaducidad, favorita.Value, nickname);
                RedirectToAction("PagoCliente");
            }
            return RedirectToAction("../Account/Login/");
        }
    }
}
