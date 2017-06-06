using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma.Areas.PCD.Models;
using System.Web.Services;
using System.Text;
using Plataforma.App_Start;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class usuariosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: usuarios

        [Authorize]
        public ActionResult Index(string nombre, int? rol, int? colegio, int? nivel)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {

                    colegio colegioTemp = new colegio();
                    colegioTemp.nombre = "zzz";
                    nivele nivelTemp = new nivele();
                    nivelTemp.nivel = "zzz";

                    List<usuario> usuarios = new List<usuario>();
                    if (nombre != null && nombre != "")
                    {
                        usuarios = db.usuarios.Where(u => (u.nombre.Contains(nombre) || u.apellidos.Contains(nombre) || u.username.Contains(nombre) || u.informacion_opcional.Contains(nombre))).ToList();
                    }
                    else
                    {
                        usuarios = db.usuarios.ToList();
                    }

                    if (rol != null)
                    {
                        usuarios = usuarios.Where(u => u.roles.Contains(db.roles.Find(rol))).ToList();
                    }

                    if (colegio != null)
                    {
                        usuarios = usuarios.Where(u => u.colegios.Contains(db.colegios.Find(colegio))).ToList();
                    }

                    if (nivel != null)
                    {
                        usuarios = usuarios.Where(u => u.niveles.Contains(db.niveles.Find(nivel))).ToList();
                    }

                    //foreach (usuario item in usuarios.Where(u => u.colegios == null || u.colegios.Count <= 0))
                    //{
                    //    usuarios.Where(u => u.id == item.id).FirstOrDefault().colegios.Add(colegioTemp);
                    //}
                    //foreach (usuario item in usuarios.Where(u => u.niveles == null || u.niveles.Count <= 0))
                    //{
                    //    usuarios.Where(u => u.id == item.id).FirstOrDefault().niveles.Add(nivelTemp);
                    //}

                    //usuarios =
                    //    usuarios.OrderBy(u => u.roles.FirstOrDefault().rol).
                    //    ThenBy(u => u.nombre + " " + u.apellidos).ToList();
                        //ThenBy(u => u.colegios.FirstOrDefault().nombre).
                        //ThenBy(u => u.niveles.FirstOrDefault().nivel).ToList();
                    ViewBag.TotalVisitas = db.log_visitas.Count();
                    ViewBag.roles = db.roles;
                    ViewBag.colegios = db.colegios;
                    ViewBag.niveles = db.niveles;
                    return View(usuarios);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        [Authorize]
        // GET: usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR) ||
                    id == usuarioSesion.id ||
                    (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM) &&
                    usuarioSesion.grupos.Where(g => g.usuarios.Where(u => u.id == id).ToList().Count > 0).ToList().Count > 0))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    usuario usuario = db.usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(usuario);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        [AllowAnonymous]
        // GET: usuarios/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.id = new SelectList(db.notificaciones, "id_usuario", "id_usuario");
                    ViewBag.id = new SelectList(db.log_visitas, "id_usuario", "id_usuario");
                    ViewBag.roles = new SelectList(db.roles, "id", "rol");
                    ViewBag.colegios = new SelectList(db.colegios, "id", "nombre");
                    ViewBag.niveles = new SelectList(db.niveles, "id", "nivel");
                    ViewBag.notificacionCorreo = true;
                    ViewBag.notificacionTelefono = true;
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            ViewBag.id = new SelectList(db.notificaciones, "id_usuario", "id_usuario");
            ViewBag.id = new SelectList(db.log_visitas, "id_usuario", "id_usuario");
            ViewBag.roles = new SelectList(db.roles.Where(r => !(r.rol.Equals(Constantes.ADMINISTRADOR) ||
                                                               r.rol.Equals(Constantes.PROFESOR_PREMIUM) ||
                                                               r.rol.Equals(Constantes.PROFESOR) ||
                                                               r.rol.Equals(Constantes.ESTUDIANTE_PREMIUM) ||
                                                               r.rol.Equals(Constantes.ESTUDIANTE))),
                                                               "id", "rol");
            ViewBag.colegios = new SelectList(db.colegios, "id", "nombre");
            ViewBag.niveles = new SelectList(db.niveles, "id", "nivel");
            ViewBag.notificacionCorreo = true;
            ViewBag.notificacionTelefono = true;
            return View();
        }

        // POST: usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellidos,username,password,telefono,telefono_2,correo,correo_2,informacion_opcional,fecha_primer_ingreso")] usuario usuario, int roles, List<int> colegios, List<int> niveles, bool? notificacionCorreo, bool? notificacionTelefono)
        {
            if (ModelState.IsValid)
            {
                List<string> destinatarios = new List<string>();
                string asunto = "Bienvenido al sitio Plataforma de Contenidos Digitales";
                string cuerpo;
                usuario.fecha_primer_ingreso = DateTime.Today;
                usuario.roles.Add(db.roles.Find(roles));

                destinatarios.Add(usuario.correo);
                foreach (int colegioTemp in colegios)
                {
                    usuario.colegios.Add(db.colegios.Find(colegioTemp));
                }
                foreach (int nivelTemp in niveles)
                {
                    usuario.niveles.Add(db.niveles.Find(nivelTemp));
                }
                if (usuario.roles.FirstOrDefault().cursos != null && usuario.roles.FirstOrDefault().cursos.Count > 0)
                {
                    usuario.cursos = usuario.roles.FirstOrDefault().cursos;
                }
                notificacione notificacion = new notificacione();
                notificacion.correo = notificacionCorreo;
                notificacion.telefono = notificacionTelefono;
                notificacion.id_usuario = usuario.id;
                notificacion.fecha_hora = DateTime.Now;
                usuario.notificacione = notificacion;
                cuerpo = "Usuario = " + usuario.username + " Password = " + usuario.password;
                usuario.password = Utilitarios.EncodePassword(string.Concat(usuario.username, usuario.password));
                db.usuarios.Add(usuario);
                db.SaveChanges();
                Utilitarios.EnviarCorreo(destinatarios, asunto, cuerpo);
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.notificaciones, "id_usuario", "id_usuario", usuario.id);
            ViewBag.id = new SelectList(db.log_visitas, "id_usuario", "id_usuario", usuario.id);
            ViewBag.roles = new SelectList(db.roles, "id", "rol", usuario.roles);
            ViewBag.colegios = new SelectList(db.colegios, "id", "nombre", usuario.colegios);
            ViewBag.niveles = new SelectList(db.niveles, "id", "nivel");
            return View(usuario);
        }

        [Authorize]
        // GET: usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR) || id == usuarioSesion.id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    usuario usuario = db.usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.nombreUsuario = usuario.username;
                    ViewBag.Roles = db.roles;
                    ViewBag.colegios = db.colegios;
                    ViewBag.notificacionCorreo = usuario.notificacione.correo;
                    ViewBag.notificacionTelefono = usuario.notificacione.telefono;
                    return View(usuario);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        // POST: usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellidos,username,password,telefono,telefono_2,correo,correo_2,informacion_opcional,fecha_primer_ingreso")] usuario usuario, int? roles, bool? notificacionCorreo, bool? notificacionTelefono, string nombreUsuario, string pass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.notificaciones.Find(usuario.id).correo = notificacionCorreo;
                db.notificaciones.Find(usuario.id).telefono = notificacionTelefono;
                db.notificaciones.Find(usuario.id).fecha_hora = DateTime.Now;
                if (roles != null && roles != 0)
                {
                    List<role> rolesUsuario = db.roles.Where(r => r.usuarios.Where(u => u.id == usuario.id).ToList().Count > 0).ToList();
                    usuario.roles = db.usuarios.Find(usuario.id).roles;
                    foreach (role item in rolesUsuario)
                    {
                        item.usuarios.Remove(usuario);
                    }
                    role rol = db.roles.Find(roles);
                    usuario.roles.Add(rol);
                }
                if (pass != null && pass != "" || usuario.username != nombreUsuario)
                {
                    if (db.usuarios.Find(usuario.id).password.Equals(Utilitarios.EncodePassword(nombreUsuario + pass)))
                    {
                        usuario.password = Utilitarios.EncodePassword(usuario.username + pass);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Contraseña Equivocada");
                        ModelState.Remove("username");
                        ViewBag.nombreUsuario = nombreUsuario;
                        ViewBag.Roles = db.roles;
                        ViewBag.colegios = db.colegios;
                        usuario.username = nombreUsuario;
                        return View(usuario);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.nombreUsuario = db.usuarios.Find(usuario.id).username;
            ViewBag.Usuario = db.usuarios.Find(usuario.id).username;
            ViewBag.Roles = db.roles;
            ViewBag.colegios = db.colegios;
            return View(usuario);
        }

        [Authorize]
        // GET: usuarios/Delete/5
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
                    usuario usuario = db.usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(usuario);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        // POST: usuarios/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            foreach (respuesta respuesta in usuario.respuestas)
            {
                db.respuestas.Remove(respuesta);
            }
            db.usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult validarUserName(String username)
        {
            if (db.usuarios.Where(u => u.username.Equals(username)).ToList().Count > 0 || 
                db.profesores_temporal.Where(p => p.username.Equals(username)).ToList().Count > 0)
            {
                return Json("ocupado", JsonRequestBehavior.AllowGet);
            }
            return Json("libre", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BloquearUsuario(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    bool bloqueado = db.usuarios.Find(id).bloqueado.GetValueOrDefault();
                    db.usuarios.Find(id).bloqueado = !bloqueado;
                    db.SaveChanges();

                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult CambiarPassword(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR) || id == usuarioSesion.id)
                {
                    if (id == null)
                    {
                        id = usuarioSesion.id;
                    }
                    ViewBag.idUsuario = id;
                    return View();
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
        public ActionResult CambiarPassword([Bind(Include = "Password, ConfirmPassword")]ResetPasswordViewModel ResetPassword, int? id)
        {
            if (ModelState.IsValid)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (id == null)
                {
                    db.usuarios.Find(usuarioSesion.id).password = Utilitarios.EncodePassword(usuarioSesion.username + ResetPassword.Password);
                }
                else
                {
                    db.usuarios.Find(id).password = Utilitarios.EncodePassword(db.usuarios.Find(id).username + ResetPassword.Password);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        public ActionResult EnviarNotificaciones(int? id, int? idGrupo)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM) || usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    if (id == null)
                    {
                        ViewBag.IdGrupo = idGrupo;
                    }
                    else
                    {
                        ViewBag.Id = id;
                    }
                    return View();
                }
                return RedirectToAction("../");
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        [Authorize]
        [HttpPost]
        public ActionResult EnviarNotificaciones(int? id, int? idGrupo, int? idColegio, int? idCurso, string asunto, string cuerpo)
        {
            List<usuario> usuarios = new List<usuario>();
            List<string> destinatarios = new List<string>();
            usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
            if (id == null)
            {
                if (idGrupo != null)
                {
                    usuarios = db.grupos.Find(idGrupo).usuarios.Where(u => u.id != usuarioSesion.id && u.notificacione.correo == true).ToList();
                }
                if (idColegio != null)
                {
                    usuarios = db.colegios.Find(idColegio).usuarios.Where(u => u.notificacione.correo == true).ToList();
                }
                if (idCurso != null)
                {
                    usuarios = db.colegios.Find(idCurso).usuarios.Where(u => u.notificacione.correo == true).ToList();
                }
            }
            else
            {
                usuarios.Add(db.usuarios.Find(id));
            }
            foreach (usuario destinatario in usuarios)
            {
                destinatarios.Add(destinatario.correo);
                if (destinatario.correo_2 != null && destinatario.correo_2 != "")
                {
                    destinatarios.Add(destinatario.correo_2);
                }
            }
            Utilitarios.EnviarCorreo(destinatarios, asunto, cuerpo);
            return Redirect("~/PCD");
        }

        [AllowAnonymous]
        public Boolean VerificarVigencia(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return false;
            }
            else if (usuario.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
            {
                return true;
            }
            else
            {
                DateTime primerIngreso = (DateTime)usuario.fecha_primer_ingreso;
                TimeSpan diferenciaPrimerIngreso = DateTime.Today - primerIngreso;
                if (usuario.roles.FirstOrDefault().rol.Equals(Constantes.ESTUDIANTE) && diferenciaPrimerIngreso.Days <= 1095)
                {
                    return true;
                }
                else if ((!usuario.roles.FirstOrDefault().rol.Equals(Constantes.ESTUDIANTE)) && diferenciaPrimerIngreso.Days <= 365)
                {
                    return true;
                }

            }
            return false;
        }

        [Authorize]
        [HttpPost]
        public ActionResult RenovarSubscripcion(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    db.usuarios.Find(id).fecha_primer_ingreso = DateTime.Today;
                    db.SaveChanges();
                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CerrarSesiones()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<usuario> usuarios = db.usuarios.Where(u => u.logueado == true).ToList();
                    foreach (usuario item in usuarios)
                    {
                        db.usuarios.Find(item.id).logueado = false;
                    }
                    db.SaveChanges();
                    return Json("Exito", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public void NotificarVencimiento()
        {
            List<usuario> usuarios = db.usuarios.ToList();
            string asunto = "Recordatorio de vencimiento de su membrecia en la Plataforma de Contenidos Digitales.";
            foreach (usuario usuario in usuarios)
            {
                if (!usuario.roles.FirstOrDefault().Equals(Constantes.ADMINISTRADOR))
                {
                    List<string> destinatarios = new List<string>();
                    destinatarios.Add(usuario.correo);
                    if (usuario.correo_2 != null)
                    {
                        destinatarios.Add(usuario.correo_2);
                    }
                    DateTime fechaVencimiento = new DateTime(usuario.fecha_primer_ingreso.Value.Year + 1, usuario.fecha_primer_ingreso.Value.Month, usuario.fecha_primer_ingreso.Value.Day);
                    TimeSpan diferenciaRenovacion = fechaVencimiento - DateTime.Today;
                    if (diferenciaRenovacion.Days == 30 || diferenciaRenovacion.Days == 15 || diferenciaRenovacion.Days == 3)
                    {
                        string mensaje = "Estimado " + usuario.nombre + "<br /> Por medio de este mensaje deseamos darle a conocer que su membresia en el sitio Plataforma de Contenidos Digitales se vencera en: " + diferenciaRenovacion + " dias." +
                            "<br/><br/>Se recomienda comunicarse con el equipo de www.pimas.co.cr para la renovación de dicha membresia. <br/> <br/> Saludos.";
                        Utilitarios.EnviarCorreo(destinatarios, asunto, mensaje);
                    }
                }
            }
        }

        [Authorize]
        public ActionResult AdministrarInstitucionesUsuario(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR) || id == usuarioSesion.id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    usuario usuario = db.usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    List<colegio> colegios = db.colegios.ToList();
                    foreach (colegio item in usuario.colegios)
                    {
                        colegios.Remove(item);
                    }
                    ViewBag.colegiosDisponibles = colegios;
                    return View(usuario);
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
        public ActionResult AdministrarInstitucionesUsuario(int id, List<int> colegios)
        {
            usuario usuario = db.usuarios.Find(id);
            for (int i = 0; i < usuario.colegios.Count; i++)
            {
                usuario.colegios.Remove(usuario.colegios.ElementAt(i));
                i--;
            }
            if (colegios != null && colegios.Count != 0)
            {
                foreach (int colegioTemp in colegios)
                {
                    if (colegioTemp > 0)
                    {
                        colegio colegioObj = db.colegios.Find(colegioTemp);
                        if (!colegioObj.usuarios.Contains(usuario))
                        {
                            usuario.colegios.Add(colegioObj);
                        }
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AdministrarNivelesUsuario(int? id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if ((usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR) || id == usuarioSesion.id))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    usuario usuario = db.usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    if (!(usuario.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR) || usuario.roles.FirstOrDefault().rol.Equals(Constantes.PROFESOR_PREMIUM)))
                    {
                        return RedirectToAction("../");
                    }
                    List<nivele> niveles = db.niveles.ToList();
                    foreach (nivele item in usuario.niveles)
                    {
                        niveles.Remove(item);
                    }
                    ViewBag.nivelesDisponibles = niveles;
                    return View(usuario);
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
        public ActionResult AdministrarNivelesUsuario(int id, List<int> niveles)
        {
            usuario usuario = db.usuarios.Find(id);
            for (int i = 0; i < usuario.niveles.Count; i++)
            {
                usuario.niveles.Remove(usuario.niveles.ElementAt(i));
                i--;
            }
            if (niveles != null && niveles.Count != 0)
            {
                foreach (int nivelTemp in niveles)
                {
                    if (nivelTemp > 0)
                    {
                        nivele nivelObj = db.niveles.Find(nivelTemp);
                        if (!nivelObj.usuarios.Contains(usuario))
                        {
                            usuario.niveles.Add(nivelObj);
                        }
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult GenerarUsuarios()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if ((usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR)))
                {
                    ViewBag.cursos = db.cursos.ToList();
                    return View();
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
        public ActionResult GenerarUsuarios(List<int> cursos, int CantidadUsuarios)
        {

            //DSUsuariosGenerados dsUsuarios = new DSUsuariosGenerados();
            List<curso> cursosMatricular = new List<curso>();
            usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
            List<string> destinatarios = new List<string>();
            destinatarios.Add(usuarioSesion.correo);
            if (usuarioSesion.correo_2 != null || usuarioSesion.correo_2 != "")
            {
                destinatarios.Add(usuarioSesion.correo_2);
            }

            destinatarios.Add("editorial@pimas.co.cr");
            foreach (int item in cursos)
            {
                cursosMatricular.Add(db.cursos.Find(item));
            }
            if (cursosMatricular != null && cursosMatricular.Count > 0)
            {
                if (CantidadUsuarios > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i <= CantidadUsuarios; i++)
                    {
                        usuario estudiante = new usuario();
                        estudiante.apellidos = Guid.NewGuid().ToString().Substring(0, 2);
                        estudiante.nombre = Guid.NewGuid().ToString().Substring(0, 3);
                        estudiante.correo = Guid.NewGuid().ToString().Substring(0, 3);
                        estudiante.telefono = 0;
                        estudiante.username = Guid.NewGuid().ToString().Substring(0, 10);
                        while (db.usuarios.Where(u => u.username.Equals(estudiante.username)).Count() > 0)
                        {
                            estudiante.username = Guid.NewGuid().ToString().Substring(0, 10);
                        }
                        estudiante.password = Guid.NewGuid().ToString().Substring(0, 10);
                        sb.AppendLine("Username = " + estudiante.username + " Password = " + estudiante.password);
                        //dsUsuarios.Tables[0].Rows.Add(estudiante.username, estudiante.password);
                        estudiante.password = Utilitarios.EncodePassword(string.Concat(estudiante.username, estudiante.password));
                        estudiante.fecha_primer_ingreso = DateTime.Today;
                        estudiante.roles = db.roles.Where(r => r.rol.Equals(Constantes.ESTUDIANTE)).ToList();
                        foreach (curso curso in cursosMatricular)
                        {
                            estudiante.cursos.Add(curso);
                        }
                        notificacione notificacion = new notificacione();
                        notificacion.telefono = true;
                        notificacion.correo = true;
                        notificacion.fecha_hora = DateTime.Now;
                        estudiante.notificacione = notificacion;
                        db.usuarios.Add(estudiante);
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
                    pdfDoc.AddTitle("Reporte de usuarios generados");
                    pdfDoc.Add(table);
                    Paragraph Text = new Paragraph("\n\n" + sb.ToString());
                    pdfDoc.Add(Text);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    s = new MemoryStream(s.ToArray());
                    Utilitarios.EnviarCorreoAdjunto(destinatarios, "Datos de usuarios generados: ",
                        "Adjunto encontrarás un documento PDF con los datos de acceso para los usuarios generados el dia " + DateTime.Today, s);
                    s.Close();
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.cursos = db.cursos.ToList();
                return View();
            }
        }


        public ActionResult ReporteDocumentosUsuario(int id)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if ((usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR)))
                {
                    MemoryStream s = new MemoryStream();
                    try
                    {
                        usuario usuario = db.usuarios.Find(id);

                        PdfPTable tableHeader = new PdfPTable(5);

                        PdfPCell celImagen = new PdfPCell(Image.GetInstance(Path.Combine(Request.PhysicalApplicationPath, "Recursos", "logo-peq.png")));
                        celImagen.Border = 0;

                        PdfPCell celTitulo = new PdfPCell(new Phrase("Plataforma de Contenidos Digitales" +
                            "\n" + DateTime.Today.ToShortDateString().ToString() +
                            "\nReporte de documentos visualizados por " + usuario.nombre + " " + usuario.apellidos,
                            new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                        celTitulo.Colspan = 4;
                        celTitulo.Border = 0;

                        tableHeader.AddCell(celImagen);
                        tableHeader.AddCell(celTitulo);

                        PdfPTable tableContenido = new PdfPTable(2);

                        PdfPCell celdaDocumento = new PdfPCell(new Phrase("Documento",
                                new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celdaDocumento.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell celdaVisitas = new PdfPCell(new Phrase("Visitas",
                                new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celdaVisitas.HorizontalAlignment = Element.ALIGN_CENTER;

                        float promedio = (float)usuario.log_visitas_documentousuario.Sum(d => d.contador) / (float)usuario.log_visitas.GroupBy(v => v.fecha_hora.Date).Count();

                        PdfPCell celdaPromedio = new PdfPCell(new Phrase(("En promedio el usuario ha visualizado: " +
                            promedio +
                            " documentos por dia"),
                                new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
                        celdaPromedio.HorizontalAlignment = Element.ALIGN_CENTER;
                        celdaPromedio.Colspan = 2;

                        tableContenido.AddCell(celdaDocumento);
                        tableContenido.AddCell(celdaVisitas);

                        foreach (var item in usuario.log_visitas_documentousuario)
                        {
                            tableContenido.AddCell(item.documento.titulo);
                            tableContenido.AddCell(item.contador.ToString());
                        }

                        tableContenido.AddCell(celdaPromedio);

                        Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, s);

                        pdfDoc.Open();
                        pdfDoc.AddTitle("Reporte de usuarios generados");
                        pdfDoc.Add(tableHeader);
                        Paragraph Text = new Paragraph("\n\n");
                        pdfDoc.Add(Text);
                        pdfDoc.Add(tableContenido);
                        pdfDoc.Add(Text);
                        pdfWriter.CloseStream = false;
                        pdfDoc.Close();
                        s = new MemoryStream(s.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                    return File(s, "application/pdf");
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=usuarios");
        }

        [AllowAnonymous]
        public ActionResult CambiarPasswordUsuario()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CambiarPasswordUsuario(String username)
        {
            if (db.usuarios.Where(u => u.username.Equals(username)).ToList().Count > 0 || db.profesores_temporal.Where(p => p.username.Equals(username)).ToList().Count > 0)
            {
                usuario usuario = db.usuarios.Where(u => u.username.Equals(username)).FirstOrDefault();
                string password = Guid.NewGuid().ToString().Substring(0, 10);
                db.usuarios.Find(usuario.id).password = 
                    Utilitarios.EncodePassword(string.Concat(usuario.username, password));
                db.SaveChanges();
                List<string> destinatarios = new List<string>();
                destinatarios.Add(usuario.correo);
                if (usuario.correo_2 != null)
                {
                    destinatarios.Add(usuario.correo_2);
                }
                string mensaje = "Estimado " + usuario.nombre + 
                    "<br /> Por medio de este mensaje deseamos darle a conocer que el cambio de su contraseña en el sitio Plataforma de Contenidos Digitales se realizo exitosamente, su nueva contraseña es: " + password +
                    "<br/> <br/> Saludos.";
                Utilitarios.EnviarCorreo(destinatarios, "Cambio de contraseña", mensaje);
                return Redirect("../Account/Login");
            }
            return RedirectToAction("UsuarioIncorrecto");
        }
        [AllowAnonymous]
        public ActionResult UsuarioIncorrecto()
        {
            return View();
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
