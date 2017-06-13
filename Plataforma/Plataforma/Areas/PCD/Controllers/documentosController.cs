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
    public class documentosController : Controller
    {
        private pimasEntities db = new pimasEntities();
        // GET: documentos
        public ActionResult Index(string nombre, int? curso, int? unidad, int? nivel, int? tipo, string descripcion)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<documento> documentos = new List<documento>();
                    if (nombre != null)
                    {
                        documentos = db.documentos.Where(d => d.titulo.Contains(nombre) || d.descripcion_corta.Contains(nombre) || d.descripcion_detallada.Contains(nombre)).
                            Include(d => d.tipo_documento).ToList();
                    }
                    else
                    {
                        documentos = db.documentos.Include(d => d.tipo_documento).ToList();
                    }

                    if (unidad != null)
                    {
                        documentos = documentos.Where(u => u.unidad == unidad).ToList();
                    }

                    if (curso != null)
                    {
                        documentos = documentos.Where(u => u.documentos_curso.Where(dc=>dc.id_curso == curso).ToList().Count>0).ToList();
                    }

                    if (nivel != null)
                    {
                        documentos = documentos.Where(u => u.niveles.Contains(db.niveles.Find(nivel))).ToList();
                    }

                    if (tipo != null)
                    {
                        documentos = documentos.Where(u => u.tipo_documento.tipo_documento1.Equals(db.tipo_documento.Find(tipo).tipo_documento1)).ToList();
                    }

                    if (descripcion != null && descripcion != "")
                    {
                        documentos = documentos.Where(u => u.descripcion_corta.Contains(descripcion) || u.descripcion_detallada.Contains(descripcion)).ToList();
                    }
                    
                    documentos = documentos.OrderBy(d => d.titulo).ToList();
                    ViewBag.unidades = db.unidades;
                    ViewBag.cursos = db.cursos;
                    ViewBag.niveles = db.niveles;
                    ViewBag.tipos = db.tipo_documento;
                    return View(documentos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        // GET: documentos/Details/5
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
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    string nombreArchivo = Path.GetFileName(documento.url);
                    string ruta = "~/Recursos/Documentos/" + nombreArchivo;
                    documento.url = ruta;

                    return View(documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        // GET: documentos/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.id_tipo = new SelectList(db.tipo_documento, "id", "tipo_documento1");
                    ViewBag.nivel = new MultiSelectList(db.niveles, "id", "nivel");
                    ViewBag.unidad = new SelectList(db.unidades, "id", "unidad");
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        // POST: documentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_tipo,url,contador_visitas,titulo,descripcion_corta,descripcion_detallada,fecha_publicacion,status,imagen,historial_version,numero_version,unidad,capitulo,descargable")] documento documento, HttpPostedFileBase file, ICollection<int> nivel)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Documentos", "" + documento.unidad);
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    documento.url = ruta_final;
                    documento.fecha_publicacion = DateTime.Today;
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    file.SaveAs(ruta_final);

                }
                foreach (int nivelSeleccionado in nivel)
                {
                    nivele nivelobj = db.niveles.Find(nivelSeleccionado);
                    documento.niveles.Add(nivelobj);
                }
                db.documentos.Add(documento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.nivel = new MultiSelectList(db.niveles, "id", "nivel");
            ViewBag.id_tipo = new SelectList(db.tipo_documento, "id", "tipo_documento1", documento.id_tipo);
            ViewBag.unidad = new SelectList(db.unidades, "id", "unidad", documento.unidad);
            return View(documento);
        }


        // GET: documentoes/Edit/5
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
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    List<nivele> niveles = db.niveles.ToList();
                    for (int i = 0; i < niveles.Count; i++)
                    {
                        foreach (var item in documento.niveles)
                        {
                            if (niveles.ElementAt(i).id == item.id)
                            {
                                niveles.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }

                    ViewBag.id_tipo = new SelectList(db.tipo_documento, "id", "tipo_documento1", documento.id_tipo);
                    ViewBag.nivel = new MultiSelectList(niveles, "id", "nivel");
                    ViewBag.unidad = new SelectList(db.unidades, "id", "unidad", documento.unidad);
                    return View(documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        // POST: documentoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_tipo,url,contador_visitas,titulo,descripcion_corta,descripcion_detallada,fecha_publicacion,status,imagen,historial_version,numero_version,unidad,capitulo,descargable")] documento documento, HttpPostedFileBase file, int unidadAnterior, ICollection<int> nivel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documento).State = EntityState.Modified;
                if (file != null)
                {
                    try
                    {
                        System.IO.File.Delete(documento.url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Documentos", "" + documento.unidad);
                    string archivo = Path.GetFileName(file.FileName);
                    string ruta_final = Path.Combine(ruta, archivo);
                    documento.url = ruta_final;
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    file.SaveAs(ruta_final);
                } else if (unidadAnterior!=0)
                {
                    if (unidadAnterior!=documento.unidad)
                    {
                        string ruta = Path.Combine(Request.PhysicalApplicationPath, "Recursos", "Documentos", "" + documento.unidad);
                        string archivo = Path.GetFileName(documento.url);
                        string ruta_final = Path.Combine(ruta, archivo);
                        try
                        {
                            System.IO.File.Move(documento.url, ruta_final);
                            documento.url = ruta_final;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (nivel!=null && nivel.Count>0)
                {
                    foreach(var item in nivel)
                    {
                        nivele nivelObj = db.niveles.Find(item);
                        documento.niveles.Add(nivelObj);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.unidad = new SelectList(db.unidades, "id", "unidad", documento.unidad);
            ViewBag.id_tipo = new SelectList(db.tipo_documento, "id", "tipo_documento1", documento.id_tipo);
            return View(documento);
        }

        // GET: documentos/Delete/5
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
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    return View(documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        // POST: documentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            documento documento = db.documentos.Find(id);
            db.documentos.Remove(documento);
            List<documentos_curso> documentosCurso = db.documentos_curso.Where(dc => dc.id_documento == id).ToList();
            foreach (documentos_curso dc in documentosCurso)
            {
                db.documentos_curso.Remove(dc);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AgregarNiveles(int? id)
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
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.id_tipo = new SelectList(db.tipo_documento, "id", "tipo_documento1", documento.id_tipo);
                    return View(documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=documentos");
        }

        public ActionResult VerDocumento(int? id, int? idCurso, int? idGrupo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.cursos.Where(c => c.documentos_curso.Where(dc => dc.id_documento == id).ToList().Count > 0).ToList().Count > 0)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    if (documento.contador_visitas == null)
                    {
                        documento.contador_visitas = 0;
                    }
                    documento.contador_visitas += 1;
                    db.SaveChanges();
                    if (db.log_visitas_documentousuario.Find(documento.id, usuarioSesion.id) == null)
                    {
                        log_visitas_documentousuario log_visitas = new log_visitas_documentousuario();
                        log_visitas.contador = 1;
                        log_visitas.id_documento = documento.id;
                        log_visitas.id_usuario = usuarioSesion.id;
                        log_visitas.fecha = DateTime.Now;
                        db.log_visitas_documentousuario.Add(log_visitas);
                    }
                    else
                    {
                        db.log_visitas_documentousuario.Find(documento.id, usuarioSesion.id).contador += 1;
                    }
                    db.SaveChanges();
                    if (idCurso != null)
                    {
                        curso curso = db.cursos.Find(idCurso);
                        ICollection<documentos_curso> documentosOrdenados = curso.documentos_curso.OrderBy(d => d.documento.unidad).ToList();
                        curso.documentos_curso = documentosOrdenados;
                        ViewBag.Curso = curso;
                        ViewBag.DocAnterior = null;
                        ViewBag.DocSiguiente = null;
                        for (int i = 0; i < curso.documentos_curso.Count; i++)
                        {
                            if (curso.documentos_curso.ElementAt(i).id_documento == documento.id)
                            {
                                if (i != 0)
                                {
                                    ViewBag.DocAnterior = curso.documentos_curso.ElementAt(i - 1).id_documento;
                                }
                                if (i != (curso.documentos_curso.Count - 1))
                                {
                                    ViewBag.DocSiguiente = curso.documentos_curso.ElementAt(i + 1).id_documento;
                                }
                            }
                        }
                        grupos grupo = new grupos();
                        grupo.id = 1;
                        
                        if (idGrupo!=null && usuarioSesion.grupos.Where(g=>g.id == idGrupo).ToList().Count>0)
                        {
                            grupo = db.grupos.Find(idGrupo);
                            List<comentario> comentarios = documento.comentarios.Where(c => c.id_grupo == grupo.id).OrderByDescending(c => c.fecha_publicacion).ToList();
                            for (int i = 0; i < comentarios.Count; i++)
                            {
                                comentarios.ElementAt(i).respuestas.OrderByDescending(r => r.fecha_publicacion);
                            }
                            ViewBag.Grupo = grupo;
                            if(comentarios == null)
                            {
                                comentarios = new List<comentario>();
                            }
                            ViewBag.Comentarios = comentarios;
                        }
                        ViewBag.Grupo = grupo;
                    }
                    string nombreArchivo = Path.GetFileName(documento.url);
                    string ruta;
                    if (documento.tipo_documento.tipo_documento1.Equals("pdf"))
                    {
                        ruta = "~/ViewerJS/#../Recursos/Documentos/" + documento.unidad + "/" + nombreArchivo;
                    }
                    else if (documento.tipo_documento.tipo_documento1.Equals("mp4"))
                    {
                        ruta = "~/Recursos/Documentos/" + documento.unidad + "/" + nombreArchivo;
                    }
                    else
                    {
                        ruta = "~/Recursos/Documentos/" + documento.unidad + "/" + nombreArchivo;
                    }
                    documento.url = ruta;

                    return View(documento);
                }
            }
            return RedirectToAction("../");
        }

        [HttpPost]
        public ActionResult OcultarDocumento(int idCurso, int idDocumento)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    db.documentos_curso.Find(idCurso, idDocumento).visibilidad = !(db.documentos_curso.Find(idCurso, idDocumento).visibilidad);
                    db.SaveChanges();
                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult AdministrarNivelesDocumento(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if ((usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR)))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    documento documento = db.documentos.Find(id);
                    if (documento == null)
                    {
                        return HttpNotFound();
                    }
                    List<nivele> niveles = db.niveles.ToList();
                    foreach (nivele item in documento.niveles)
                    {
                        niveles.Remove(item);
                    }
                    ViewBag.nivelesDisponibles = niveles;
                    return View(documento);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdministrarNivelesDocumento(int id, List<int> niveles)
        {
            documento documento = db.documentos.Find(id);
            for (int i = 0; i < documento.niveles.Count; i++)
            {
                documento.niveles.Remove(documento.niveles.ElementAt(i));
                i--;
            }
            if (niveles != null && niveles.Count != 0)
            {
                foreach (int nivelTemp in niveles)
                {
                    if (nivelTemp > 0)
                    {
                        nivele nivelObj = db.niveles.Find(nivelTemp);
                        if (!nivelObj.documentos.Contains(documento))
                        {
                            documento.niveles.Add(nivelObj);
                        }
                    }
                }
            }
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
