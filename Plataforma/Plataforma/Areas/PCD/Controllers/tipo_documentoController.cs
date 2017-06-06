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
    [Authorize]
    public class tipo_documentoController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: tipo_documento
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<tipo_documento> tipos = new List<tipo_documento>();
                    if (nombre != null)
                    {
                        tipos = db.tipo_documento.Where(t => t.tipo_documento1.Contains(nombre)).ToList();
                    }
                    else
                    {
                        tipos = db.tipo_documento.ToList();
                    }
                    return View(tipos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_documento");
        }

        // GET: tipo_documento/Details/5
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
                    tipo_documento tipo_documento = db.tipo_documento.Find(id);
                    if (tipo_documento == null)
                    {
                        return HttpNotFound();
                    }
                    string nombreArchivo = Path.GetFileName(tipo_documento.icono);
                    string ruta = "~/Recursos/Iconos/" + nombreArchivo;
                    tipo_documento.icono = ruta;
                    return View(tipo_documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_documento");
        }

        // GET: tipo_documento/Create
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
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_documento");
        }

        // POST: tipo_documento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo_documento1,icono")] tipo_documento tipo_documento, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file !=null && file.ContentLength > 0)
                {
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Iconos");
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    tipo_documento.icono = ruta_final;
                    file.SaveAs(ruta_final);
                }
                db.tipo_documento.Add(tipo_documento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(tipo_documento);
        }

        // GET: tipo_documento/Edit/5
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
                    tipo_documento tipo_documento = db.tipo_documento.Find(id);
                    if (tipo_documento == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tipo_documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_documento");
        }

        // POST: tipo_documento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo_documento1,icono")] tipo_documento tipo_documento, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_documento).State = EntityState.Modified;

                if (file != null)
                {
                    try
                    {
                        System.IO.File.Delete(tipo_documento.icono);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Iconos");
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    tipo_documento.icono = ruta_final;
                    file.SaveAs(ruta_final);

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_documento);
        }

        // GET: tipo_documento/Delete/5
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
                    tipo_documento tipo_documento = db.tipo_documento.Find(id);
                    if (tipo_documento == null)
                    {
                        return HttpNotFound();
                    }
                    string nombreArchivo = Path.GetFileName(tipo_documento.icono);
                    string ruta = "~/Recursos/Iconos/" + nombreArchivo;
                    tipo_documento.icono = ruta;
                    return View(tipo_documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=tipo_documento");
        }

        // POST: tipo_documento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_documento tipo_documento = db.tipo_documento.Find(id);
            try
            {
                db.tipo_documento.Remove(tipo_documento);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se puede borrar este tipo de documento";
                string nombreArchivo = Path.GetFileName(tipo_documento.icono);
                string ruta = "~/Recursos/Iconos/" + nombreArchivo;
                tipo_documento.icono = ruta;
                return View(tipo_documento);
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
