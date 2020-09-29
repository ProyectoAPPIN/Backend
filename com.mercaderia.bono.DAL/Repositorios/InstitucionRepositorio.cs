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
    public sealed partial class InstitucionRepositorio : Repositorio<Institucion>
    {
        public InstitucionRepositorio(BilleteraEntities context) : base(context)
        {

        }

        /// <summary>
        /// Método que obtiene todas las instituciones/existentes
        /// </summary>
        /// <returns></returns>
        public List<Institucion> ObtenerInstitucion()
        {
            return dbSet.OrderBy(t => new { t.codInstitucion, t.nombre }).ToList<Institucion>();
        }

        /// <summary>
        /// Método que obtiene una institucion específica (por código)
        /// </summary>
        /// <param name="pCodInstitucion">
        /// Código de la institucion
        /// </param>
        /// <returns></returns>
        public Institucion ObtenerInstitucionPorCodigo(string pCodInstitucion)
        {
            return dbSet.Where(x => x.codInstitucion == pCodInstitucion).FirstOrDefault();
        }
    }
}
