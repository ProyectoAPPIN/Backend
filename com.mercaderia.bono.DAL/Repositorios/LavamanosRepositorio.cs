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
    public sealed partial class LavamanosRepositorio : Repositorio<Lavamanos>
    {
        public LavamanosRepositorio(BilleteraEntities context) : base(context)
        {

        }
        public Lavamanos ObtenerPorID(int codLavamanos, string codInstitucion)
        {
            return dbSet.Where(x => x.codLavamanos == codLavamanos && x.codInstitucion == codInstitucion ).FirstOrDefault();
        }
    }
}
