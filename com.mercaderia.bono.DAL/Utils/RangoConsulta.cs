using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.mercaderia.bono.DAL
{
    public class RangoConsulta
    {
        public int From { get; set; }
        public int To { get; set; }

        public RangoConsulta(int from, int to)
        {
            this.From = from - 1;
            this.To = to;
        }

        public int ObtenerMaximoResultados()
        {
            return To - From + 1;
        }
    }
}