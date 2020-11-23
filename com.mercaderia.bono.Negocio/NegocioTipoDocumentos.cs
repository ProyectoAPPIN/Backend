using com.mercaderia.bono.DAL;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using com.mercaderia.bono.Utilidades.Auditoria;
using com.mercaderia.bono.Entidades.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using com.mercaderia.bono.Utilidades.Excepciones;
using com.mercaderia.bono.Entidades.Enumeradores;
using static com.mercaderia.bono.Entidades.Enumeradores.Enums;
using com.mercaderia.bono.Notificaciones.Correo;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using com.mercaderia.bono.Entidades.Dto;

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioTipoDocumentos //: NegocioAbstracto
    {
        /// <summary>
        /// Método que obtiene todos los documentos existentes
        /// </summary>
        /// <returns></returns>
        /// 
        public List<TipoDocumento> ObtenerTipoDocumentos()
        {
            List<TipoDocumento> lstTipoDoc = new List<TipoDocumento>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstTipoDoc = unitOfWork.TipoDocumentoRepositorio.ObtenerTipoDocumento();                   
                if (lstTipoDoc == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoError);
                }
                if (lstTipoDoc.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoInexistente);
                }
            }
            return lstTipoDoc;
        }

        /// <summary>
        /// Método que obtiene los datos de una ciudad específica (por código)
        /// </summary>
        /// <param name="pCodCiudad">
        /// Código de la Ciudad
        /// </param>
        /// <returns></returns>
        public TipoDocumento ObtenerCiudadPorCodigo(string pDocumento)
        {
            TipoDocumento tipodoc = new TipoDocumento();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                tipodoc = unitOfWork.TipoDocumentoRepositorio.ObtenerTipoDocPorCodigo(pDocumento);
                if (tipodoc.IsNull())
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoInexistente);
                }               
            }
            return tipodoc;
        }

        /// <summary>
        /// Método que obtiene todos roles
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Perfil> ObtenerRoles()
        {
            List<Perfil> lstperfil = new List<Perfil>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstperfil = unitOfWork.RolesRepositorio.ObtenerRoles();
                if (lstperfil == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgRolesError);
                }
                if (lstperfil.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgRolesInexistente);
                }
            }
            return lstperfil;
        }

        public string sendNotificacion()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {                
                var persona = unitOfWork.UsuarioRepositorio.ObtenerListaUsuariosActivoEmail();

                if (persona.Count > 0)
                {
                    foreach (var item in persona)
                    {
                        EnviarEmail(item.correo);
                    }
                }
                return "Notificaciones enviadas correctamente";
            }
        }

        /// <summary>
        /// Metodo para enviar correo notificacion Masivo
        /// </summary>        
        /// <returns></returns>
        private void EnviarEmail(string email)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                Dominio emaiNotificacionLavadoFormato = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.notificacionLavado.ToString());

                if (emaiNotificacionLavadoFormato.IsNull())
                    throw new ExceptionControlada(string.Format(Mensajes.MsgErrorDominioNoEncontrado, Enums.EnumTablaDominio.sms_formato.ToString()));
                var message = string.Format(emaiNotificacionLavadoFormato.valor);

                Dominio dominioSMTP = negocioDominio.ConsultarPorId(EnumTablaDominio.configuracionSMTP.ToString());
                if (dominioSMTP.IsNull() || string.IsNullOrEmpty(dominioSMTP.valor))
                    throw new ExceptionControlada(Mensajes.MsgConfiguracionSMTPRequerida);
                CorreoProxy correoProxy = new CorreoProxy(dominioSMTP.valor);
                Correo correoEnviar = new Correo();


                Dominio dominioRemitentePrincipal = negocioDominio.ConsultarPorId(EnumTablaDominio.correoRemitentePrincipal.ToString());
                if (dominioRemitentePrincipal.IsNull() || string.IsNullOrEmpty(dominioRemitentePrincipal.valor))
                    throw new ExceptionControlada(Mensajes.MsgCorreoRemitentePrincipal);

                Dominio domonioAsuntoCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.asuntoNotificacionLavado.ToString());
                if (domonioAsuntoCorreo.IsNull() || string.IsNullOrEmpty(domonioAsuntoCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgAsuntoCorreoRegistroPedido);

                Dominio plantillaCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.plantillaCorreoNotificacion.ToString());
                if (plantillaCorreo.IsNull() || string.IsNullOrEmpty(plantillaCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgTextoInicialCorreoRegistroPedido);

                var bytes = Convert.FromBase64String(Constantes.IMAGEN_BANNER_NOTI);
                var contents = new MemoryStream(bytes);

                correoEnviar.De = dominioRemitentePrincipal.valor;
                correoEnviar.Asunto = domonioAsuntoCorreo.valor;
                correoEnviar.Para = new List<string>() { email };
                correoEnviar.Cuerpo = plantillaCorreo.valor.Replace("@MESSAGE", message);
                correoEnviar.Adjunto = contents;
                correoProxy.EnviarCorreo(correoEnviar, true);
            }
            catch (Exception ex)
            {
                Logger log = Logger.Instancia;
                log.EscribirLogError($"Error al enviar Correo bono:{10}", ex);
                return;
            }
        }
                     
        public string sendNotificacionFirebase()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                //obtengo todas las personas y realizo el envio de la notificacion
                //cambiarlo para validar si realizaron el registro de entrada
                var persona = unitOfWork.UsuarioRepositorio.ObtenerListaUsuariosActivo();

                if (persona.Count > 0)
                {
                    NegocioDominio negocioDominio = new NegocioDominio();
                    Dominio notificacionPush = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.notificacionLavadoPush.ToString());

                    if (notificacionPush.IsNull())
                        throw new ExceptionControlada(string.Format(Mensajes.MsgErrorDominioNoEncontrado, Enums.EnumTablaDominio.sms_formato.ToString()));

                    var mensaje = string.Format(notificacionPush.valor);

                    //sendNotificacionFirebase("", "Hola");
                    foreach (var item in persona)
                    {
                        //Construyo el objeto para enviar la notificacion al celular
                        var data = new
                        {
                            to = item.tokenDispositivo,
                            data = new
                            {
                                name = item.nombres,
                                userId = Convert.ToString(item.codUsuario),
                                message = mensaje,
                                status = true
                            }
                        };

                        sendNotificacionFirebase(data);
                    }
                }
                return "Notificaciones enviadas correctamente";
            }
        }


        private void sendNotificacionFirebase(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            sendNotificacion(byteArray);
            
        }

        public void sendNotificacion(Byte[] byteArray)
        {
            try
            {
                string server_api_key = ConfigurationManager.AppSettings["SERVER_API_KEY"];
                string sender_id = ConfigurationManager.AppSettings["SENDER_ID"];
                string urlNotificacion = ConfigurationManager.AppSettings["baseURLNotificacion"];
                WebRequest tRequest = WebRequest.Create(urlNotificacion);
                tRequest.Method = "post";
                tRequest.ContentType = " application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", server_api_key));
                tRequest.Headers.Add(string.Format("Sender: id={0}", sender_id));

                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                Logger log = Logger.Instancia;
                log.EscribirLogError($"Error al enviar notificaciones firebase", ex);
                return;
            }
        }

        public List<ultimoLavadoDTO> obtenerUltimoLavadoManos(string codUsuario)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var ultimoLavado = unitOfWork.RegistroLavadoRepositorio.ObtenerUltimoLavado(codUsuario);
                return ultimoLavado;
            }            
        }
        public List<RecordatorioDTO> obtenerRecordatorioLavadoManos(string codUsuario)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var recordatorioLavado = unitOfWork.RegistroLavadoRepositorio.ObtenerRecordatorioLavado(codUsuario);
                return recordatorioLavado;
            }
        }
    }
}
