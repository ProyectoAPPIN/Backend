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
    [RoutePrefix("servicios/persona")]
    public sealed partial class PersonaController : ApiController
    {
    	Logger log = Logger.Instancia;
        // GET: servicios/persona
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
                NegocioPersona negocioPersona = new NegocioPersona();
                return Content(HttpStatusCode.OK, negocioPersona.Consultar(filterBy, orderBy, from.Value, to.Value));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al obtener la lista de Persona", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
            
        }
        
        // GET: servicios/persona
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetById(string tipoIdentificacion, string identificacion)
        {
        	try
            {
                NegocioPersona negocioPersona = new NegocioPersona();
                return Content(HttpStatusCode.OK, negocioPersona.ConsultarPorId(tipoIdentificacion, identificacion));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al obtener el centro Centro", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }


        // POST: servicios/persona
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Persona persona)
        {
            try
            {
                NegocioPersona negocioPersona = new NegocioPersona();
                return Content(HttpStatusCode.OK, negocioPersona.Guardar(persona));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al crear Persona", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // PUT: servicios/persona
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]Persona persona)
        {
            try
            {
                NegocioPersona negocioPersona = new NegocioPersona();
                return Content(HttpStatusCode.OK, negocioPersona.Actualizar(persona));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al actualizar Persona", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // DELETE: servicios/persona
        [HttpDelete]
        [Route("")]
        public IHttpActionResult Delete(string tipoIdentificacion, string identificacion)
        {
            try
            {
                NegocioPersona negocioPersona = new NegocioPersona();
                return Content(HttpStatusCode.OK, negocioPersona.Eliminar(tipoIdentificacion, identificacion));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al eliminar Persona", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

    }
}
