//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace com.mercaderia.bono.Entidades.ModeloEntidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.RegistroSintoma = new HashSet<RegistroSintoma>();
            this.RegistroIngreso = new HashSet<RegistroIngreso>();
            this.RegistroLavado = new HashSet<RegistroLavado>();
        }
    
        public int codUsuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string celular { get; set; }
        public int codPerfil { get; set; }
        public string codInstitucion { get; set; }
        public string correo { get; set; }
        public Nullable<bool> sexo { get; set; }
        public Nullable<bool> activo { get; set; }
        public string codigoActivacion { get; set; }
        public Nullable<System.DateTime> codActivacionExpira { get; set; }
        public string tokenDispositivo { get; set; }
    
        public virtual Perfil Perfil { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegistroSintoma> RegistroSintoma { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegistroIngreso> RegistroIngreso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegistroLavado> RegistroLavado { get; set; }
    }
}
