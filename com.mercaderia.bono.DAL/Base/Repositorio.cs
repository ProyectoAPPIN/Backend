using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using com.mercaderia.bono.Utilidades.Excepciones;
using System.Globalization;
using com.mercaderia.bono.Entidades.ModeloEntidades;

namespace com.mercaderia.bono.DAL
{
    public abstract partial class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class
    {
        internal BilleteraEntities context;
        internal DbSet<TEntity> dbSet;
        protected Repositorio(BilleteraEntities contextApp)
        {
            if (contextApp != null)
            {
                contextApp.Configuration.LazyLoadingEnabled = false;
                context = contextApp;
                dbSet = contextApp.Set<TEntity>();
            }
            else
            {
                throw new ArgumentNullException("contextApp");
            }

        }


        public virtual IEnumerable<TEntity> List()
        {
            IQueryable<TEntity> query = dbSet;
            return query.ToList();
        }

        public virtual void Actualizar(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

        }

        public virtual void Adicionar(TEntity entity)
        {
            dbSet.Add(entity);

        }

        //public virtual TEntity[] Consultar(ICollection<InformacionFiltro> filtros, ICollection<InformacionOrdenamiento> ordenamientos, RangoConsulta rango)
        //{
        //    IQueryable<TEntity> q = dbSet;
        //    Expression<Func<TEntity, bool>> whereExpression = ConstruirArbolFiltros(filtros);
        //    if (whereExpression != null)
        //        q = q.Where(whereExpression);
        //    //if (ordenamientos != null && ordenamientos.Count > 0)
        //    //{
        //    //    q = AgregarOrdenamiento(q, ordenamientos);
        //    //}
        //    if (rango != null)
        //    {
        //        if(!(ordenamientos != null && ordenamientos.Count > 0))
        //        q = q.OrderBy(x => 0); ;
        //        q = q.Skip(rango.From);
        //        q = q.Take(rango.ObtenerMaximoResultados());
        //        return q.ToArray();
        //    }
        //    else
        //        return q.ToArray();

        //}

        /* public virtual IQueryable<TEntity> AgregarOrdenamiento(IQueryable<TEntity> q, ICollection<InformacionOrdenamiento> ordenamientos)
         {
             if (ordenamientos != null)
             {
                 for (var i = 0; i < ordenamientos.Count; i++)
                 {
                     InformacionOrdenamiento ord = ordenamientos.ElementAt(i);
                     if (EntidadTieneAtributo(ord.Campo))
                     {
                         ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entidad");
                         Expression campo = Expression.PropertyOrField(pe, ord.Campo);
                         var exp = Expression.Lambda(campo, pe);
                         Type[] types = new Type[] { q.ElementType, exp.Body.Type };
                         switch (ord.Tipo)
                         {
                             case TipoOrdenamiento.Ascendente:
                                 var e1 = Expression.Call(typeof(Queryable), i == 0 ? "OrderBy" : "ThenBy", types, q.Expression, exp);
                                 q = q.Provider.CreateQuery<TEntity>(e1);
                                 break;
                             case TipoOrdenamiento.Descendente:
                                 var e2 = Expression.Call(typeof(Queryable), i == 0 ? "OrderByDescending" : "ThenByDescending", types, q.Expression, exp);
                                 q = q.Provider.CreateQuery<TEntity>(e2);
                                 break;
                             default:
                                 var e3 = Expression.Call(typeof(Queryable), i == 0 ? "OrderBy" : "ThenBy", types, q.Expression, exp);
                                 q = q.Provider.CreateQuery<TEntity>(e3);
                                 break;

                         }
                     }
                     else
                     {
                         throw new BaseException("No es posible ordenar por el campo: " + ord.Campo);
                     }
                 }
                 return q;
             }
             else
             {
                 throw new ArgumentNullException("ordenamientos");
             }
         }*/

        /*   public virtual Expression<Func<TEntity,bool>> ConstruirArbolFiltros(ICollection<InformacionFiltro> filtros)
           {
               Expression < Func<TEntity, bool> > exp = null;
               if( filtros != null && filtros.Count > 0)
               {
                   ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entidad");
                   Expression where = ObtenerExpresionWhere(pe, filtros);
                   if (where != null)
                   {
                       exp = Expression.Lambda<Func<TEntity, bool>>(where, new ParameterExpression[] { pe });
                   }
               }

               return exp;
           }*/

