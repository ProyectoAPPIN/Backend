using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.mercaderia.bono.DAL
{
    public enum TipoFiltro
    {
        Like,
        Exacto, 
        NotLike,
        Mayor,
        Menor,
        MayorIgual, 
        MenorIgual,
        Diferente
    };
}