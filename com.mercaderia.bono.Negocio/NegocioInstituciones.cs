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

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioInstituciones
    {
        /// <summary>
        /// Método que obtiene todos las instituciones existentes
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Institucion> ObtenerInstituciones()
        {
            List<Institucion> lstInstituciones = new List<Institucion>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstInstituciones = unitOfWork.InstitucionRepositorio.ObtenerInstitucion();
                if (lstInstituciones == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgInstitucionError);
                }
                if (lstInstituciones.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgInstitucionInexistente);
                }
            }
            return lstInstituciones;
        }

        /// <summary>
        /// Método que obtiene los datos de una ciudad específica (por código)
        /// </summary>
        /// <param name="pCodInstitucion">
        /// Código de la institucion
        /// </param>
        /// <returns></returns>
        public Institucion ObtenerInstitucionPorCodigo(string pCodInstitucion)
        {
            Institucion institucion = new Institucion();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                institucion = unitOfWork.InstitucionRepositorio.ObtenerInstitucionPorCodigo(pCodInstitucion);
                if (institucion.IsNull())
                {
                    throw new ExceptionControlada(Mensajes.MsgInstitucionInexistente);
                }
            }
            return institucion;
        }
    }
}
