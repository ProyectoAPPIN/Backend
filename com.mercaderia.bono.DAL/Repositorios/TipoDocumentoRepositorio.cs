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
    public sealed partial class TipoDocumentoRepositorio : Repositorio<TipoDocumento>
    {
        public TipoDocumentoRepositorio(BilleteraEntities context) : base(context)
        {

        }

        /// <summary>
        /// Método que obtiene todas los tipos documentos existentes
        /// </summary>
        /// <returns></returns>
        public List<TipoDocumento> ObtenerTipoDocumento()
        { 
            return dbSet.OrderBy(t => new { t.nomenclatura, t.descripcion }).ToList<TipoDocumento>();
        }

        /// <summary>
        /// Método que obtiene los datos de un tipo de documento (por código)
        /// </summary>
        /// <param name="pCodDocumento">
        /// Código del tipo de documento
        /// </param>
        /// <returns></returns>
        public TipoDocumento ObtenerTipoDocPorCodigo(string pCodDocumento)
        {
            return dbSet.Where(x => x.nomenclatura == pCodDocumento).FirstOrDefault();
        }
    }
}
