using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.mercaderia.bono.DAL;
using com.mercaderia.bono.Utilidades.Excepciones;

namespace com.mercaderia.bono.Negocio
{
    public abstract class NegocioAbstracto
    {
        protected ICollection<InformacionFiltro> InvocarDecodificacionFiltro(string filterBy)
        {
            ICollection<InformacionFiltro> filtros = null;
            if(!String.IsNullOrEmpty(filterBy))
            {
                filtros = DecodificarFiltro(filterBy);
            }

            return filtros;
        }

        private ICollection<InformacionFiltro> DecodificarFiltro(string filterBy)
        {
            ICollection<InformacionFiltro> filtros = new List<InformacionFiltro>();
            foreach(string condicion in DecodificarParametrosFiltro(filterBy))
            {
                TipoFiltro tipoFiltro = IdentificarTipoFiltro(condicion);
                string[] campoValor = condicion.Split(new string[] { UtilEnumeradores.IdReducidoFiltro(tipoFiltro) }, StringSplitOptions.None);

                string campo = PrimerCaracterMayuscula(campoValor[0]);
                if (!EntidadContieneAtributo(campo))
                {
                    throw new BaseException("La entidad no contiene el atributo " + campo);
                }

                string valor = campoValor[1];
                if(String.IsNullOrEmpty(valor))
                {
                    throw new BaseException("No se especificó un valor para el campo " + campo);
                }

                TipoOperador tipoOperador = IdentificarTipoOperador(filterBy, condicion);

                filtros.Add(new InformacionFiltro(campo, tipoFiltro, valor, tipoOperador));
            }

            return filtros;
        }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private ICollection<string> DecodificarParametrosFiltro(string filterBy)
        {
            ICollection<string> parametros = new List<string>();
            foreach(string temp in filterBy.Split(new string[] { UtilEnumeradores.IdReducidoOperador(TipoOperador.And) }, StringSplitOptions.None))
            {
                string[] x = temp.Split(UtilEnumeradores.IdReducidoOperador(TipoOperador.Or).ToArray());
                foreach(String elem in x)
                {
                    parametros.Add(elem);
                }
            }
            return parametros;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private TipoOperador IdentificarTipoOperador(string filterBy, string condicion)
        {
            int index = filterBy.IndexOf(condicion, StringComparison.CurrentCultureIgnoreCase);
            if (index + condicion.Length == filterBy.Length)
            {
                return TipoOperador.And;
            }
            else
            {
                return UtilEnumeradores.TipoOperadorPorId(filterBy.ElementAt(index + condicion.Length).ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private TipoFiltro IdentificarTipoFiltro(string parametro)
        {
            TipoFiltro tipoFiltro;
            if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.Diferente)))
            {
                tipoFiltro = TipoFiltro.Diferente;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.Like)))
            {
                tipoFiltro = TipoFiltro.Like;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.MenorIgual)))
            {
                tipoFiltro = TipoFiltro.MenorIgual;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.MayorIgual)))
            {
                tipoFiltro = TipoFiltro.MayorIgual;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.Mayor)))
            {
                tipoFiltro = TipoFiltro.Mayor;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.Menor)))
            {
                tipoFiltro = TipoFiltro.Menor;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.NotLike)))
            {
                tipoFiltro = TipoFiltro.NotLike;
            }
            else if (parametro.Contains(UtilEnumeradores.IdReducidoFiltro(TipoFiltro.Exacto)))
            {
                tipoFiltro = TipoFiltro.Exacto;
            }
            else
            {
                throw new BaseException("Operador de comparaci�n no identificado en filtro: " + parametro);
            }
            return tipoFiltro;
        }

        protected ICollection<InformacionOrdenamiento> InvocarDecodificacionOrdenamiento(string orderBy)
        {
            ICollection<InformacionOrdenamiento> ordenamiento = null;
            if(!String.IsNullOrEmpty(orderBy))
            {
                ordenamiento = DecodificarOrdenamiento(orderBy);
            }
            return ordenamiento;
        }

        private ICollection<InformacionOrdenamiento> DecodificarOrdenamiento(string orderBy)
        {
            ICollection<InformacionOrdenamiento> ordenamiento = new List<InformacionOrdenamiento>();

            foreach(string orden in DecodificarParametrosOrdedBy(orderBy))
            {
                string[] campoDireccion = orden.Split(UtilConstantes.SeparadorOrderBy.ToCharArray());
                string campo = PrimerCaracterMayuscula(campoDireccion[0]);
                if (!EntidadContieneAtributo(campo))
                {
                    throw new BaseException("La entidad no contiene el atributo " + campo);
                }

                TipoOrdenamiento tipoOrdenamiento;
                if(campoDireccion.Length == 2)
                {
                    tipoOrdenamiento = UtilEnumeradores.TipoOrdenamientoPorId(campoDireccion[1]);
                }
                else if(campoDireccion.Length == 1)
                {
                    tipoOrdenamiento = TipoOrdenamiento.Ascendente;
                }
                else
                {
                    throw new BaseException("El parametro especificado para el campo: " + campo + ", tiene un error de sintaxis.");
                }
                ordenamiento.Add(new InformacionOrdenamiento(tipoOrdenamiento, campo));
            }
            return ordenamiento;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private ICollection<String> DecodificarParametrosOrdedBy(String orderBy)
        {

            return orderBy.Split(UtilConstantes.SeparadorParametrosConsulta.ToCharArray()).ToList();
        }
        
        public static string PrimerCaracterMayuscula(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentNullException("Cadena indefinida para PrimerCaracterMayuscula");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        protected abstract bool EntidadContieneAtributo(string nombreAtributo);


    }
}