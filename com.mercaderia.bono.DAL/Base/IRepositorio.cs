using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using com.mercaderia.bono.DAL;

namespace com.mercaderia.bono.DAL
{
    public partial interface IRepositorio<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> List();
        void Adicionar(TEntity entity);
        void Eliminar(TEntity entity);
        void Actualizar(TEntity entity);
        int TotalRegistros();
		//TEntity[] Consultar(ICollection<InformacionFiltro> filtros, ICollection<InformacionOrdenamiento> ordenamientos, RangoConsulta rango);       
    }
}
