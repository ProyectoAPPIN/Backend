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
    public sealed partial class RolesRepositorio : Repositorio<Perfil>
    {
        public RolesRepositorio(BilleteraEntities context) : base(context)
        {
        }
        /// <summary>
        /// Método que obtiene todas los roles existentes
        /// </summary>
        /// <returns></returns>
        public List<Perfil> ObtenerRoles()
        {
            return dbSet.OrderBy(t => new { t.codPerfil, t.nombre, t.activo }).ToList<Perfil>();
        }
    }
}
