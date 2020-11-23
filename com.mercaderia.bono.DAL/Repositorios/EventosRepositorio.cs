using com.mercaderia.bono.Entidades.ModeloEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.DAL.Repositorios
{
    public sealed partial class EventosRepositorio : Repositorio<RegistroIngreso>
    {
        public EventosRepositorio(BilleteraEntities context) : base(context)
        {

        }        
    }
}
