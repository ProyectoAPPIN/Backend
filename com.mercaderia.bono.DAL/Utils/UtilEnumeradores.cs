using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.mercaderia.bono.Utilidades.Excepciones;

namespace com.mercaderia.bono.DAL
{
    public sealed class UtilEnumeradores
    {
        private UtilEnumeradores() { }
        
        public static string IdReducidoOperador(TipoOperador op)
        {
            if (op == TipoOperador.And)
            {
                return "&";
            }
            else
            {
                return "|";
            }
        }

        public static TipoOperador TipoOperadorPorId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                if (id.Equals("&"))
                {
                    return TipoOperador.And;
                }
                else if (id.Equals("|"))
                {
                    return TipoOperador.Or;
                }
                else
                {
                    throw new BaseException("El tipo de operador no existe");
                }
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public static string IdReducidoOrdenamiento(TipoOrdenamiento ord)
        {
            if(ord == TipoOrdenamiento.Ascendente)
            {
                return "ASC";
            }
            else if(ord == TipoOrdenamiento.Descendente)
            {
                return "DESC";
            }
            else
            {
                return "SIN";
            }
        }

        public static TipoOrdenamiento TipoOrdenamientoPorId(string id)
        {
            if (id != null)
            {
                if (id.Equals("ASC"))
                {
                    return TipoOrdenamiento.Ascendente;
                }
                else if (id.Equals("DESC"))
                {
                    return TipoOrdenamiento.Descendente;
                }
                else if (id.Equals("SIN"))
                {
                    return TipoOrdenamiento.SinOrdenar;
                }
                else
                {
                    throw new BaseException("El tipo de ordenamiento no existe");
                }
            }
            else
            {
                throw new BaseException("El ordenamiento es null");
            }
        }

        public static string IdReducidoFiltro(TipoFiltro fil)
        {
            if (fil == TipoFiltro.Diferente)
            {
                return "!=";
            }
            else if (fil == TipoFiltro.Exacto)
            {
                return "=";
            }
            else if (fil == TipoFiltro.Like)
            {
                return ":LIKE:";
            }
            else if (fil == TipoFiltro.Mayor)
            {
                return ">";
            }
            else if (fil == TipoFiltro.MayorIgual)
            {
                return ">=";
            }
            else if (fil == TipoFiltro.Menor)
            {
                return "<";
            }
            else if (fil == TipoFiltro.MenorIgual)
            {
                return "<=";
            }
            else if (fil == TipoFiltro.NotLike)
            {
                return ":NOTLIKE";
            }
            else
            {
                return "=";
            }
        }

        public static TipoFiltro TipoFiltroPorId(string id)
        {
            if (id != null)
            {
                if (id.Equals("!="))
                {
                    return TipoFiltro.Diferente;
                }
                else if (id.Equals("="))
                {
                    return TipoFiltro.Exacto;
                }
                else if (id.Equals(":LIKE:"))
                {
                    return TipoFiltro.Like;
                }
                else if (id.Equals(">"))
                {
                    return TipoFiltro.Mayor;
                }
                else if (id.Equals(">="))
                {
                    return TipoFiltro.MayorIgual;
                }
                else if (id.Equals("<"))
                {
                    return TipoFiltro.Menor;
                }
                else if (id.Equals("<="))
                {
                    return TipoFiltro.MenorIgual;
                }
                else if (id.Equals(":NOTLIKE:"))
                {
                    return TipoFiltro.NotLike;
                }
                else
                {
                    throw new BaseException("El tipo de filtro no existe");
                }
            }
            else
            {
                throw new BaseException("El ordenamiento es null");
            }
        }

    }
}