        /*  public virtual Expression ObtenerExpresionWhere(ParameterExpression pe , ICollection<InformacionFiltro> filtros)
          {
              Expression expression = null;
              TipoOperador Operador = TipoOperador.And;

              if(filtros != null)
              {
                  for (var i = 0; i < filtros.Count; i++)
                  {
                      InformacionFiltro inff = filtros.ElementAt(i);
                      if (EntidadTieneAtributo(inff.Campo))
                      {
                          Expression left = Expression.PropertyOrField(pe, inff.Campo);
                          PropertyInfo prop = ObtenerAtributoEntidad(inff.Campo);
                          Type tipo = prop.PropertyType;
                          Object value = ObtenerValorPorTipo(inff.Valor, tipo);
                          Expression right = Expression.Constant(value,tipo);
                          Expression ape;
                          MethodInfo metodo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                          switch (inff.Tipo)
                          {
                              case TipoFiltro.Exacto:
                                  ape = Expression.Equal(left, right);
                                  break;
                              case TipoFiltro.Diferente:
                                  ape = Expression.NotEqual(left, right);
                                  break;
                              case TipoFiltro.Like:
                                  ape = Expression.Call(left, metodo, right);
                                  break;
                              case TipoFiltro.Mayor:
                                  ape = Expression.GreaterThan(left, right);
                                  break;
                              case TipoFiltro.MayorIgual:
                                  ape = Expression.GreaterThanOrEqual(left, right);
                                  break;
                              case TipoFiltro.Menor:
                                  ape = Expression.LessThan(left, right);
                                  break;
                              case TipoFiltro.MenorIgual:
                                  ape = Expression.LessThanOrEqual(left, right);
                                  break;
                              case TipoFiltro.NotLike:
                                  ape = Expression.Call(left, metodo, right);
                                  ape = Expression.Not(ape);
                                  break;
                              default:
                                  throw new BaseException("No se definió un operador");
                          }

                          if (i == 0)
                          {
                              expression = ape;
                          }
                          else
                          {
                              switch (Operador)
                              {
                                  case TipoOperador.And:
                                      expression = Expression.AndAlso(expression, ape);
                                      break;
                                  case TipoOperador.Or:
                                      expression = Expression.OrElse(expression, ape);
                                      break;
                                  default:
                                      expression = Expression.AndAlso(expression, ape);
                                      break;
                              }
                          }

                          Operador = inff.Operador;
                      }
                      else
                      {
                          throw new BaseException("No es posible filtrar por el campo: " + inff.Campo);
                      }

                  }

                  return expression;
              }
              else
              {
                  throw new ArgumentNullException("filtros");
              }
          }*/

        //protected abstract bool EntidadTieneAtributo(string atributo);

        // protected abstract PropertyInfo ObtenerAtributoEntidad(string atributo);

        public virtual void Eliminar(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);

        }

        public virtual int TotalRegistros()
        {
            return dbSet.Count();
        }

        public T[] EjecutarSentenciaSQL<T>(string sentencia)
        {
            return context.Database.SqlQuery<T>(sentencia).ToArray();
        }

        public Object ObtenerValorPorTipo(Object valor, Type tipo)
        {
            if (valor != null && tipo != null)
            {
                string val = valor.ToString();
                if (tipo == typeof(bool))
                {
                    return bool.Parse(val);
                }
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return int.Parse(val, CultureInfo.InvariantCulture);
                }
                if (tipo == typeof(float))
                {
                    return float.Parse(val, CultureInfo.InvariantCulture);
                }
                if (tipo == typeof(long))
                {
                    return long.Parse(val, CultureInfo.InvariantCulture);
                }
                if (tipo == typeof(short))
                {
                    return short.Parse(val, CultureInfo.InvariantCulture);
                }
                if (tipo == typeof(decimal))
                {
                    return decimal.Parse(val, CultureInfo.InvariantCulture);
                }
                if (tipo == typeof(string))
                {
                    return val;
                }   
                if (tipo == typeof(Object))
                {
                    return valor;
                }
                else
                {
                    throw new BaseException("No es posible identificar el tipo del valor: " + valor);
                }
            }
            else
            {
                throw new ArgumentNullException("valor");
            }
        }

    }
}

