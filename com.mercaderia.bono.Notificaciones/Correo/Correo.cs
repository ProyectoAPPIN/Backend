using System.Collections.Generic;
using System.IO;

namespace com.mercaderia.bono.Notificaciones.Correo
{
    public class Correo
    {
       
            #region atributos propios de la entidad 
            
                /// <summary>
                /// Gets or sets the de.
                /// </summary>
                /// <value>The de.</value>
                public string De { get; set; }

                /// <summary>
                /// Gets or sets the para.
                /// </summary>
                /// <value>The para.</value>
                public List<string> Para { get; set; }

                /// <summary>
                /// Gets or sets the con copia.
                /// </summary>
                /// <value>The con copia.</value>
                public List<string> ConCopia { get; set; }

                /// <summary>
                /// Gets or sets the asunto.
                /// </summary>
                /// <value>The asunto.</value>
                public string Asunto { get; set; }

                /// <summary>
                /// Gets or sets the cuerpo.
                /// </summary>
                /// <value>The cuerpo.</value>
                public string Cuerpo { get; set; }

                /// <summary>
                /// Gets or sets the adjunto.
                /// </summary>
                /// <value>The adjunto.</value>
                public Stream Adjunto { get; set; }

                /// <summary>
                /// Gets or sets the nombre adjunto.
                /// </summary>
                /// <value>The nombre adjunto.</value>
                public string NombreAdjunto { get; set; }
            #endregion
    }
}
