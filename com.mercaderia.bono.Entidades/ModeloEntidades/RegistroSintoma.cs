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
    
    public partial class RegistroSintoma
    {
        public int codRegistro { get; set; }
        public int codSintoma { get; set; }
        public System.DateTime fecha { get; set; }
        public Nullable<bool> activo { get; set; }
        public int codUsuario { get; set; }
    
        public virtual Sintoma Sintoma { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
