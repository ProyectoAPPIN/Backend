using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class ultimoLavadoDTO
    {
        public int codUsuario { get; set; }
        public DateTime? fecha { get; set; }
        public TimeSpan? hora { get; set; }
    }
}
