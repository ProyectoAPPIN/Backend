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

namespace Bonos.Api.Rest.Controllers
{
    [RoutePrefix("api/Comunes")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DominioController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// Método que devuelve los tipos de documento registrados en la base de datos
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
        /// Método que devuelve los roles registrados en la base de datos
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
    }
}
