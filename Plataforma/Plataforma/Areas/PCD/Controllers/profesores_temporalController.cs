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
    
    public class profesores_temporalController : Controller
    {
        private pimasEntities db = new pimasEntities();

        [Authorize]
        // GET: profesores_temporal
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<profesores_temporal> profes = new List<profesores_temporal>();
                    if (nombre != null)
                    {
                        profes = db.profesores_temporal.Where(p => p.nombre.Contains(nombre) || p.apellidos.Contains(nombre)).ToList();
                    }
                    else
                    {
                        profes = db.profesores_temporal.ToList();
                    }
                    return View(profes);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=profesores_temporal");
        }

        [Authorize]
        // GET: profesores_temporal/Details/5
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
                    profesores_temporal profesores_temporal = db.profesores_temporal.Find(id);
                    if (profesores_temporal == null)
                    {
                        return HttpNotFound();
                    }
                    return View(profesores_temporal);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=profesores_temporal");
        }

        public ActionResult SolicitudInscripcion()
        {
            return View();
        }

        [AllowAnonymous]
        // GET: profesores_temporal/Create
        public ActionResult Create()
        {
            ViewBag.Colegios = db.colegios.ToList();
            return View();
        }

        // POST: profesores_temporal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellidos,username,password,colegio,correo,telefono,informacion_opcional")] profesores_temporal profesores_temporal)
        {
            if (ModelState.IsValid)
            {
                string pass = profesores_temporal.password;
                profesores_temporal.password = Utilitarios.EncodePassword(string.Concat(profesores_temporal.username, profesores_temporal.password));
                db.profesores_temporal.Add(profesores_temporal);
                db.SaveChanges();
                List<string> destinatarios = new List<string>();
                destinatarios.Add(profesores_temporal.correo);
                Utilitarios.EnviarCorreo(destinatarios, "PCD: Solicitud Recibida", 
                    "Estimado "+ profesores_temporal.nombre +
                    "<br /><br />Hemos recibido una solicitud de registro en el sitio Plataforma Contenidos Digitales, dicha solicitud será procesada y" +
                    " se le notificará una vez acabe el proceso.<br /><br />Le recordamos que:<br />Su usuario registrado es: " + profesores_temporal.username +
                    "<br/>Su password es: "+pass+
                    "<br /><br />Saludos");
                return RedirectToAction("SolicitudInscripcion");
            }
            ViewBag.Colegios = db.colegios.ToList();
            return View(profesores_temporal);
        }
        [Authorize]
        // GET: profesores_temporal/Edit/5
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
                    profesores_temporal profesores_temporal = db.profesores_temporal.Find(id);
                    if (profesores_temporal == null)
                    {
                        return HttpNotFound();
                    }
                    return View(profesores_temporal);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=profesores_temporal");
        }

        // POST: profesores_temporal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellidos,username,password,colegio,correo,telefono,informacion_opcional")] profesores_temporal profesores_temporal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profesores_temporal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profesores_temporal);
        }

        [Authorize]
        // GET: profesores_temporal/Delete/5
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
                    profesores_temporal profesores_temporal = db.profesores_temporal.Find(id);
                    if (profesores_temporal == null)
                    {
                        return HttpNotFound();
                    }
                    db.profesores_temporal.Remove(profesores_temporal);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=profesores_temporal");
        }

        // POST: profesores_temporal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            profesores_temporal profesores_temporal = db.profesores_temporal.Find(id);
            db.profesores_temporal.Remove(profesores_temporal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Registrar(int? id)
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
                    profesores_temporal profesores_temporal = db.profesores_temporal.Find(id);
                    if (profesores_temporal == null)
                    {
                        return HttpNotFound();
                    }
                    usuario usuario = new usuario();
                    usuario.nombre = profesores_temporal.nombre;
                    usuario.apellidos = profesores_temporal.apellidos;
                    usuario.username = profesores_temporal.username;
                    usuario.password = profesores_temporal.password;
                    usuario.telefono = profesores_temporal.telefono;
                    usuario.correo = profesores_temporal.correo;

                    colegio colegio = db.colegios.Where(c=>c.nombre.Equals(profesores_temporal.colegio)).FirstOrDefault();
                    if (colegio != null)
                    {
                        usuario.colegios.Add(colegio);
                    }
                    
                    usuario.informacion_opcional = profesores_temporal.informacion_opcional;
                    usuario.fecha_primer_ingreso = DateTime.Today;
                    usuario.roles = db.roles.Where(r => r.rol.Equals(Constantes.PROFESOR)).ToList();

                    if (usuario.roles.FirstOrDefault().cursos.Count > 0)
                    {
                        foreach (curso curso in usuario.roles.FirstOrDefault().cursos)
                        {
                            usuario.cursos.Add(curso);
                        }
                    }

                    notificacione notificacion = new notificacione();
                    notificacion.telefono = true;
                    notificacion.correo = true;
                    notificacion.fecha_hora = DateTime.Now;
                    usuario.notificacione = notificacion;
                    db.usuarios.Add(usuario);
                    db.profesores_temporal.Remove(profesores_temporal);
                    db.SaveChanges();
                    List<string> destinatarios = new List<string>();
                    destinatarios.Add(profesores_temporal.correo);
                    Utilitarios.EnviarCorreo(destinatarios, "PCD: Solicitud Aceptada",
                        "Estimado "+ usuario.nombre  +
                        "<br /><br />El proceso de registro de usuario en el sitio Plataforma de Contenidos Digitales ha concluido y su solicitud fue aceptada.<br/><br />Gracias." +
                        "<br /><br />Le recordamos que:<br /> su usuario registrado es: " + usuario.username +
                        "<br />Esperamos que los contenidos de la plataforma le sean útiles en sus labores."
                        );
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=profesores_temporal");
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
