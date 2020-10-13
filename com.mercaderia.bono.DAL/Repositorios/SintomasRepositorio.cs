using com.mercaderia.bono.Entidades.Dto;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.DAL
{
    public sealed partial class SintomasRepositorio : Repositorio<Sintoma>
    {
        public SintomasRepositorio(BilleteraEntities context) : base(context)
        {

        }
        /// <summary>
        /// Método que obtiene todas las instituciones/existentes
        /// </summary>
        /// <returns></returns>
        public List<Sintoma> ObtenerSintomas()
        {
            return dbSet.OrderBy(t => new { t.codSintoma, t.descripcion, t.opcion }).ToList<Sintoma>();
        }

        public List<ResponseSintomasDTO> AddSintomasUsuario(SintomasDTO[] sintomasUsuario, string idUsuario)
        {
            List<ResponseSintomasDTO> LstSintomaUsuario = new List<ResponseSintomasDTO>();

            using (UnitOfWork unitOfWork = new UnitOfWork())
            { 
                var usuarioExistente = unitOfWork.UsuarioRepositorio.ObtenerPorID(Convert.ToInt32(idUsuario));

                if (usuarioExistente != null)
                {
                    //recorre el likstado de sintomas enviados para registralos en la bd
                    foreach (var sintoma in sintomasUsuario)
                    {
                        RegistroSintoma sintomaUsuario = null;
                        sintomaUsuario = new RegistroSintoma()
                        {
                            codSintoma = Convert.ToInt32(sintoma.codigo),
                            codUsuario = usuarioExistente.codUsuario,
                            activo = sintoma.estado,
                            fecha = DateTime.Now
                        };
                        unitOfWork.RegistroSintomasRepositorio.Adicionar(sintomaUsuario);
                        unitOfWork.Save();
                    }

                    LstSintomaUsuario.Add(new ResponseSintomasDTO
                    {
                        cod = "1",
                        mensaje = "Se registraron los sintomas correctamente"
                    });
                }else
                {
                    LstSintomaUsuario.Add(new ResponseSintomasDTO
                    {
                        cod = "-1",
                        mensaje = "El usuario no esta registrado"
                    });
                }              
                return LstSintomaUsuario;
            }
        }
    }
}
