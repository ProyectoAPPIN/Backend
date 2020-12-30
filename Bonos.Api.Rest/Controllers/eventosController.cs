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

namespace Bonos.Api.Rest.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Headers")]
    [RoutePrefix("api/Eventos")]
    public class eventosController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// Servicio para registrar el ingreso a la sede       
        /// </summary>
        /// <returns></returns>   
        [HttpPost]
        [Route("registrarIngreso")]
        public IHttpActionResult RegistrarIngreso([FromBody]IngresoDto ingreso)
        {
            try
            {
                NegocioEventos negocioEvento = new NegocioEventos();
                List<RespuestaIngresoDto> ingresoResultante = negocioEvento.registrarIngreso(ingreso);                

                return Content(HttpStatusCode.OK, ingresoResultante);
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
        /// Servicio para registrar el ingreso a la sede       
        /// </summary>
        /// <returns></returns>   
        [HttpPost]
        [Route("registrarIngresoLavado")]
        public IHttpActionResult RegistrarLavado([FromBody]IngresoLavadoManosDto ingresoLavado)
        {
            try
            {
                NegocioEventos negocioEvento = new NegocioEventos();
                List<RespuestaIngresoLavadoDto> ingresoResultante = negocioEvento.registrarIngresoLavado(ingresoLavado);

                return Content(HttpStatusCode.OK, ingresoResultante);
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
    }
}
