using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class RespuestaIngresoDto
    {
        public string codUsuario { get; set; }
        public DateTime fecha { get; set; }
        public TimeSpan hora { get; set; }
        public string sede { get; set; }
    }
}
