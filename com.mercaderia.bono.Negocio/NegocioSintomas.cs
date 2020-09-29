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
using com.mercaderia.bono.Entidades.Dto;

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioSintomas
    {
        /// <summary>
        /// Método que obtiene todos los documentos existentes
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Sintoma> ObtenerSintomas()
        {
            List<Sintoma> lstSintomas = new List<Sintoma>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstSintomas = unitOfWork.SintomasRepositorio.ObtenerSintomas();                    
                if (lstSintomas == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgSintomasError);
                }
                if (lstSintomas.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgSintomasInexistente);
                }
            }
            return lstSintomas;
        }

        public List<ResponseSintomasDTO> CargueSintomasUsuario(SintomasDTO[] sintomasUsuario, string idUsuario)
        {
           
            //Proceso para registrar sintomas de un usuario en el BD
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                List<ResponseSintomasDTO> lstSintomasUsuario = new List<ResponseSintomasDTO>();                

                lstSintomasUsuario = unitOfWork.SintomasRepositorio.AddSintomasUsuario(sintomasUsuario, idUsuario);
                return lstSintomasUsuario;
            }
        }

    }
}
