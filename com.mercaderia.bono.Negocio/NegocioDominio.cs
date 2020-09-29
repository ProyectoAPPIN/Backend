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
    public sealed partial class NegocioDominio
    {
        public Dominio ConsultarPorId(string dominio)
        {
            Dominio dominioEntity = new Dominio();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                dominioEntity = unitOfWork.DominioRepositorio.ObtenerPorID(dominio);
            }
            return dominioEntity;
        }
    }
}
