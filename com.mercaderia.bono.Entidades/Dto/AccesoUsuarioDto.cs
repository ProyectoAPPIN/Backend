using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class AccesoUsuarioDto
    {
        public int codUsuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public bool? activo { get; set; }
        public string ingresoActivo { get; set; }
    }
}
