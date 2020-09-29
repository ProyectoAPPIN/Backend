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
    [RoutePrefix("servicios/historicoredencion")]
    public sealed partial class HistoricoRedencionController : ApiController
    {
    	Logger log = Logger.Instancia;
        // GET: servicios/historicoredencion
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
                NegocioHistoricoRedencion negocioHistoricoRedencion = new NegocioHistoricoRedencion();
                return Content(HttpStatusCode.OK, negocioHistoricoRedencion.Consultar(filterBy, orderBy, from.Value, to.Value));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al obtener la lista de HistoricoRedencion", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
            
        }
        
        // GET: servicios/historicoredencion
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetById(int historicoRedencionId)
        {
        	try
            {
                NegocioHistoricoRedencion negocioHistoricoRedencion = new NegocioHistoricoRedencion();
                return Content(HttpStatusCode.OK, negocioHistoricoRedencion.ConsultarPorId(historicoRedencionId));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al obtener el centro Centro", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }


        // POST: servicios/historicoredencion
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]HistoricoRedencion historicoRedencion)
        {
            try
            {
                NegocioHistoricoRedencion negocioHistoricoRedencion = new NegocioHistoricoRedencion();
                return Content(HttpStatusCode.OK, negocioHistoricoRedencion.Guardar(historicoRedencion));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al crear HistoricoRedencion", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // PUT: servicios/historicoredencion
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]HistoricoRedencion historicoRedencion)
        {
            try
            {
                NegocioHistoricoRedencion negocioHistoricoRedencion = new NegocioHistoricoRedencion();
                return Content(HttpStatusCode.OK, negocioHistoricoRedencion.Actualizar(historicoRedencion));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al actualizar HistoricoRedencion", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // DELETE: servicios/historicoredencion
        [HttpDelete]
        [Route("")]
        public IHttpActionResult Delete(int historicoRedencionId)
        {
            try
            {
                NegocioHistoricoRedencion negocioHistoricoRedencion = new NegocioHistoricoRedencion();
                return Content(HttpStatusCode.OK, negocioHistoricoRedencion.Eliminar(historicoRedencionId));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al eliminar HistoricoRedencion", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

    }
}
