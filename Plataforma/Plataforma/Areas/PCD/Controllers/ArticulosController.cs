using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;
using System.IO;
using Plataforma.App_Start;

namespace Plataforma.Areas.PCD.Controllers
{
    public class articulosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: PCD/articulos
        public ActionResult Index()
        {
            return View(db.articulos.ToList());
        }

        public ActionResult Articulos()
        {
            List<articulo> articulosTemp = db.articulos.ToList();

            foreach (articulo articuloTemp in articulosTemp)
            {
                string nombreArchivo = Path.GetFileName(articuloTemp.url_imagen);
                string ruta = "~/Recursos/Articulos/" + articuloTemp.id_categoria + "/" + nombreArchivo;
                articuloTemp.url_imagen = ruta;
            }
                return View(articulosTemp);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AgregarAlCarrito(int idArticulo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];

                carrito carrito = new carrito();
                carrito.id_usuario = usuarioSesion.id;
                carrito.id_articulo = idArticulo;
                carrito.cantidad = 1;

                db.carrito.Add(carrito);
                db.SaveChanges();

                return Json("exito", JsonRequestBehavior.AllowGet);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        /*public ActionResult CarritoCompras()
        {
            usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
            List<carrito> carrito = new List<carrito>();
            carrito = db.carrito.Where(c => c.id_usuario == usuarioSesion.id).ToList();
            return View();
        }*/



        // GET: PCD/articulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articulo articulo = db.articulos.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // GET: PCD/articulos/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.categoria_articulo, "id", "nombre");
            ViewBag.id_estado_articulo = new SelectList(db.estado_articulo, "id", "nombre");
            return View();
        }

        // POST: PCD/articulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,detalle,precio1,precio2,precio3,url_imagen,id_categoria, id_estado_articulo")] articulo articulo, 
            HttpPostedFileBase file)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        if (file.ContentLength > 0)
                        {
                            string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Articulos", "" + articulo.id_categoria);
                            string archivo = Path.GetFileName(file.FileName);
                            string ruta_final = Path.Combine(ruta, archivo);
                            articulo.url_imagen = ruta_final;
                            if (!Directory.Exists(ruta))
                            {
                                Directory.CreateDirectory(ruta);
                            }
                            file.SaveAs(ruta_final);
                        }
                        db.articulos.Add(articulo);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            //ViewBag.id_categoria = new SelectList(db.categoria_articulo, "id", "nombre", articulo.id_categoria);
            return View(articulo);
        }

        // GET: PCD/articulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articulo articulo = db.articulos.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.categoria_articulo, "id", "nombre", articulo.id_categoria);
            ViewBag.id_estado_articulo = new SelectList(db.estado_articulo, "id", "nombre");
            return View(articulo);
        }

        // POST: PCD/articulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,detalle,precio1,precio2,precio3, url_imagen, id_categoria, id_estado_articulo")] articulo articulo, HttpPostedFileBase file, int categoriaAnterior)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(articulo).State = EntityState.Modified;
                        if (file != null)
                        {
                            try
                            {
                                System.IO.File.Delete(articulo.url_imagen);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Articulos", "" + articulo.id_categoria);
                            string archivo = Path.GetFileName(file.FileName);
                            string ruta_final = Path.Combine(ruta, archivo);
                            articulo.url_imagen = ruta_final;
                            if (!Directory.Exists(ruta))
                            {
                                Directory.CreateDirectory(ruta);
                            }
                            file.SaveAs(ruta_final);
                        }
                        else if (categoriaAnterior != 0)
                        {
                            if (categoriaAnterior != articulo.id_categoria)
                            {
                                string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Articulos", "" + articulo.id_categoria);
                                string archivo = Path.GetFileName(articulo.url_imagen);
                                string ruta_final = Path.Combine(ruta, archivo);
                                try
                                {
                                    System.IO.File.Move(articulo.url_imagen, ruta_final);
                                    articulo.url_imagen = ruta_final;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.id_categoria = new SelectList(db.categoria_articulo, "id", "nombre", articulo.id_categoria);
            ViewBag.id_estado_articulo = new SelectList(db.estado_articulo, "id", "nombre");
            return View(articulo);
        }

        // GET: PCD/articulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articulo articulo = db.articulos.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // POST: PCD/articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    articulo articulo = db.articulos.Find(id);
                    db.articulos.Remove(articulo);
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

