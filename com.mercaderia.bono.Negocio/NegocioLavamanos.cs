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
    public sealed partial class NegocioLavamanos
    {
        public Lavamanos ConsultarPorId(int codLavamanos, string codInstitucion)
        {
            Lavamanos lavamanosEntity = new Lavamanos();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lavamanosEntity = unitOfWork.LavamanosRepositorio.ObtenerPorID(codLavamanos, codInstitucion);
            }
            return lavamanosEntity;
        }
    }
}
