using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.mercaderia.bono.DAL
{
    public class InformacionFiltro
    {
        public String Campo { get; set;}
	    public TipoFiltro Tipo { get; set;}
        public Object Valor { get; set;}
	    public TipoOperador Operador { get; set;}

        public InformacionFiltro(String campo, TipoFiltro tipo, Object valor, TipoOperador operador)
        {
            this.Campo = campo;
            this.Tipo = tipo;
            this.Valor = valor;
            this.Operador = operador;
        }
    }
}