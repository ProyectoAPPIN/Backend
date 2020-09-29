using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Utilidades.Excepciones
{
    public class ExceptionControlada : Exception, ISerializable
    {

        /// <summary>
        /// Código de la excepción controlada
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Inicializa la instancia de ExcepcionControlada
        /// </summary>
        public ExceptionControlada()
        {
        }

        /// <summary>
        /// Technical unhandled exception is when a technical error occurs and shows the user a generic message, but makes exceptions detail.
        /// </summary>
        /// <param name="mensaje"> Information message to the developer.</param>        
        public ExceptionControlada(string mensaje)
            : base(mensaje) { this.Code = string.Empty; }

        public ExceptionControlada(string mensaje, string cod)
            : base(mensaje) { this.Code = cod; }

        public ExceptionControlada(string mensaje, string cod, decimal saldo)
            : base(mensaje) { this.Code = cod; this.Data.Add("SaldoDisponible", saldo);  }

        public ExceptionControlada(string mensaje, string cod, decimal valorTransaccion, string nombre, string apellido, string idTransaccionPost)
            : base(mensaje) { this.Code = cod; this.Data.Add("transaccionId", idTransaccionPost); this.Data.Add("valorTransaccion", valorTransaccion);
            this.Data.Add("nombre", nombre); this.Data.Add("apellido", apellido);  ;
        }

        public ExceptionControlada(string mensaje, string cod, Array errores)
            : base(mensaje) { this.Code = cod; this.Data.Add("ListaErrores", errores); }

        public ExceptionControlada(string mensaje, string cod, string codEstado, string descEstado)
            : base(mensaje) { this.Code = cod; this.Data.Add("CodigoEstado", codEstado); this.Data.Add("DescripcionEstado", descEstado); }

    }
}
