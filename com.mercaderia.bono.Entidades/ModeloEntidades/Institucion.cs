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
    
    public partial class Institucion
    {
        public string codInstitucion { get; set; }
        public string nombre { get; set; }
        public string nit { get; set; }
        public string tipo { get; set; }
        public string codMunicipio { get; set; }
        public Nullable<bool> activo { get; set; }
    
        public virtual Municipio Municipio { get; set; }
    }
}
