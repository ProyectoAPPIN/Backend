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
    public sealed partial class DominioRepositorio : Repositorio<Dominio>
    {
        public DominioRepositorio(BilleteraEntities context) : base(context)
        {

        }
        public Dominio ObtenerPorID(string dominio)
        {
            return dbSet.Where(x => x.nombre == dominio).FirstOrDefault();
        }
    }
}
