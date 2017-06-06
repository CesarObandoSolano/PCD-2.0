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
    public class documentos_profeController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: documentos_profe
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    //var documentos_profe = db.documentos_profe.Where(d => d.id_profesor == usuarioSesion.id).Include(d => d.curso).Include(d => d.usuario);
                    List<documentos_profe> documentos = new List<documentos_profe>();
                    if(nombre != null){
                        documentos = db.documentos_profe.Where(d => d.id_profesor == usuarioSesion.id && d.titulo.Contains(nombre)).Include(d => d.curso).Include(d => d.usuario).ToList();
                    }else{
                        documentos = db.documentos_profe.Where(d => d.id_profesor == usuarioSesion.id).Include(d => d.curso).Include(d => d.usuario).ToList();
                    }
                    return View(documentos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos_profe");
        }

        // GET: documentos_profe/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    documentos_profe documentos_profe = db.documentos_profe.Find(id);
                    if (documentos_profe == null && usuarioSesion.documentos_profe.Where(d => d.id_profesor == id).Count() > 0)
                    {
                        return HttpNotFound();
                    }
                    return View(documentos_profe);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos_profe");
        }

        // GET: documentos_profe/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    ViewBag.id_curso = new SelectList(usuarioSesion.cursos, "id", "curso1");
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos_profe");
        }

        // POST: documentos_profe/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_curso,url,titulo,visibilidad")] documentos_profe documentos_profe, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                documentos_profe.id_profesor = usuarioSesion.id;

                if (file.ContentLength > 0)
                {
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Documentos", "DocumentosProfesor", ""+usuarioSesion.id);
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    documentos_profe.url = ruta_final;
                    if(!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    file.SaveAs(ruta_final);
                }                
                db.documentos_profe.Add(documentos_profe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            usuario usuario = (usuario)HttpContext.Session["usuario"];
            ViewBag.id_curso = new SelectList(usuario.cursos, "id", "curso1", documentos_profe.id_curso);
            return View(documentos_profe);
        }

        // GET: documentos_profe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    documentos_profe documentos_profe = db.documentos_profe.Find(id);
                    if (documentos_profe == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.id_curso = new SelectList(usuarioSesion.cursos, "id", "curso1", documentos_profe.id_curso);
                    return View(documentos_profe);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos_profe");
        }

        // POST: documentos_profe/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_curso,id_profesor,url,titulo,visibilidad")] documentos_profe documentos_profe, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentos_profe).State = EntityState.Modified;
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (file != null)
                {
                    try
                    {
                        System.IO.File.Delete(documentos_profe.url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Documentos", "DocumentosProfesor", ""+usuarioSesion.id);
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    documentos_profe.url = ruta_final;
                    file.SaveAs(ruta_final);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            usuario usuario = (usuario)HttpContext.Session["usuario"];
            ViewBag.id_curso = new SelectList(usuario.cursos, "id", "curso1", documentos_profe.id_curso);
            return View(documentos_profe);
        }

        // GET: documentos_profe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    documentos_profe documentos_profe = db.documentos_profe.Find(id);
                    if (documentos_profe == null)
                    {
                        return HttpNotFound();
                    }
                    return View(documentos_profe);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos_profe");
        }

        // POST: documentos_profe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            documentos_profe documentos_profe = db.documentos_profe.Find(id);
            db.documentos_profe.Remove(documentos_profe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerDocumento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            documentos_profe documentoprofe = db.documentos_profe.Find(id);
            if (documentoprofe == null)
            {
                return HttpNotFound();
            }
            string nombreArchivo = Path.GetFileName(documentoprofe.url);
            string ruta;
            if (!Path.GetExtension(documentoprofe.url).Equals("mp4"))
            {
                ruta = "~/ViewerJS/#../Recursos/Documentos/DocumentosProfesor/"+ documentoprofe.id_profesor + "/" + nombreArchivo;
            }
            else
            {
                ruta = "~/Recursos/Documentos/DocumentosProfesor/" + documentoprofe.id_profesor + "/" + nombreArchivo;
            }
            documentoprofe.url = ruta;
            return View(documentoprofe);
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
