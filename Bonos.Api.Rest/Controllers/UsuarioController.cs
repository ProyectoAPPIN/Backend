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
using com.mercaderia.bono.Entidades.Dto;
using System.Web.Http.Cors;

namespace Bonos.Api.Rest.Controllers
{

    [RoutePrefix("api/Usuario")]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Headers")]
    public class UsuarioController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// Método que registra el usuario en la aplicacion
        /// </summary>
        /// <returns></returns>       
        [HttpPost]
        [Route("Guardar")]
        public IHttpActionResult Post([FromBody]UsuarioDto usuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                Usuario usuarioCreado = negocioUsuario.CrearUsuario(usuario);

                List<UsuarioActivoDto> usuarioResultante = new List<UsuarioActivoDto>();

                usuarioResultante.Add(new UsuarioActivoDto
                {
                    codUsuario = Convert.ToString(usuarioCreado.codUsuario),
                    codigoActivacion = usuarioCreado.codigoActivacion,
                    activo = usuarioCreado.activo
                });

                return Content(HttpStatusCode.OK, usuarioResultante);
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al crear Usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        /// <summary>
        /// Servicio para activar el usuario       
        /// </summary>
        /// <returns></returns>   
        [HttpPost]
        [Route("ActivacionUsuario")]
        public IHttpActionResult ReportTransactionBonoFromPOS([FromBody]UsuarioActivoDto usuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                List<respuestaActivacionDto> activacionResultante = negocioUsuario.ActivacionUsuario(usuario);                

                return Content(HttpStatusCode.OK, activacionResultante);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(ex.Message, ex);
                return Content(HttpStatusCode.NotFound, new ApiException(HttpStatusCode.NotFound, ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al activar usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        /// <summary>
        /// Servicio para dar acceso a la aplicacion se valida si el tipo documento y numero de documento e institucion existen
        /// Se verifica si tiene un registro de ingreso
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("ValidaUsuario")]
        public HttpResponseMessage GetValidaUsuario(string tipoDocumento, string documento, string universidad)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                List<AccesoUsuarioDto> lstTipoDocResultado = negocioUsuario.ValidaUsuario(tipoDocumento, documento, universidad);
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
        /// Servicio para reenviar codigo activacion       
        /// </summary>
        /// <returns></returns>   
        [HttpPost]
        [Route("ReenviarCodigoActivacion")]
        public IHttpActionResult ReenviarCodActivacion([FromBody]UsuarioActivoDto usuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();

                List<UsuarioActivoDto> CodigoResultante = negocioUsuario.ReeviarCodActivacion(usuario);
                return Content(HttpStatusCode.OK, CodigoResultante);
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al reeenviar el codigo de activación", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }
        /// <summary>
        /// Método que consulta informacion de un usuario si ya existe en la base de datos para cargar informacion existente
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPrecargueUsuario")]
        public HttpResponseMessage GetUsuario(string tipoDocumento, string documento)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();

                List<UsuarioDto> usuarioResultado = negocioUsuario.UsuarioPreCargue(tipoDocumento, documento);
                if (usuarioResultado == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgUsuarioNoExistente);
                }
                return Request.CreateResponse(HttpStatusCode.OK, usuarioResultado);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgUsuarioError, ex);
                return Request.CreateResponse(HttpStatusCode.Conflict, new ApiException(HttpStatusCode.Conflict,  ex.Message, ex));
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
