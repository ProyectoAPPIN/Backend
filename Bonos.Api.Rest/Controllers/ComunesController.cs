using Bonos.Api.Rest.Helper;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using com.mercaderia.bono.Negocio;
using com.mercaderia.bono.Utilidades.Auditoria;
using com.mercaderia.bono.Utilidades.Excepciones;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Cors;
using com.mercaderia.bono.Entidades.Dto;
using System.Linq;

namespace Bonos.Api.Rest.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Headers")]
    [RoutePrefix("api/Comunes")]
     public class DominioController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// M�todo que devuelve los tipos de documento registrados en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTipoDocumentos")]
        public HttpResponseMessage GetTipoDocumentos()
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                List<TipoDocumento> lstTipoDocResultado = negocioComun.ObtenerTipoDocumentos();
                if (lstTipoDocResultado.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgTipoDocumentoInexistente);
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstTipoDocResultado);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgTipoDocumentoError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,
                    ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError(Mensajes.MsgErrorNoEspacificado, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ApiException(HttpStatusCode.InternalServerError, Mensajes.MsgErrorNoEspacificado, ex));
            }
        }
        /// <summary>
        /// M�todo que devuelve los roles registrados en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoles")]
        public HttpResponseMessage GetRoles()
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                List<Perfil> lstperfil = negocioComun.ObtenerRoles();
                if (lstperfil.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgRolesInexistente);
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstperfil);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgTipoDocumentoError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,
                    ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError(Mensajes.MsgErrorNoEspacificado, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ApiException(HttpStatusCode.InternalServerError, Mensajes.MsgErrorNoEspacificado, ex));
            }
        }
        /// <summary>
        /// Metodo para enviar notificaciones masivas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSendNotificaciones")]
        public HttpResponseMessage GetSendNotificaciones()
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                string retorno = negocioComun.sendNotificacion();
                if (retorno == "")
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgNotificacionesError);
                }

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
                
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgTipoDocumentoError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,
                    ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError(Mensajes.MsgErrorNoEspacificado, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ApiException(HttpStatusCode.InternalServerError, Mensajes.MsgErrorNoEspacificado, ex));
            }
        }
        /// <summary>
        /// Metodo para enviar notificaciones masivas firebase
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSendNotificacionFirebase")]
        public IHttpActionResult GetSendNotificacionFirebase()
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                string retorno = negocioComun.sendNotificacionFirebase();
                return Content(HttpStatusCode.OK, retorno);
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al actualizar Bono", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }            
        }

        /// <summary>
        /// M�todo que devuelve los tipos de documento registrados en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUltimoLavadoManos")]
        public HttpResponseMessage GetUltimoLavadoManos(string codUsuario)
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                List<ultimoLavadoDTO> lstUltimoLavado = negocioComun.obtenerUltimoLavadoManos(codUsuario);
                if (lstUltimoLavado.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgUltimoLavadoInexistente);
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstUltimoLavado);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgTipoDocumentoError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,
                    ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError(Mensajes.MsgErrorNoEspacificado, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ApiException(HttpStatusCode.InternalServerError, Mensajes.MsgErrorNoEspacificado, ex));
            }
        }

        /// <summary>
        /// M�todo que devuelve recordatorio de lavado de manos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRecordatorioLavadoManos")]
        public HttpResponseMessage GetRecordatoriosLavadoManos(string codUsuario)
        {
            try
            {
                NegocioTipoDocumentos negocioComun = new NegocioTipoDocumentos();
                List<RecordatorioDTO> lstRecordatorioLavado = new List<RecordatorioDTO>();
                lstRecordatorioLavado = negocioComun.obtenerRecordatorioLavadoManos(codUsuario);
                //if (lstRecordatorioLavado.Count == 0)
                //{
                 
                //    string hora = DateTime.Now.ToString("HH:mm:ss");
                //    int[] partes = hora.Split(new char[] { ':' }).Select(x => Convert.ToInt32(x)).ToArray();

                //    TimeSpan tiempo = new TimeSpan(partes[0], partes[1], partes[2]);
                //    lstRecordatorioLavado.Add(new RecordatorioDTO()
                //    {
                //        codRegistro = 1,
                //        codUsuario = 6723,
                //        fecha = DateTime.Now,
                //        hora = tiempo,
                //        fechaCierre = null,
                //        horaCierre = null
                //    });
                      
                //    return Request.CreateResponse(HttpStatusCode.NotFound, lstRecordatorioLavado);
                //}
                return Request.CreateResponse(HttpStatusCode.OK, lstRecordatorioLavado);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgTipoDocumentoError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,
                    ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError(Mensajes.MsgErrorNoEspacificado, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ApiException(HttpStatusCode.InternalServerError, Mensajes.MsgErrorNoEspacificado, ex));
            }
        }
    }
}
