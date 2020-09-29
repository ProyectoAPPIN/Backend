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

namespace Bonos.Api.Rest.Controllers
{
    [RoutePrefix("api/Institucion")]
    public class InstitucionController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// Método que devuelve las instituciones registrados en la base de datos
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetInstituciones")]
        public HttpResponseMessage GetInstituciones()
        {
            try
            {
                NegocioInstituciones negocioInstitucion = new NegocioInstituciones();

                List<Institucion> lstTipoDocResultado = negocioInstitucion.ObtenerInstituciones();
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
    }
}
