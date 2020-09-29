using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace com.mercaderia.bono.Utilidades.Excepciones
{
	[Serializable]
    public class BaseException : Exception
    {

        public BaseException() { }
        
        public BaseException(string message) : base(message) { }
        
        public BaseException(string message, Exception innerException) : base(message, innerException) { }
        
        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        
    }
}
