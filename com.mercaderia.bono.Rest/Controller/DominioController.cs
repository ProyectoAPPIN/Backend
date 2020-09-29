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
    [RoutePrefix("servicios/dominio")]
    public sealed partial class DominioController : ApiController
    {
    	Logger log = Logger.Instancia;
        // GET: servicios/dominio
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
                NegocioDominio negocioDominio = new NegocioDominio();
                return Content(HttpStatusCode.OK, negocioDominio.Consultar(filterBy, orderBy, from.Value, to.Value));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al obtener la lista de Dominio", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
            
        }
        
        // GET: servicios/dominio
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetById(string dominio, string valor)
        {
        	try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                return Content(HttpStatusCode.OK, negocioDominio.ConsultarPorId(dominio, valor));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al obtener el centro Centro", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }


        // POST: servicios/dominio
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Dominio dominio)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                return Content(HttpStatusCode.OK, negocioDominio.Guardar(dominio));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al crear Dominio", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // PUT: servicios/dominio
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]Dominio dominio)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                return Content(HttpStatusCode.OK, negocioDominio.Actualizar(dominio));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al actualizar Dominio", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // DELETE: servicios/dominio
        [HttpDelete]
        [Route("")]
        public IHttpActionResult Delete(string dominio, string valor)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                return Content(HttpStatusCode.OK, negocioDominio.Eliminar(dominio, valor));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al eliminar Dominio", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

    }
}
