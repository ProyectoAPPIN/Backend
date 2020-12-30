using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class ListaSintomasDto
    {
        public int codSintoma { get; set; }
        public string descripcion { get; set; }
        public int? opcion { get; set; }
        public bool estado { get; set; }
    }
}
