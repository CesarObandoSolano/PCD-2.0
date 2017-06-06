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
    public class rolesController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: roles
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<role> roles = new List<role>();
                    if (nombre != null)
                    {
                        roles = db.roles.Where(r => r.rol.Contains(nombre)).ToList();
                    }
                    else
                    {
                        roles = db.roles.ToList();
                    }
                    return View(roles);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=roles");
        }

        // GET: roles/Details/5
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
                    role role = db.roles.Find(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }
                    return View(role);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=roles");
        }

        // GET: roles/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.permiso = new MultiSelectList(db.permisos, "id", "descripcion");
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=roles");
        }

        // POST: roles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,rol,permisos")] role role, ICollection<int> permiso)
        {
            if (ModelState.IsValid)
            {
                foreach (int permisoSeleccionado in permiso)
                {
                    permiso permisoobj = db.permisos.Find(permisoSeleccionado);
                    role.permisos.Add(permisoobj);
                }
                db.roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: roles/Edit/5
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
                    role role = db.roles.Find(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }
                    List<permiso> permisos = db.permisos.ToList();
                    for (int i = 0; i < permisos.Count; i++)
                    {
                        foreach (var item in role.permisos)
                        {
                            if (permisos.ElementAt(i).id == item.id)
                            {
                                permisos.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }
                    List<curso> cursos = db.cursos.ToList();
                    for (int i = 0; i < cursos.Count; i++)
                    {
                        foreach (var item in role.cursos)
                        {
                            if (cursos.ElementAt(i).id == item.id)
                            {
                                cursos.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }
                    ViewBag.ListaCursos = new SelectList(cursos, "id", "curso1");
                    ViewBag.ListaPermisos = new SelectList(permisos, "id", "descripcion");
                    return View(role);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=roles");
        }

        // POST: roles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,rol")] role role, ICollection<int> listaPermisos, ICollection<int> quitarPermisos, ICollection<int> listaCursos, ICollection<int> quitarCursos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                List<usuario> usuarios = db.usuarios.Where(u => u.roles.FirstOrDefault().id == role.id).ToList();
                if (quitarPermisos != null)
                {
                    foreach (int permisoTemporal in quitarPermisos)
                    {
                        permiso permisoObj = db.permisos.Find(permisoTemporal);
                        db.permisos.Find(permisoObj.id).roles.Remove(role);
                    }
                }
                if (listaPermisos != null)
                {
                    foreach (int permisoTemporal in listaPermisos)
                    {
                        permiso permisoObj = db.permisos.Find(permisoTemporal);
                        role.permisos.Add(permisoObj);
                    }
                }
                if (quitarCursos != null)
                {
                    foreach (int cursoTemporal in quitarCursos)
                    {
                        curso cursoObj = db.cursos.Find(cursoTemporal);
                        db.cursos.Find(cursoObj.id).roles.Remove(role);
                        foreach (usuario usuario in usuarios)
                        {
                            db.usuarios.Find(usuario.id).cursos.Remove(cursoObj);
                        }
                    }
                }
                if (listaCursos != null)
                {
                    foreach (int cursoTemporal in listaCursos)
                    {
                        curso cursoObj = db.cursos.Find(cursoTemporal);
                        role.cursos.Add(cursoObj);
                        foreach (usuario usuario in usuarios)
                        {
                            if (cursoObj.usuarios.Where(u => u.id == usuario.id).ToList().Count <= 0)
                            {
                               db.cursos.Find(cursoObj.id).usuarios.Add(usuario);
                            }
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: roles/Delete/5
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
                    role role = db.roles.Find(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }
                    return View(role);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=roles");
        }

        // POST: roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            role role = db.roles.Find(id);
            db.roles.Remove(role);
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
