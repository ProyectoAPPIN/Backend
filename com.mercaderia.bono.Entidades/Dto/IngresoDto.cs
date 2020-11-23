using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class IngresoDto
    {
        public string codUsuario { get; set; }
        public string CodInstitucion { get; set; }        
        public string temperatura { get; set; }
        public string oxigenacion { get; set; }
        public string codQR { get; set; }
    }
}
