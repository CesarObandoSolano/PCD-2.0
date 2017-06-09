using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;
using System.Text;
using Plataforma.App_Start;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class gruposController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: grupos
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<grupos> grupos = new List<grupos>();
                    if (nombre != null)
                    {
                        grupos = db.grupos.Where(g => g.grupo.Contains(nombre)).ToList();
                    }
                    else
                    {
                        grupos = db.grupos.ToList();
                    }
                    return View(grupos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=grupos");
        }

        // GET: grupos/Details/5
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
                    grupos grupos = db.grupos.Find(id);
                    if (grupos == null)
                    {
                        return HttpNotFound();
                    }
                    return View(grupos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=grupos");
        }

        // GET: grupos/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.permiso = new MultiSelectList(db.permisos, "id", "descripcion");
                    ViewBag.cursos = new SelectList(db.cursos, "id", "curso1");
                    ViewBag.CantidadUsuarios = 0;
                    ICollection<usuario> profesores = db.usuarios.Where(u => u.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM)).ToList();
                    ViewBag.profesores = profesores;
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=grupos");
        }

        // POST: grupos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,grupo, permisos")] grupos grupos, ICollection<int> permiso, int profesores, int cursos, int CantidadUsuarios)
        {
            if (ModelState.IsValid)
            {
                foreach (int permisoSeleccionado in permiso)
                {
                    permiso permisoobj = db.permisos.Find(permisoSeleccionado);
                    grupos.permisos.Add(permisoobj);
                }
                usuario usuario = db.usuarios.Find(profesores);
                grupos.usuarios.Add(usuario);
                curso curso = db.cursos.Find(cursos);
                curso.usuarios.Add(usuario);
                grupos.curso = curso;
                List<string> destinatarios = new List<string>();
                destinatarios.Add(usuario.correo);
                if(usuario.correo_2 != null || usuario.correo_2 != "")
                {
                    destinatarios.Add(usuario.correo_2);
                }
                destinatarios.Add("editorial@pimas.co.cr");
                if (CantidadUsuarios > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i <= CantidadUsuarios; i ++)
                    {
                        usuario estudiante = new usuario();
                        estudiante.apellidos = Guid.NewGuid().ToString().Substring(0, 2);
                        estudiante.nombre = Guid.NewGuid().ToString().Substring(0, 3);
                        estudiante.correo = Guid.NewGuid().ToString().Substring(0, 3);
                        estudiante.telefono = 0;
                        estudiante.username = Guid.NewGuid().ToString().Substring(0,10);
                        while (db.usuarios.Where(u=>u.username.Equals(estudiante.username)).Count()>0)
                        {
                            estudiante.username = Guid.NewGuid().ToString().Substring(0, 10);
                        }
                        estudiante.password = Guid.NewGuid().ToString().Substring(0, 10);
                        sb.AppendLine("Username = " + estudiante.username + " Password = " + estudiante.password);
                        estudiante.password = Utilitarios.EncodePassword(string.Concat(estudiante.username, estudiante.password));
                        estudiante.fecha_primer_ingreso = DateTime.Today;
                        estudiante.roles = db.roles.Where(r => r.rol.Equals(Constantes.ESTUDIANTE_PREMIUM)).ToList();
                        estudiante.cursos.Add(curso);
                        notificacione notificacion = new notificacione();
                        notificacion.telefono = true;
                        notificacion.correo = true;
                        notificacion.fecha_hora = DateTime.Now;
                        estudiante.notificacione = notificacion;
                        grupos.usuarios.Add(estudiante);
                    }
                    PdfPTable table = new PdfPTable(5);

                    PdfPCell celImagen = new PdfPCell(Image.GetInstance(Path.Combine(Request.PhysicalApplicationPath, "Recursos", "logo-peq.png")));
                    celImagen.Border = 0;

                    PdfPCell celTitulo = new PdfPCell(new Phrase("Plataforma de Contenidos Digitales" +
                        "\n" + DateTime.Today.ToShortDateString().ToString() +
                        "\nReporte de usuarios generados",
                        new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                    celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                    celTitulo.Colspan = 4;
                    celTitulo.Border = 0;

                    table.AddCell(celImagen);
                    table.AddCell(celTitulo);
                    MemoryStream s = new MemoryStream();

                    Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, s);
                    pdfDoc.Open();
                    iTextSharp.text.Image logoPimas = iTextSharp.text.Image.GetInstance("~/Recursos/Aplicacion/logo-pimas.png");
                    
                    logoPimas.ScaleToFit(90, 45);
                    logoPimas.Alignment = Element.ALIGN_LEFT;
                    pdfDoc.Add(logoPimas);
                    pdfDoc.AddTitle("Reporte de usuarios generados");
                    pdfDoc.Add(table);
                    Paragraph Text = new Paragraph("\n\n" + sb.ToString());
                    pdfDoc.Add(Text);
                    Paragraph footer = new Paragraph("\n\nPublicaciones Innovadoras en Matemática para Secundaria PIMAS 	 Cédula Jurídica: 3-101-469172" +
                                        "editorial @pimas.co.cr ⧫ www.pimas.co.cr ⧫ Facebook / PimasCR ⧫ Tel: 8310 0573");
                    pdfDoc.Add(footer);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    s = new MemoryStream(s.ToArray());
                    Utilitarios.EnviarCorreoAdjunto(destinatarios, "Datos de usuarios generados: ",
                        "Estimado "+ usuario.nombre + " "+ usuario.apellidos +
                        ": Adjunto encontrará un documento PDF con los datos de acceso para los usuarios generados el dia "
                        + DateTime.Today.Date + " y las indicaciones necesarias.", s);
                    


                    s.Close();
                    //Utilitarios.EnviarCorreo(destinatarios, "Datos de usuarios del grupo: " + grupos.grupo, sb.ToString());
                }
                db.grupos.Add(grupos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupos);
        }

        // GET: grupos/Edit/5
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
                    grupos grupos = db.grupos.Find(id);
                    if (grupos == null)
                    {
                        return HttpNotFound();
                    }
                    List<int> disabled = new List<int>();
                    foreach (permiso permisoTemp in grupos.permisos)
                    {
                        disabled.Add(permisoTemp.id);
                    }
                    ViewBag.ListaPermisos = new SelectList(db.permisos, "id", "descripcion", null, null, disabled);
                    return View(grupos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=grupos");
        }

        // POST: grupos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,grupo")] grupos grupos, ICollection<int> listaPermisos, ICollection<int> quitarPermisos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupos).State = EntityState.Modified;
                if (quitarPermisos != null)
                {
                    foreach (int permisoTemporal in quitarPermisos)
                    {
                        permiso permisoObj = db.permisos.Find(permisoTemporal);
                        db.permisos.Find(permisoObj.id).grupos.Remove(grupos);
                    }
                }
                if (listaPermisos != null)
                {
                    foreach (int permisoTemporal in listaPermisos)
                    {
                        permiso permisoObj = db.permisos.Find(permisoTemporal);
                        grupos.permisos.Add(permisoObj);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupos);
        }

        // GET: grupos/Delete/5
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
                    grupos grupos = db.grupos.Find(id);
                    if (grupos == null)
                    {
                        return HttpNotFound();
                    }
                    return View(grupos);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=grupos");
        }

        // POST: grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            grupos grupos = db.grupos.Find(id);
            db.grupos.Remove(grupos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DesmatricularUsuario(int id, int idGrupo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM) && usuarioSesion.id != id)
                {
                    db.grupos.Find(idGrupo).usuarios.Remove(db.usuarios.Find(id));
                    db.SaveChanges();
                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarComentario(int idDocumento, int idGrupo, string comentario)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                //if (usuarioSesion.cursos.Where(c => c.documentos_curso.Where(dc => dc.id_documento == idDocumento).ToList().Count > 0).ToList().Count>0)
                //{
                comentario comentarioObj = new comentario();
                comentarioObj.id_documento = idDocumento;
                comentarioObj.id_grupo = idGrupo;
                comentarioObj.id_usuario = usuarioSesion.id;
                comentarioObj.fecha_publicacion = DateTime.Now;
                comentarioObj.comentario1 = comentario;
                db.comentarios.Add(comentarioObj);
                db.SaveChanges();
                return Json("Exito", JsonRequestBehavior.AllowGet);
                //}
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarRespuesta(int idComentario, string respuesta)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                //if (usuarioSesion.cursos.Where(c => c.documentos_curso.Where(dc => dc.id_documento == idDocumento).ToList().Count > 0).ToList().Count>0)
                //{
                respuesta respuestaObj = new respuesta();
                respuestaObj.id_comentario = idComentario;
                respuestaObj.respuesta1 = respuesta;
                respuestaObj.id_usuario = usuarioSesion.id;
                respuestaObj.fecha_publicacion = DateTime.Now;
                db.respuestas.Add(respuestaObj);
                db.SaveChanges();
                return Json("Exito", JsonRequestBehavior.AllowGet);
                //}
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
