﻿using com.mercaderia.bono.DAL;
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
using com.mercaderia.bono.Entidades.Dto;
using com.mercaderia.bono.Entidades.Enumeradores;
using static com.mercaderia.bono.Entidades.Enumeradores.Enums;
using com.mercaderia.bono.Notificaciones.Correo;
using System.IO;

namespace com.mercaderia.bono.Negocio
{
    public sealed partial class NegocioUsuario
    {
        /// <summary>
        /// metodo que consulta un usuario por tipo de identificacion y tipo de identificacion
        /// </summary>
        /// <param name="numUsuario"></param>
        /// <returns></returns>
        public Usuario ConsultarPorId(int numUsuario)
        {
            Usuario usuario = new Usuario();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                usuario = unitOfWork.UsuarioRepositorio.ObtenerPorID(numUsuario);
            }
            return usuario;
        }
        /// <summary>
        /// Metodo para registrar un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string CrearUsuario(UsuarioDto usuario)
        {
            var codigoActivacion = GenerarCodigoActivacion();
            var usuarioGenerado = RegistrarUsuario(usuario, codigoActivacion);

            if (usuarioGenerado != null)
            {                
                EnviarEmail(usuario.correo, codigoActivacion);                
            }

            return Convert.ToString(usuarioGenerado.codUsuario);
        }
        /// <summary>
        /// Metodo pra registrar el usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="codigoActivacion"></param>
        /// <returns></returns>
        private Usuario RegistrarUsuario(UsuarioDto usuario, string codigoActivacion)
        {
            var codigoUsuario = GenerarCodigoUsuario();

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                Dominio dominioEncontrado = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.vencimiento_token_activacion.ToString());
                var persona = unitOfWork.UsuarioRepositorio.ObtenerPorCedula(usuario.TipoDocumento, usuario.numeroDocumento);
                Usuario nuevoUsuario = null;
                if (persona == null)
                {
                    nuevoUsuario = new Usuario()
                    {
                        codUsuario = Convert.ToInt32(codigoUsuario),
                        nombres = usuario.nombres,
                        apellidos = usuario.apellidos,
                        TipoDocumento = usuario.TipoDocumento,
                        numeroDocumento = usuario.numeroDocumento,
                        celular = usuario.celular,
                        codPerfil = usuario.codPerfil,
                        codInstitucion = usuario.codInstitucion,
                        correo = usuario.correo,
                        sexo = usuario.sexo,
                        activo = usuario.activo,
                        codigoActivacion = codigoActivacion,
                        codActivacionExpira = DateTime.Now.AddMinutes(int.Parse(dominioEncontrado.valor))
                };
                    unitOfWork.UsuarioRepositorio.Adicionar(nuevoUsuario);
                    unitOfWork.Save();
                }
                return nuevoUsuario;
            }
        }
        /// <summary>
        /// Metodo para generar codigo de usuario aleatorio
        /// </summary>
        /// <returns></returns>
        private string GenerarCodigoUsuario()
        {
            Random r = new Random();
            int i;
            string code = string.Empty;

            for (i = 1; i < 3; ++i)
            {
                int rInt = r.Next(0, 999); //for ints
                code = $"{Constantes.INI_CODIGO_USUARIO}{rInt}";
                var usuario = ConsultarPorId(Convert.ToInt32(code));
                if ((usuario == null))
                {
                    break;
                }

            }
            return code;
        }
        /// <summary>
        /// Metodo para generar codigo activacion aleatorio
        /// </summary>
        /// <returns></returns>
        private string GenerarCodigoActivacion()
        {
            Random r = new Random();           
            string code = "";
            int rInt = r.Next(0, 9999); //Numero Aleatorio 4 digitos
            code =  rInt.ToString();
            return code;
        }
        /// <summary>
        /// Metodo para enviar correo de activacion
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private void EnviarEmail(string email, string codigoActivacion)
        {
            try
            {
                NegocioDominio negocioDominio = new NegocioDominio();
                Dominio emailBonoFormato = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.emailActivacionFormato.ToString());
                if (emailBonoFormato.IsNull())
                    throw new ExceptionControlada(string.Format(Mensajes.MsgErrorDominioNoEncontrado, Enums.EnumTablaDominio.sms_formato.ToString()));
                var message = string.Format(emailBonoFormato.valor, codigoActivacion);


                Dominio dominioSMTP = negocioDominio.ConsultarPorId(EnumTablaDominio.configuracionSMTP.ToString());

                if (dominioSMTP.IsNull() || string.IsNullOrEmpty(dominioSMTP.valor))
                    throw new ExceptionControlada(Mensajes.MsgConfiguracionSMTPRequerida);
                CorreoProxy correoProxy = new CorreoProxy(dominioSMTP.valor);
                Correo correoEnviar = new Correo();


                Dominio dominioRemitentePrincipal = negocioDominio.ConsultarPorId(EnumTablaDominio.correoRemitentePrincipal.ToString());
                if (dominioRemitentePrincipal.IsNull() || string.IsNullOrEmpty(dominioRemitentePrincipal.valor))
                    throw new ExceptionControlada(Mensajes.MsgCorreoRemitentePrincipal);

                Dominio domonioAsuntoCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.asuntoCorreoUsuarioGenerado.ToString());
                if (domonioAsuntoCorreo.IsNull() || string.IsNullOrEmpty(domonioAsuntoCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgAsuntoCorreoRegistroPedido);

                Dominio plantillaCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.plantillaCorreoBonoGenerado.ToString());
                if (plantillaCorreo.IsNull() || string.IsNullOrEmpty(plantillaCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgTextoInicialCorreoRegistroPedido);

                var bytes = Convert.FromBase64String(Constantes.IMAGEN_BANNER_EMAIL);                

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
        /// <summary>
        /// Metodo para activar el usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Usuario ActivacionUsuario(UsuarioActivoDto usuario)
        {
            if (string.IsNullOrEmpty(usuario.codUsuario))
                throw new ExceptionControlada(Mensajes.MsgUsuarioVacio, Mensajes.codErrorUsuarioVacio);

            if (string.IsNullOrEmpty(usuario.codigoActivacion))
                throw new ExceptionControlada(Mensajes.MsgCodigoActivacionVacio, Mensajes.codErrorMsgCodigoActivacionVacio);

            Usuario usuarioEncontrado = this.ConsultarPorId(Convert.ToInt32(usuario.codUsuario));

            if (usuarioEncontrado.IsNull())
                throw new ExceptionControlada(Mensajes.MsgUsuarioNoExistente, Mensajes.codErrorMsgUsuarioNoExistente);

            if (!usuarioEncontrado.codigoActivacion.Equals(usuario.codigoActivacion))
                throw new ExceptionControlada(Mensajes.MsgcodMsgtokenErroneo, Mensajes.codMsgcodMsgtokenErroneo);

            if (usuarioEncontrado.codActivacionExpira < DateTime.Now)
                throw new ExceptionControlada(Mensajes.MsgtokenExpiraErroneo, Mensajes.codMsgtokenExpiraErroneo);
            
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                usuarioEncontrado.codUsuario = usuarioEncontrado.codUsuario;
                usuarioEncontrado.activo = true;                             
                unitOfWork.UsuarioRepositorio.Actualizar(usuarioEncontrado);               
                unitOfWork.Save();
            }
            return usuarioEncontrado;
        }
        /// <summary>
        /// Metodo para validar si el usuario esta activo y dar acceso a la aplicacion
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<AccesoUsuarioDto> ValidaUsuario(string tipoDocumento, string documento, string universidad)
        {
            List<AccesoUsuarioDto> lstUsuarioActivo = new List<AccesoUsuarioDto>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstUsuarioActivo = unitOfWork.UsuarioRepositorio.ObtenerUsuarioActivo(tipoDocumento, documento, universidad);
                if (lstUsuarioActivo == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgUsuarioActivoError);
                }
                if (lstUsuarioActivo.Count == 0)
                {
                    throw new ExceptionControlada(Mensajes.MsgUsuarioActivoErrorInexistente);
                }
            }
            return lstUsuarioActivo;
        }
        /// <summary>
        /// Metodo para reenviar codigo de activacion
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<UsuarioActivoDto> ReeviarCodActivacion(UsuarioActivoDto usuario)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var usuarioExistente = unitOfWork.UsuarioRepositorio.ObtenerPorID(Convert.ToInt32(usuario.codUsuario));

                if (usuario == null)
                    throw new ExceptionControlada(Mensajes.UsuarioNoExiste);                

                if (usuarioExistente != null)
                {
                    var codigoActivacion = GenerarCodigoActivacion();
                    NegocioDominio negocioDominio = new NegocioDominio();
                    Dominio dominioEncontrado = negocioDominio.ConsultarPorId(Enums.EnumTablaDominio.vencimiento_token_activacion.ToString());

                    //Actualizo en codigo activacion en el registro en la base de datos
                    usuarioExistente.codUsuario = Convert.ToInt32(usuario.codUsuario);
                    usuarioExistente.codigoActivacion = codigoActivacion;
                    usuarioExistente.codActivacionExpira = DateTime.Now.AddMinutes(int.Parse(dominioEncontrado.valor));
                    unitOfWork.UsuarioRepositorio.Actualizar(usuarioExistente);
                    unitOfWork.Save();

                    //Envia Email de codigo de activacion
                    EnviarEmail(usuarioExistente.correo, codigoActivacion);
                }

                List<UsuarioActivoDto> ActivacionUsuario = new List<UsuarioActivoDto>();

                ActivacionUsuario.Add(new UsuarioActivoDto
                {
                    codUsuario = Convert.ToString(usuarioExistente.codUsuario),
                    codigoActivacion = usuarioExistente.codigoActivacion,
                    activo = usuarioExistente.activo
                });             
                  
                return ActivacionUsuario; 
            }
        }
        /// <summary>
        /// Metodo para obtener un usuario existente en la base de datos para precargar informacion
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Usuario UsuarioPreCargue(string tipoDocumento, string documento)
        {            
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Usuario lstUsuarioExistente = unitOfWork.UsuarioRepositorio.ObtenerPorCedula(tipoDocumento, documento);
                if (lstUsuarioExistente == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgUsuarioActivoError);
                }
                return lstUsuarioExistente;
            }            
        }
    }
}

