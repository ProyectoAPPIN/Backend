using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.mercaderia.bono.DAL
{
    public class InformacionOrdenamiento
    {
        public TipoOrdenamiento Tipo { get; set; }
        public string Campo { get; set; }

        public InformacionOrdenamiento(TipoOrdenamiento tipo, string campo)
        {
            this.Tipo = tipo;
            this.Campo = campo;
        }
    }
}