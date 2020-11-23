using com.mercaderia.bono.DAL;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using com.mercaderia.bono.Utilidades.Auditoria;
using com.mercaderia.bono.Entidades.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using com.mercaderia.bono.Utilidades.Excepciones;
using com.mercaderia.bono.Entidades.Enumeradores;
using static com.mercaderia.bono.Entidades.Enumeradores.Enums;
using com.mercaderia.bono.Notificaciones.Correo;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using com.mercaderia.bono.Entidades.Dto;

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioEventos
    {
        /// <summary>
        /// Método que registra el ingreso del usuario a la universidad
        /// </summary>
        /// <returns></returns>
        /// 
        public List<RespuestaIngresoDto> registrarIngreso(IngresoDto IngresoDto)
        {
            List<RespuestaIngresoDto> respuesta = new List<RespuestaIngresoDto>();
            int error = 0;
            if (string.IsNullOrEmpty(IngresoDto.codUsuario))
            {
                respuesta.Add(new RespuestaIngresoDto()
                {
                    codUsuario = Mensajes.codErrorUsuarioVacio,                    
                    sede = "-1"
                });
                error = 1;
            }
            
            if (error == 0)
            {
                NegocioUsuario usuario = new NegocioUsuario();
                Usuario usuarioEncontrado = usuario.ConsultarPorId(Convert.ToInt32(IngresoDto.codUsuario));                
                // pasa todas las validaciones realizo la activacion del usuario           
                if (error == 0)
                {
                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        RegistroIngreso nuevoIngreso = null;
                        usuarioEncontrado.codUsuario = usuarioEncontrado.codUsuario;
                        string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        string hora = DateTime.Now.ToString("HH:mm:ss");

                        int[] partes = hora.Split(new char[] { ':' }).Select(x => Convert.ToInt32(x)).ToArray();
                        TimeSpan tiempo = new TimeSpan(partes[0], partes[1], partes[2]);

                        nuevoIngreso = new RegistroIngreso()
                        {                             
                             codUsuario = usuarioEncontrado.codUsuario,
                             codInstitucion = usuarioEncontrado.codInstitucion,
                             fecha = Convert.ToDateTime(fecha), 
                             hora = tiempo,
                             temperatura = IngresoDto.temperatura,
                             oxigenacion = IngresoDto.oxigenacion     
                        };

                        unitOfWork.EventosRepositorio.Adicionar(nuevoIngreso);
                        unitOfWork.Save();                     

                        respuesta.Add(new RespuestaIngresoDto()
                        {
                            codUsuario = Convert.ToString(usuarioEncontrado.codUsuario),
                            fecha = Convert.ToDateTime(fecha),
                            hora = tiempo,
                            sede = nuevoIngreso.codInstitucion
                        });
                    }
                }
            }

            return respuesta;
        }
    }
}
