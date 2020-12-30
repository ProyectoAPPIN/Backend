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
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Headers")]
    [RoutePrefix("api/Sintomas")]    
    public class SintomasController : ApiController
    {
        Logger log = Logger.Instancia;
        /// <summary>
        /// Método que devuelve los sintomas registrados en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSintomas")]
        public HttpResponseMessage GetSintomas()
        {
            try
            {
                NegocioSintomas negocioSintomas = new NegocioSintomas();
                List<ListaSintomasDto> listaSintomasResultado = new List<ListaSintomasDto>();
                List<Sintoma> lstSintomasResultado = negocioSintomas.ObtenerSintomas();

                foreach(var sintoma in lstSintomasResultado)
                {
                    ListaSintomasDto item = new ListaSintomasDto()
                    {
                        codSintoma = sintoma.codSintoma,
                        descripcion = sintoma.descripcion,
                        opcion = sintoma.opcion,
                        estado = false
                    };
                    listaSintomasResultado.Add(item);
                }

                if (lstSintomasResultado.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Mensajes.MsgSintomasInexistente);
                }
                return Request.CreateResponse(HttpStatusCode.OK, listaSintomasResultado);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(Mensajes.MsgSintomasError, ex);
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
        /// Método que registra los sintomas de un usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("loadUsuarioSintomas")]
        public HttpResponseMessage CargueMasivoGiftCards([FromBody]SintomasDTO[] sintomasUsuario)
        {
            try
            {
                NegocioSintomas negocioSintomas = new NegocioSintomas();

                var lista = negocioSintomas.CargueSintomasUsuario(sintomasUsuario);
                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (ExceptionControlada ex)
            {
                log.EscribirLogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, new ApiException(HttpStatusCode.NotFound, ex.Message, ex));
            }
            catch (Exception ex)
            {
                log.EscribirLogError("Error al cargar datos de sintomas", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Mensajes.DescFallo);
            }
        }
        
    }
}
