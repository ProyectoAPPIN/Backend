using com.mercaderia.bono.Entidades.ModeloEntidades;
using com.mercaderia.bono.Entidades.Utilidades;
using com.mercaderia.bono.Utilidades.Excepciones;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace com.mercaderia.bono.Notificaciones.Correo
{
    public class CorreoProxy
    {
        public ConfiguracionCorreo configuracionCorreo { get; set; }
        public CorreoProxy(ConfiguracionCorreo configuracionCorreo) 
        {
            this.configuracionCorreo = configuracionCorreo;
        }
        public CorreoProxy(string configSMTP)
        {
            this.configuracionCorreo = JsonConvert.DeserializeObject<ConfiguracionCorreo>(configSMTP);
        }
       
        /// <summary>
        ///Send an email and controls whether it runs or not with a wrapper
        /// </summary>
        /// <param name="data">object wiht data email values</param>
        /// <returns>yes or not send the email</returns>
        public void EnviarCorreo(Correo data, bool adjutaImagen=false)
        {
            try
            {
                //Validación previa
                if (data.IsNull())
                {
                    throw new ExceptionControlada("El objeto de correo ingresado se encuentra vacio");
                }
               

                // Sending mail format is validated
                if (String.IsNullOrEmpty(data.De))
                {
                    throw new ExceptionControlada("La direccion de correo del remitente no esta definida");
                }

                if (!EsDireccionValida(data.De))
                {
                    throw new ExceptionControlada("La direccion de correo del remitente tiene un formato invalido");
                }

                // The recipient's mail format is validated
                if (!data.Para.IsNull())
                {
                    foreach (var Para in data.Para)
                    {
                        if (!EsDireccionValida(Para))
                        {
                            throw new ExceptionControlada(string.Format("La direccion de correo destinatario {0} tiene un formato invalido", Para));
                        }
                    }
                }
                else
                {
                    throw new ExceptionControlada("Debe especificar una direccion de correo destinatario");
                }

                // Email format is validated copy
                if (!data.ConCopia.IsNull())
                {
                    foreach (var ConCopia in data.ConCopia)
                    {
                        if (!EsDireccionValida(ConCopia))
                        {
                            throw new ExceptionControlada("El correo debe tener un asunto");
                        }
                    }
                }
                else
                {
                    // No copy addresses
                }

                // Validates the email has a subject defined
                if (String.IsNullOrEmpty(data.Asunto))
                {
                    throw new ExceptionControlada("El correo debe tener un asunto");
                }

                // Validates the email message body has a defined
                if (String.IsNullOrEmpty(data.Cuerpo))
                {
                    throw new ExceptionControlada("El correo debe tener un cuerpo de mensaje");
                }

                // It validates that if the mail has an attachment that exists and does not exceed the defined size
                //if (!data.adjunto.isnull())
                //{
                //    // the file size must be configured in the configurator
                //    if (data.adjunto.canseek && (data.adjunto.length / 1024) > this.configuracioncorreo.tamanioarchivoadjunto)
                //    {
                //        throw new exceptioncontrolada(string.format("el archivo adjunto del correo supera el tamaño de {0} kb", this.configuracioncorreo.tamanioarchivoadjunto ));
                //    }
                //}

                // Mail object is created
                MailMessage email = new MailMessage();
                email.From = new MailAddress(data.De);

                // The recipient addresses are appended
                foreach (var Para in data.Para)
                { email.To.Add(new MailAddress(Para)); }

                // Copy the e-mail are appended
                if (!data.ConCopia.IsNull())
                {
                    foreach (var ConCopia in data.ConCopia)
                    {
                        email.CC.Add(new MailAddress(ConCopia));
                    }
                }

                email.Subject = data.Asunto;
                email.Body = data.Cuerpo;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;


                long lengthFile = 0;
                if (!data.Adjunto.IsNull())
                    lengthFile = data.Adjunto.Length; 

                // The file attached to the email
                if (!data.Adjunto.IsNull() && lengthFile > 0)
                {
                    Attachment archivoAdjunto = new Attachment(data.Adjunto, data.NombreAdjunto);
                    email.Attachments.Add(archivoAdjunto);
                    if(adjutaImagen)
                    {
                        email.Body = String.Format(data.Cuerpo, archivoAdjunto.ContentId);
                    }
                }

                // The client sending the mail is created
                SmtpClient smtp = new SmtpClient();
                smtp.Host = this.configuracionCorreo.servidorSMTP;
                smtp.Port = this.configuracionCorreo.puertoServidorSMTP;
                smtp.EnableSsl = this.configuracionCorreo.habilitaSSLServidorSMTP;
                smtp.UseDefaultCredentials = true;                
                smtp.Credentials = new System.Net.NetworkCredential(this.configuracionCorreo.usuarioServidorSMTP, this.configuracionCorreo.claveUsuarioServidorSMTP);
               
                // The mail is sent and released
                smtp.Send(email);
                email.Dispose();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

       


        /// <Summary >
        /// Validates if an email address is a valid format
        /// </Summary>     
        public bool EsDireccionValida(string direccionCorreo)
        {

                direccionCorreo = Regex.Replace(direccionCorreo, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Return true if an email address is a valid format.
                return Regex.IsMatch(direccionCorreo,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Mappear el dominio
        /// </summary>   
        private static string DomainMapper(Match match)
        {
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            domainName = idn.GetAscii(domainName);
            return string.Concat(match.Groups[1].Value, domainName);
        }

    }
}






