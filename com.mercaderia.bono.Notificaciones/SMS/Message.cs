using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.Notificaciones.SMS
{
    public class Message
    {
        public string to { get; set; }
        public Status status { get; set; }
        public string messageId { get; set; }
        public int smsCount { get; set; }
    }
}
