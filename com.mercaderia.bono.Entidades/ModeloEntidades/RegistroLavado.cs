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
    
    public partial class RegistroLavado
    {
        public int codRegistro { get; set; }
        public int codUsuario { get; set; }
        public int codLavamanos { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<System.TimeSpan> hora_registro { get; set; }
    
        public virtual Lavamanos Lavamanos { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
