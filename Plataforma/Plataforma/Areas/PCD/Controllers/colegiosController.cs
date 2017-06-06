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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class colegiosController : Controller
    {
        private pimasEntities db = new pimasEntities();

        // GET: colegios
        public ActionResult Index(string nombre)
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    List<colegio> colegios = new List<colegio>();
                    if (nombre != null)
                    {
                        colegios = db.colegios.Where(c => c.nombre.Contains(nombre)).Include(c => c.pais).ToList();
                    }
                    else
                    {
                        colegios = db.colegios.Include(c => c.pais).ToList();
                    }
                    colegios = colegios.OrderBy(c => c.id_tipo_colegio).
                        ThenBy(c=>c.nombre).ToList();
                    return View(colegios);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=colegios");
        }

        // GET: colegios/Details/5
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
                    colegio colegio = db.colegios.Find(id);
                    if (colegio == null)
                    {
                        return HttpNotFound();
                    }
                    return View(colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=colegios");
        }

        // GET: colegios/Create
        public ActionResult Create()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                if (usuarioSesion.roles.FirstOrDefault().rol.Equals(Constantes.ADMINISTRADOR))
                {
                    ViewBag.id_pais = new SelectList(db.paises, "id", "pais1");
                    ViewBag.id_tipo_colegio = new SelectList(db.tipo_colegio, "id", "tipo_institucion");
                    return View();
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=colegios");
        }

        // POST: colegios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_tipo_colegio,id_pais,nombre")] colegio colegio)
        {
            if (ModelState.IsValid)
            {
                db.colegios.Add(colegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_pais = new SelectList(db.paises, "id", "pais1", colegio.id_pais);
            ViewBag.id_tipo_colegio = new SelectList(db.tipo_colegio, "id", "tipo_institucion", colegio.id_tipo_colegio);
            return View(colegio);
        }

        // GET: colegios/Edit/5
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
                    colegio colegio = db.colegios.Find(id);
                    if (colegio == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.id_pais = new SelectList(db.paises, "id", "pais1", colegio.id_pais);
                    ViewBag.id_tipo_colegio = new SelectList(db.tipo_colegio, "id", "tipo_institucion");
                    return View(colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=colegios");
        }

        // POST: colegios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_tipo_colegio,id_pais,nombre")] colegio colegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_pais = new SelectList(db.paises, "id", "pais1", colegio.id_pais);
            ViewBag.id_tipo_colegio = new SelectList(db.tipo_colegio, "id", "tipo_institucion", colegio.id_tipo_colegio);
            return View(colegio);
        }

        // GET: colegios/Delete/5
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
                    colegio colegio = db.colegios.Find(id);
                    if (colegio == null)
                    {
                        return HttpNotFound();
                    }
                    return View(colegio);
                }
                else
                {
                    return RedirectToAction("../");
                }
            }
            return RedirectToAction("../Account/Login/ReturnUrl=colegios");
        }

        // POST: colegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            colegio colegio = db.colegios.Find(id);
            db.colegios.Remove(colegio);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                        colegio colegio = db.colegios.Find(id);

                        PdfPTable tableHeader = new PdfPTable(5);

                        PdfPCell celImagen = new PdfPCell(Image.GetInstance(Path.Combine(Request.PhysicalApplicationPath, "Recursos", "logo-peq.png")));
                        celImagen.Border = 0;

                        PdfPCell celTitulo = new PdfPCell(new Phrase("Plataforma de Contenidos Digitales" +
                            "\n" + DateTime.Today.ToShortDateString().ToString() +
                            "\nReporte de documentos visualizados por los usuarios del colegio " + colegio.nombre,
                            new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                        celTitulo.Colspan = 4;
                        celTitulo.Border = 0;

                        tableHeader.AddCell(celImagen);
                        tableHeader.AddCell(celTitulo);

                        PdfPTable tableContenido = new PdfPTable(2);

                        PdfPCell celdaNombre = new PdfPCell(new Phrase("Usuario",
                                new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celdaNombre.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell celdaVisitas = new PdfPCell(new Phrase("Visitas",
                                new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                        celdaVisitas.HorizontalAlignment = Element.ALIGN_CENTER;

                        List<float> promediosUsuario = new List<float>();

                        tableContenido.AddCell(celdaNombre);
                        tableContenido.AddCell(celdaVisitas);

                        foreach (var usuario in colegio.usuarios)
                        {
                            tableContenido.AddCell(usuario.nombre + " " + usuario.apellidos);
                            tableContenido.AddCell(usuario.log_visitas_documentousuario.Sum(d => d.contador).ToString());
                            if (usuario.log_visitas.GroupBy(v => v.fecha_hora.Date).Count() > 0)
                            {
                                promediosUsuario.Add((float)usuario.log_visitas_documentousuario.Sum(d => d.contador) / (float)usuario.log_visitas.GroupBy(v => v.fecha_hora.Date).Count());
                            }
                        }

                        PdfPCell celdaPromedio = new PdfPCell(new Phrase(("En promedio los usuarios han visualizado: " +
                            promediosUsuario.Sum() / promediosUsuario.Count +
                            " documentos por dia"),
                                new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
                        celdaPromedio.HorizontalAlignment = Element.ALIGN_CENTER;
                        celdaPromedio.Colspan = 2;

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

