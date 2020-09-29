using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using com.mercaderia.bono.Utilidades.Auditoria;

namespace Bonos.Api.Rest.Helper
{
    public static class RetryHelper
    {
        static Logger log = Logger.Instancia;
        public static void RetryOnException(int times, TimeSpan delay, Action operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    operation();
                    break; 
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                        throw;

                    log.EscribirLogError($"Exception caught on attempt {attempts} - will retry after delay {delay}", ex);

                    Task.Delay(delay).Wait();
                }
            } while (true);
        }
    }
}
