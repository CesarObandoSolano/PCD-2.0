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
    public class librosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: libros
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    //ICollection<libro> libros = db.libros.Where(l => l.usuarios.FirstOrDefault().id == usuarioSesion.id).ToList();
                    List<libro> librosFiltrados = new List<libro>();
                    if (nombre != null)
                    {
                        librosFiltrados = db.libros.Where(l => l.usuarios.FirstOrDefault().id == usuarioSesion.id && l.descripcion.Contains(nombre)).ToList();
                    }
                    else
                    {
                        librosFiltrados = db.libros.Where(l => l.usuarios.FirstOrDefault().id == usuarioSesion.id).ToList();
                    }
                    return View(librosFiltrados);
                }
                else if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<libro> librosFiltrados = new List<libro>();
                    if (nombre != null)
                    {
                        librosFiltrados = db.libros.Where(l => l.solicitado == true && l.descripcion.Contains(nombre)).ToList();
                    }
                    else
                    {
                        librosFiltrados = db.libros.Where(l => l.solicitado == true).ToList();
                    }
                    return View(librosFiltrados);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=libros");
        }

        // GET: libros/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM) ||
                    ((usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR)) && db.libros.Find(id) != null && db.libros.Find(id).solicitado == true))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    libro libro = db.libros.Find(id);
                    if (libro == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.usuario = db.usuarios.Find(libro.usuarios.FirstOrDefault().id);
                    return View(libro);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=libros");
        }

        // GET: libros/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {

                    List<documento> documentos = db.documentos.ToList();
                    //foreach (curso curso in usuarioSesion.cursos)
                    //{
                    //    foreach (documentos_curso documentoCurso in curso.documentos_curso)
                    //    {
                    //        documentos.Add(documentoCurso.documento);
                    //    }
                    //}
                    ViewBag.Documentos = documentos;
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=libros");
        }

        // POST: libros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] libro libro, ICollection<int> documentosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                libro.usuarios.Add(db.usuarios.Find(usuarioSesion.id));
                libro.colegios = libro.usuarios.First().colegios;
                foreach (int id in documentosSeleccionados)
                {
                    libro.documentos.Add(db.documentos.Find(id));
                }
                db.libros.Add(libro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(libro);
        }

        // GET: libros/Edit/5
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
                    libro libro = db.libros.Find(id);
                    if (libro == null)
                    {
                        return HttpNotFound();
                    }
                    List<documento> documentos = db.documentos.ToList();
                    foreach (documento documento in libro.documentos)
                    {
                        if (documentos.Contains(documento))
                        {
                            documentos.Remove(documento);
                        }
                    }
                    //foreach (curso curso in usuarioSesion.cursos)
                    //{
                    //    foreach (documentos_curso documentoCurso in curso.documentos_curso)
                    //    {
                    //        if (libro.documentos.Where(d => d.id == documentoCurso.id_documento).ToList().Count <= 0)
                    //        {
                    //            documentos.Add(documentoCurso.documento);
                    //        }
                    //    }
                    //}
                    ViewBag.Documentos = documentos;
                    return View(libro);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=libros");
        }

        // POST: libros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] libro libro, ICollection<int> documentosSeleccionados, ICollection<int> documentosRemover)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libro).State = EntityState.Modified;
                if (documentosRemover != null)
                {
                    foreach (int docRemover in documentosRemover)
                    {
                        documento documento = db.documentos.Find(docRemover);
                        db.documentos.Find(documento.id).libros.Remove(libro);
                    }
                }
                if (documentosSeleccionados != null)
                {
                    foreach (int docAgregar in documentosSeleccionados)
                    {
                        documento documento = db.documentos.Find(docAgregar);
                        libro.documentos.Add(documento);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(libro);
        }

        // GET: libros/Delete/5
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
                    libro libro = db.libros.Find(id);
                    if (libro == null)
                    {
                        return HttpNotFound();
                    }
                    return View(libro);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=libros");
        }

        // POST: libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            libro libro = db.libros.Find(id);
            db.libros.Remove(libro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitarLibro(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    db.libros.Find(id).solicitado = true;
                    db.SaveChanges();
                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
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
