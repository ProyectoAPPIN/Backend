using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Entidades.Enumeradores
{
    public static class Enums
    {
           
        /// <summary>
        /// Dominios
        /// </summary>
        public enum EnumTablaDominio
        {

            /// <summary>
            /// Tipo de identificación
            /// </summary>
            tipo_identificacion,

            /// <summary>
            /// Número de minutos para el vencimiento
            /// </summary>
            vencimiento_token_monedero,

            /// <summary>
            /// Cuerpo del mensaje porSMS
            /// </summary>
            sms_formato,

            /// <summary>
            /// URL del servicio de SMS
            /// </summary>
            sms_service_url,

            /// <summary>
            /// Nombre del destinatario del bonus
            /// </summary>
            sms_bonus_from,

            /// <summary>
            /// Prefix internacional para Colombia
            /// </summary>
            mobie_prefix_colombia,

            /// <summary>
            /// Configuración para el envío del correo electrónico
            /// </summary>
            configuracionSMTP,

            /// <summary>
            /// Correo remitente principal
            /// </summary>
            correoRemitentePrincipal,

            /// <summary>
            /// Asunto para el correo sobre registro de pedido
            /// </summary>
            asuntoCorreoRegistroPedido,

            /// <summary>
            /// Plantilla del correo registro pedido
            /// </summary>
            plantillaCorreoRegistroPedido,

            /// <summary>
            /// Asunto de correo de bono generado
            /// </summary>
            asuntoCorreoUsuarioGenerado,
            /// <summary>
            /// Plantilla de correo de bono generado
            /// </summary>
            plantillaCorreoBonoGenerado,
            /// <summary>
            /// Formato de texto de correo de bono generado
            /// </summary>
            emailActivacionFormato,
            /// <summary>
            /// Tipo de movimiento origen de una transacción
            /// </summary>
            TipoOrigenMov,

            /// <summary>
            /// Terminos y Condiciones
            /// </summary>
            terminos_y_condiciones,

            /// <summary>
            /// Número de minutos para el vencimiento
            /// </summary>
            vencimiento_token_activacion
        }        
    }
}
