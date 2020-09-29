using log4net;
using System;
using System.Reflection;

namespace com.mercaderia.bono.Utilidades.Auditoria
{
    public class Logger
    {
        #region Singleton
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        public static Logger Instancia
        {
            get
            {
               
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Logger();
                            log4net.Config.XmlConfigurator.Configure();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void EscribirLogError(object mensaje, Exception exception)
        {
            if (mensaje == null)
                throw new ArgumentNullException("mensaje");
            if (exception == null)
                throw new ArgumentNullException("exception");

            //Se registra el log en el componente
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(mensaje, exception);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void EscribirLogError(object mensaje)
        {
            if (mensaje == null)
                throw new ArgumentNullException("mensaje");

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(mensaje);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void EscribirInfo(object mensaje)
        {
            if (mensaje == null)
                throw new ArgumentNullException("mensaje");

            string t = Assembly.GetExecutingAssembly().Location;

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(mensaje);

        }
    }
}
