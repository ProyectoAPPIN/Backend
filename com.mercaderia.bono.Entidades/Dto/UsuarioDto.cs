using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Dto
{
    public class UsuarioDto
    {
        public int codUsuario { get; set; }        
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string celular { get; set; }
        public int codPerfil { get; set; }
        public string codInstitucion { get; set; }
        public string correo { get; set; }
        public bool sexo { get; set; }
        public bool activo { get; set; }
        public string codigoActivacion { get; set; }
        public DateTime codActivacionExpira { get; set; }      
    }
}
