using com.mercaderia.bono.DAL;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using com.mercaderia.bono.Utilidades.Auditoria;
using com.mercaderia.bono.Entidades.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using com.mercaderia.bono.Utilidades.Excepciones;
using com.mercaderia.bono.Entidades.Enumeradores;
using static com.mercaderia.bono.Entidades.Enumeradores.Enums;
using com.mercaderia.bono.Notificaciones.Correo;

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioTipoDocumentos //: NegocioAbstracto
    {
        /// <summary>
        /// Método que obtiene todos los documentos existentes
        /// </summary>
        /// <returns></returns>
        /// 
        public List<TipoDocumento> ObtenerTipoDocumentos()
        {
            List<TipoDocumento> lstTipoDoc = new List<TipoDocumento>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstTipoDoc = unitOfWork.TipoDocumentoRepositorio.ObtenerTipoDocumento();                   
                if (lstTipoDoc == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoError);
                }
                if (lstTipoDoc.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoInexistente);
                }
            }
            return lstTipoDoc;
        }

        /// <summary>
        /// Método que obtiene los datos de una ciudad específica (por código)
        /// </summary>
        /// <param name="pCodCiudad">
        /// Código de la Ciudad
        /// </param>
        /// <returns></returns>
        public TipoDocumento ObtenerCiudadPorCodigo(string pDocumento)
        {
            TipoDocumento tipodoc = new TipoDocumento();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                tipodoc = unitOfWork.TipoDocumentoRepositorio.ObtenerTipoDocPorCodigo(pDocumento);
                if (tipodoc.IsNull())
                {
                    throw new ExceptionControlada(Mensajes.MsgTipoDocumentoInexistente);
                }               
            }
            return tipodoc;
        }

        /// <summary>
        /// Método que obtiene todos roles
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Perfil> ObtenerRoles()
        {
            List<Perfil> lstperfil = new List<Perfil>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstperfil = unitOfWork.RolesRepositorio.ObtenerRoles();
                if (lstperfil == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgRolesError);
                }
                if (lstperfil.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgRolesInexistente);
                }
            }
            return lstperfil;
        }

        public string sendNotificacion()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {                
                var persona = unitOfWork.UsuarioRepositorio.ObtenerListaUsuariosActivo();

                if (persona.Count > 0)
                {
                    foreach (var item in persona)
                    {
                        EnviarEmail(item.correo);
                    }
                }
                return "Notificaciones enviadas correctamente";
            }
        }

        /// <summary>
        /// Metodo para enviar correo notificacion Masivo
        /// </summary>        
        /// <returns></returns>
        private void EnviarEmail(string email)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                Dominio emaiNotificacionLavadoFormato = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.notificacionLavado.ToString());

                if (emaiNotificacionLavadoFormato.IsNull())
                    throw new ExceptionControlada(string.Format(Mensajes.MsgErrorDominioNoEncontrado, Enums.EnumTablaDominio.sms_formato.ToString()));
                var message = string.Format(emaiNotificacionLavadoFormato.valor);

                Dominio dominioSMTP = negocioDominio.ConsultarPorId(EnumTablaDominio.configuracionSMTP.ToString());
                if (dominioSMTP.IsNull() || string.IsNullOrEmpty(dominioSMTP.valor))
                    throw new ExceptionControlada(Mensajes.MsgConfiguracionSMTPRequerida);
                CorreoProxy correoProxy = new CorreoProxy(dominioSMTP.valor);
                Correo correoEnviar = new Correo();


                Dominio dominioRemitentePrincipal = negocioDominio.ConsultarPorId(EnumTablaDominio.correoRemitentePrincipal.ToString());
                if (dominioRemitentePrincipal.IsNull() || string.IsNullOrEmpty(dominioRemitentePrincipal.valor))
                    throw new ExceptionControlada(Mensajes.MsgCorreoRemitentePrincipal);

                Dominio domonioAsuntoCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.asuntoNotificacionLavado.ToString());
                if (domonioAsuntoCorreo.IsNull() || string.IsNullOrEmpty(domonioAsuntoCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgAsuntoCorreoRegistroPedido);

                Dominio plantillaCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.plantillaCorreoNotificacion.ToString());
                if (plantillaCorreo.IsNull() || string.IsNullOrEmpty(plantillaCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgTextoInicialCorreoRegistroPedido);

                var bytes = Convert.FromBase64String(Constantes.IMAGEN_BANNER_NOTI);

                correoEnviar.De = dominioRemitentePrincipal.valor;
                correoEnviar.Asunto = domonioAsuntoCorreo.valor;
                correoEnviar.Para = new List<string>() { email };
                correoEnviar.Cuerpo = plantillaCorreo.valor.Replace("@MESSAGE", message);
                correoProxy.EnviarCorreo(correoEnviar, true);
            }
            catch (Exception ex)
            {
                Logger log = Logger.Instancia;
                log.EscribirLogError($"Error al enviar Correo bono:{10}", ex);
                return;
            }
        }


    }
}
