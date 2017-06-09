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
using System.Web.Script.Serialization;
using Plataforma.App_Start;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class cursosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: cursos
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<curso> cursos = new List<curso>();
                    if (nombre != null && nombre != "")
                    {
                        cursos = db.cursos.Where(c => c.curso1.Contains(nombre)).Include(c => c.documentos_profe).OrderBy(c=> c.curso1).ToList();
                    }
                    else
                    {
                        cursos = db.cursos.Include(c => c.documentos_profe).OrderBy(c => c.curso1).ToList();
                    }
                    return View(cursos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        // GET: cursos/Details/5
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
                    curso curso = db.cursos.Find(id);
                    if (curso == null)
                    {
                        return HttpNotFound();
                    }
                    curso.usuarios = curso.usuarios.OrderBy(u => u.nombre).ToList();
                    return View(curso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        // GET: cursos/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.id = new SelectList(db.documentos_profe, "id_curso", "url");
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        // POST: cursos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,curso1,contador_visitas")] curso curso)
        {
            if (ModelState.IsValid)
            {
                db.cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("AgregarDocumentos", new { id = curso.id });
            }

            ViewBag.id = new SelectList(db.documentos_profe, "id_curso", "url", curso.id);
            return View(curso);
        }

        // GET: cursos/Edit/5
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
                    curso curso = db.cursos.Find(id);
                    if (curso == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.id = new SelectList(db.documentos_profe, "id_curso", "url", curso.id);
                    return View(curso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        // POST: cursos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,curso1,contador_visitas")] curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.documentos_profe, "id_curso", "url", curso.id);
            return View(curso);
        }

        // GET: cursos/Delete/5
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
                    curso curso = db.cursos.Find(id);
                    if (curso == null)
                    {
                        return HttpNotFound();
                    }
                    return View(curso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        // POST: cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            curso curso = db.cursos.Find(id);
            db.cursos.Remove(curso);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AgregarDocumentos(int? id)
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
                    DocumentosCurso documentosCurso = new DocumentosCurso();
                    curso curso = db.cursos.Find(id);
                    ICollection<nivele> niveles = db.niveles.ToList();
                    if (curso == null || niveles == null)
                    {
                        return HttpNotFound();
                    }
                    documentosCurso.curso = curso;
                    documentosCurso.niveles = niveles;
                    return View(documentosCurso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        public ActionResult VerDocumentos(int? id, int? idCurso, int? unidad, int? capitulo)
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
                    DocumentosCurso documentosCurso = new DocumentosCurso();
                    curso curso = db.cursos.Find(idCurso);
                    ICollection<documento> documentos = db.niveles.Find(id).documentos;
                    if (curso == null || documentos == null)
                    {
                        return HttpNotFound();
                    }
                    documentosCurso.curso = curso;
                    for (int i = 0; i < documentos.Count; i++)
                    {
                        documento documento = documentos.ElementAt(i);
                        foreach (var documentoCurso in curso.documentos_curso)
                        {
                            if (documento.id == documentoCurso.id_documento)
                            {
                                documentos.Remove(documento);
                                i--;
                            }
                        }
                    }

                    if (unidad != null)
                    {
                        documentos = documentos.Where(d => d.unidad == unidad).ToList();
                    }
                    if (capitulo != null)
                    {
                        documentos = documentos.Where(d => d.capitulo == capitulo).ToList();
                    }
                    documentosCurso.documentos = documentos;
                    documentosCurso.unidad = unidad.GetValueOrDefault();
                    documentosCurso.capitulo = capitulo.GetValueOrDefault();
                    documentosCurso.curso.contador_visitas = id;
                    ViewBag.docsSeleccionados = curso.documentos_curso.Where(dc => dc.documento.niveles.Where(n=>n.id == id).ToList().Count > 0);
                    ViewBag.unidades = db.unidades;
                    return View(documentosCurso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        [HttpPost]
        public ActionResult VerDocumentos([Bind(Include = "curso, niveles, documentos, documentosSeleccionados")] DocumentosCurso documentosCurso)
        {
            List<string> destinatarios = new List<string>();
            string asunto = "PCD - Actualización de Curso";
            string mensaje = "Le informamos que se actualizó el curso " + db.cursos.Find(documentosCurso.curso.id).curso1 +
                " en la plataforma de PIMAS. Los cambios son los siguientes:<br/>-  Se incluyeron / actualizaron los siguientes archivos: ";
            foreach(documentos_curso item in db.documentos_curso.Where(dc=>dc.id_curso == documentosCurso.curso.id))
            {
                db.documentos_curso.Remove(item);
            }
            foreach (var idDocumento in documentosCurso.documentosSeleccionados)
            {
                documentos_curso dc = new documentos_curso();
                dc.id_curso = documentosCurso.curso.id;
                dc.id_documento = idDocumento;
                dc.visibilidad = true;
                db.documentos_curso.Add(dc);
                mensaje += "<br />" + db.documentos.Find(idDocumento).titulo;
            }
            mensaje += "<br /><br /> Esperamos le sean de utilidad.";
            List<usuario> usuarios = db.cursos.Find(documentosCurso.curso.id).usuarios.ToList();
            foreach (usuario usuario in usuarios)
            {
                if (usuario.notificacione.correo == true)
                {
                    destinatarios.Add(usuario.correo);
                    if (usuario.correo_2 != null)
                    {
                        destinatarios.Add(usuario.correo_2);
                    }
                }
            }
            db.SaveChanges();
            Utilitarios.EnviarCorreo(destinatarios, asunto, mensaje);
            return RedirectToAction("AgregarDocumentos", new { id = documentosCurso.curso.id });
            //return RedirectToAction("Index");
        }

        public ActionResult VerCurso(int? id, int? idGrupo, string nombre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            curso curso = db.cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            if (Session["usuario"] != null)
            {
                ViewBag.documentosprofe = null;
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.cursos.Where(c=>c.id==curso.id).Count() > 0 ||
                    (usuarioSesion.grupos != null && usuarioSesion.grupos.Where(g => g.curso.id == curso.id).Count() > 0))
                {
                    if (curso.contador_visitas == null)
                    {
                        db.cursos.Find(id).contador_visitas = 0;
                    }
                    db.cursos.Find(id).contador_visitas += 1;
                    if(db.log_visitas_cursousuario.Find(curso.id, usuarioSesion.id) == null)
                    {
                        log_visitas_cursousuario log_visitas = new log_visitas_cursousuario();
                        log_visitas.contador = 1;
                        log_visitas.id_curso = curso.id;
                        log_visitas.id_usuario = usuarioSesion.id;
                        log_visitas.fecha = DateTime.Now;
                        db.log_visitas_cursousuario.Add(log_visitas);
                    }else
                    {
                        db.log_visitas_cursousuario.Find(curso.id, usuarioSesion.id).contador += 1;
                    }
                    db.SaveChanges();
                    if (curso.documentos_curso.Count > 0)
                    {
                        ICollection<documentos_curso> documentosOrdenados = curso.documentos_curso.OrderBy(d => d.documento.unidade.unidad).ToList();
                        curso.documentos_curso = documentosOrdenados;
                        foreach (documentos_curso documentoTemp in curso.documentos_curso)
                        {
                            string nombreArchivo = Path.GetFileName(documentoTemp.documento.tipo_documento.icono);
                            string ruta = "~/Recursos/Iconos/" + nombreArchivo;
                            documentoTemp.documento.tipo_documento.icono = ruta;
                        }
                    }
                    if (idGrupo!=null && usuarioSesion.grupos.Where(g => g.id == idGrupo).Count() > 0)
                    {
                        grupos grupo = usuarioSesion.grupos.Where(g => g.id == idGrupo).FirstOrDefault();
                        ViewBag.Grupo = grupo;
                        usuario profesor = grupo.usuarios.Where(u => u.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM)).FirstOrDefault();
                        if (profesor != null && profesor.documentos_profe.Count > 0)
                        {
                            ICollection<documentos_profe> documentosProfesor = db.usuarios.Find(profesor.id).documentos_profe.Where(dp => dp.curso.id == curso.id).ToList();
                            
                            ViewBag.documentosprofe = documentosProfesor;
                        }
                    }
                    if (nombre!=null && nombre != "")
                    {
                        curso.documentos_curso = curso.documentos_curso.Where(d => d.documento.titulo.ToLower().Contains(nombre.ToLower()) || 
                        (d.documento.descripcion_corta != null && d.documento.descripcion_corta.ToLower().Contains(nombre.ToLower())) ||
                        (d.documento.descripcion_detallada != null && d.documento.descripcion_detallada.ToLower().Contains(nombre.ToLower()))).ToList();
                    }
                    
                    return View(curso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        [HttpGet]
        public JsonResult ListaCursos()
        {
            List<curso> cursos = db.cursos.ToList();
            return Json(cursos.Select(x => new { id = x.id, curso1 = x.curso1 }), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MatricularUsuarios(int? id, string nombreusuario, int? rol, int? colegio, int? nivel)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    curso curso = db.cursos.Find(id);
                    if (curso == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    List<usuario> usuarios = new List<usuario>();
                    if (nombreusuario != null && nombreusuario != "")
                    {
                        usuarios = db.usuarios.
                            Where(u => (u.nombre.Contains(nombreusuario) || u.apellidos.Contains(nombreusuario))).ToList();
                    }else
                    {
                        usuarios = db.usuarios.ToList();
                    }
                    usuarios = usuarios.Where(u => !(u.cursos.Contains(curso))).ToList();
                    if (rol != null && rol > 0)
                    {
                        usuarios = usuarios.Where(u => u.roles.FirstOrDefault().id == rol).ToList();
                    }
                    if (colegio != null && colegio > 0)
                    {
                        colegio colegioTemp = db.colegios.Find(colegio);
                        usuarios = usuarios.Where(u => u.colegios.Contains(colegioTemp)).ToList();
                    }
                    if (nivel != null && nivel > 0)
                    {
                        nivele nivelTemp = db.niveles.Find(nivel);
                        usuarios = usuarios.Where(u => u.niveles.Contains(nivelTemp)).ToList();
                    }
                    //usuarios =
                    //    usuarios.OrderBy(u => u.roles.FirstOrDefault().rol).
                    //    ThenBy(u => u.colegios.FirstOrDefault().nombre).
                    //    ThenBy(u => u.niveles.FirstOrDefault().nivel).ToList();
                    ViewBag.usuariosAMatricular = usuarios;
                    ViewBag.roles = db.roles.ToList();
                    ViewBag.colegios = db.colegios.ToList();
                    ViewBag.niveles = db.niveles.ToList();
                    return View(curso);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        [HttpPost]
        public ActionResult MatricularUsuarios(int id, ICollection<int> usuariosMatricular)
        {
            curso curso = db.cursos.Find(id);
            if (usuariosMatricular != null)
            {
                curso.usuarios.Clear();
                foreach (int idUsuario in usuariosMatricular)
                {
                    usuario usuarioTemporal = db.usuarios.Find(idUsuario);
                    usuarioTemporal.cursos.Add(curso);
                    List<string> destinatarios = new List<string>();
                    destinatarios.Add(usuarioTemporal.correo);
                    Utilitarios.EnviarCorreo(destinatarios, "PCD: Solicitud Aceptada",
                        "Estimado " + usuarioTemporal.nombre + " " + usuarioTemporal.apellidos +
                        "<br /><br />Le informamos que ha sido matriculado en el curso “X” de nuestra Plataforma de Contenidos Digitales PIMAS," +
                        " el cual debe aparecerle ahora en la sección “Cursos” del Menú." +
                        "<br />Gracias por utilizar nuestros productos."
                        );
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult IndiceCursos()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    var grupos = usuarioSesion.grupos;
                    return View(grupos.ToList());
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
        }

        public ActionResult AdministrarCurso(int? id, string nombre, string nombreDoc, int? unidad, int? nivel, int? tipo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM))
                {
                    grupos grupo = db.grupos.Find(id);
                    if (grupo == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    if (nombre == null)
                    {
                        nombre = "";
                    }

                    grupo.usuarios = grupo.usuarios.Where(u => u.id != usuarioSesion.id 
                    && (u.nombre.Contains(nombre)|| u.apellidos.Contains(nombre)|| u.username.Contains(nombre))).ToList();

                    if (unidad != null)
                    {
                        grupo.curso.documentos_curso = grupo.curso.documentos_curso.Where(dc => dc.documento.unidad == unidad).ToList();
                    }

                    if (nivel != null)
                    {
                        grupo.curso.documentos_curso = grupo.curso.documentos_curso.Where(dc => dc.documento.niveles.Contains(db.niveles.Find(unidad))).ToList();
                    }

                    if (tipo != null)
                    {
                        grupo.curso.documentos_curso = grupo.curso.documentos_curso.Where(dc => dc.documento.tipo_documento.id == tipo).ToList();
                    }

                    if (nombreDoc != null && nombreDoc != "")
                    {
                        grupo.curso.documentos_curso = grupo.curso.documentos_curso.Where(dc => dc.documento.titulo.Contains(nombreDoc) || 
                        dc.documento.descripcion_corta.Contains(nombreDoc) || (dc.documento.descripcion_detallada!=null && dc.documento.descripcion_detallada.Contains(nombreDoc))).ToList();
                    }

                    ViewBag.unidades = db.unidades;
                    ViewBag.niveles = db.niveles;
                    ViewBag.tipos = db.tipo_documento;
                    return View(grupo);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=cursos");
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
