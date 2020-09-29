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
    [RoutePrefix("servicios/usuario")]
    public sealed partial class UsuarioController : ApiController
    {
    	Logger log = Logger.Instancia;
        // GET: servicios/usuario
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
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                return Content(HttpStatusCode.OK, negocioUsuario.Consultar(filterBy, orderBy, from.Value, to.Value));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al obtener la lista de Usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
            
        }
        
        // GET: servicios/usuario
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetById(string uIDUsuario)
        {
        	try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                return Content(HttpStatusCode.OK, negocioUsuario.ConsultarPorId(uIDUsuario));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al obtener el centro Centro", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }


        // POST: servicios/usuario
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Usuario usuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                return Content(HttpStatusCode.OK, negocioUsuario.Guardar(usuario));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al crear Usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // PUT: servicios/usuario
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]Usuario usuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                return Content(HttpStatusCode.OK, negocioUsuario.Actualizar(usuario));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al actualizar Usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

        // DELETE: servicios/usuario
        [HttpDelete]
        [Route("")]
        public IHttpActionResult Delete(string uIDUsuario)
        {
            try
            {
                NegocioUsuario negocioUsuario = new NegocioUsuario();
                return Content(HttpStatusCode.OK, negocioUsuario.Eliminar(uIDUsuario));
            }
            catch (Exception ex)
            {
            	log.EscribirLogError("Error al eliminar Usuario", ex);
                return Content(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }

    }
}
