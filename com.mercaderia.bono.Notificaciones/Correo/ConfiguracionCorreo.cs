namespace com.mercaderia.bono.Notificaciones.Correo
{
    public class ConfiguracionCorreo
    {
        /// <summary>
        /// The invalid
        /// </summary>
        public bool invalida = false;

        /// <summary>
        /// Gets or sets the paswword user server SMTP.
        /// </summary>
        /// <value>the paswword user server SMTP.</value>
        public string claveUsuarioServidorSMTP { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [habilita SSL servidor SMTP].
        /// </summary>
        /// <value><c>true</c> if [habilita SSL servidor SMTP]; otherwise, <c>false</c>.</value>
        public bool habilitaSSLServidorSMTP { get; set; }

        /// <summary>
        /// Gets or sets the server port SMTP.
        /// </summary>
        /// <value>The server port SMTP.</value>
        public int puertoServidorSMTP { get; set; }

        /// <summary>
        /// Gets or sets the server SMTP.
        /// </summary>
        /// <value>The servidor SMTP.</value>
        public string servidorSMTP { get; set; }
        /// <summary>
        /// Gets or sets the Attached file size.
        /// </summary>
        /// <value>The Attached file size..</value>
        public int tamanioArchivoAdjunto { get; set; }

        /// <summary>
        /// Gets or sets the user server SMTP.
        /// </summary>
        /// <value>The usuario servidor SMTP.</value>
        public string usuarioServidorSMTP { get; set; }
    }
}
