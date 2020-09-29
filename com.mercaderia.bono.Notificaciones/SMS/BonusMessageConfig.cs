using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Notificaciones.SMS
{
    public class BonusMessageConfig
    {
        /// <summary>
        /// Mensaje y parátros
        /// </summary>
       public BonusSMS message { get; set; }

        /// <summary>
        /// Url del servicio de mensajeria 
        /// </summary>
        public string serviceURL { get; set; }

    }
}
