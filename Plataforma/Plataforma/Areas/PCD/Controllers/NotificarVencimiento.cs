using Plataforma.Areas.PCD.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plataforma.Areas.PCD.Controllers
{
    public class NotificarVencimiento : IJob
    {
        private pimasEntities db = new pimasEntities();
        public void Execute(IJobExecutionContext context)
        {
            List<usuario> usuarios = db.usuarios.ToList();
            string asunto = "Recordatorio de vencimiento de su membrecia en la Plataforma de Contenidos Digitales.";
            foreach (usuario usuario in usuarios)
            {
                if (!usuario.roles.FirstOrDefault().Equals("Administrador"))
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
                        string mensaje = "Estimado " + usuario.nombre + "<br /> Por medio de este mensaje deseamos darle a conocer que su membresia en el sitio Plataforma de Contenidos Digitales se vencera en: " + diferenciaRenovacion.Days + " dias." +
                            "<br/><br/>Se recomienda comunicarse con el equipo de <a href='www.pimas.co.cr'>PIMAS</a> para la renovación de dicha membresia. <br/> <br/> Gracias.";
                        Utilitarios.EnviarCorreo(destinatarios, asunto, mensaje);
                    }
                }
            }
        }
    }
}