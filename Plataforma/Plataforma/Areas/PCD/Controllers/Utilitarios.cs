using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Plataforma.Areas.PCD.Controllers
{
    public class Utilitarios
    {

        public static string EncodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }

        public static void EnviarCorreo(List<string> destinatarios, string asunto, string cuerpo)
        {
            MailMessage mensaje = new MailMessage();
            foreach (string destinatario in destinatarios)
            {
                if (destinatario != null && destinatario.Contains("@"))
                {
                    mensaje.To.Add(new MailAddress(destinatario));
                }
            }
            mensaje.From = new MailAddress("plataforma@pimas.co.cr");
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.Priority = MailPriority.Normal;
            mensaje.IsBodyHtml = true;

            SmtpClient emisor = new SmtpClient();
            string output = "";
            try
            {
                emisor.Send(mensaje);
                emisor.Dispose();
                output = "Correo electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }
            Console.WriteLine(output);
        }

        public static void EnviarCorreoAdjunto(List<string> destinatarios, string asunto, string cuerpo, Stream archivo)
        {
            MailMessage mensaje = new MailMessage();
            foreach (string destinatario in destinatarios) {
                if (destinatario != null && destinatario.Contains("@"))
                {
                    mensaje.To.Add(new MailAddress(destinatario));
                }
            }
            mensaje.From = new MailAddress("plataforma@pimas.co.cr");
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.Priority = MailPriority.Normal;
            mensaje.IsBodyHtml = true;

            
            SmtpClient emisor = new SmtpClient();
            string output = "";
            try
            {
                mensaje.Attachments.Add(new Attachment(archivo, "Reporte.pdf"));
                emisor.Send(mensaje);
                emisor.Dispose();
                output = "Correo electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }
            Console.WriteLine(output);
        }

    }
}