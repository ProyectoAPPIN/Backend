using com.mercaderia.bono.Entidades;
using com.mercaderia.bono.Negocio;
using com.mercaderia.bono.Utilidades.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace com.mercaderia.bono.Rest
{
    [RoutePrefix("servicios/logmovimientos")]
    public sealed partial class LogMovimientosController : ApiController
    {
    	Logger log = Logger.Instancia;
        // GET: servicios/logmovimientos
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(string filterBy, string orderBy, int? from, int? to)
        {
            try
            {
                if (!from.HasValue) from = -1;
                if (!to.HasValue) to = -1;
                if (string.IsNullOrEmpty(filterBy)) filterBy = string.Empty;
                if (string.IsNullOrEmpty(orderBy)) orderBy = string.Empty;
                NegocioLogMovimientos negocioLogMovimientos = new NegocioLogMovimientos();
                return Content(HttpStatusCode.OK, negocioLogMovimientos.Consultar(filterBy, orderBy, from.Value, to.Value));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al obtener la lista de LogMovimientos", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
            
        }
        
        // GET: servicios/logmovimientos
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetById(int movimientoId)
        {
        	try
            {
                NegocioLogMovimientos negocioLogMovimientos = new NegocioLogMovimientos();
                return Content(HttpStatusCode.OK, negocioLogMovimientos.ConsultarPorId(movimientoId));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al obtener el centro Centro", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }


        // POST: servicios/logmovimientos
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]LogMovimientos logMovimientos)
        {
            try
            {
                NegocioLogMovimientos negocioLogMovimientos = new NegocioLogMovimientos();
                return Content(HttpStatusCode.OK, negocioLogMovimientos.Guardar(logMovimientos));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al crear LogMovimientos", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // PUT: servicios/logmovimientos
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]LogMovimientos logMovimientos)
        {
            try
            {
                NegocioLogMovimientos negocioLogMovimientos = new NegocioLogMovimientos();
                return Content(HttpStatusCode.OK, negocioLogMovimientos.Actualizar(logMovimientos));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al actualizar LogMovimientos", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // DELETE: servicios/logmovimientos
        [HttpDelete]
        [Route("")]
        public IHttpActionResult Delete(int movimientoId)
        {
            try
            {
                NegocioLogMovimientos negocioLogMovimientos = new NegocioLogMovimientos();
                return Content(HttpStatusCode.OK, negocioLogMovimientos.Eliminar(movimientoId));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al eliminar LogMovimientos", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

    }
}
