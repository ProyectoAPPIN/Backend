using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class RecordatorioDTO
    {
        public int codRegistro { get; set; }
        public int codUsuario { get; set; }
        public DateTime? fecha { get; set; }
        public TimeSpan? hora { get; set; }
        public DateTime? fechaCierre { get; set; }
        public TimeSpan? horaCierre { get; set; }
    }
}
