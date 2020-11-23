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
        public Usuario CrearUsuario(UsuarioDto usuario)
        {
            var codigoActivacion = GenerarCodigoActivacion();
            var usuarioGenerado = RegistrarUsuario(usuario, codigoActivacion);

            if (usuarioGenerado != null)
            {                
                EnviarEmail(usuario.correo, codigoActivacion);                
            }

            return usuarioGenerado;
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
                        codActivacionExpira = DateTime.Now.AddMinutes(int.Parse(dominioEncontrado.valor)),
                        tokenDispositivo = usuario.token
                };
                    unitOfWork.UsuarioRepositorio.Adicionar(nuevoUsuario);
                    unitOfWork.Save();
                }
                //Actualizo la informacion del usuario ya existente
                else
                {
                    nuevoUsuario = new Usuario()
                    {
                        codUsuario = persona.codUsuario,
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
                        codActivacionExpira = DateTime.Now.AddMinutes(int.Parse(dominioEncontrado.valor)),
                        tokenDispositivo = usuario.token
                    };
                    //Cargo el objeto para modificarlo
                    persona.nombres = nuevoUsuario.nombres;
                    persona.apellidos = nuevoUsuario.apellidos;
                    persona.TipoDocumento = nuevoUsuario.TipoDocumento;
                    persona.numeroDocumento = nuevoUsuario.numeroDocumento;
                    persona.celular = nuevoUsuario.celular;
                    persona.correo = nuevoUsuario.correo;
                    persona.codPerfil = nuevoUsuario.codPerfil;
                    persona.codInstitucion = nuevoUsuario.codInstitucion;
                    persona.sexo = nuevoUsuario.sexo;
                    persona.activo = nuevoUsuario.activo;
                    persona.codigoActivacion = nuevoUsuario.codigoActivacion;
                    persona.codActivacionExpira = nuevoUsuario.codActivacionExpira;
                    persona.tokenDispositivo = nuevoUsuario.tokenDispositivo;
                    unitOfWork.UsuarioRepositorio.Actualizar(persona);
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
            int rInt = r.Next(0, 999999); //Numero Aleatorio 4 digitos
            code =  rInt.ToString();
            code = code.Substring(0, 4);
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

                Dominio plantillaCorreo = negocioDominio.ConsultarPorId(EnumTablaDominio.plantillaCorreoActivacionCuenta.ToString());
                if (plantillaCorreo.IsNull() || string.IsNullOrEmpty(plantillaCorreo.valor))
                    throw new ExceptionControlada(Mensajes.MsgTextoInicialCorreoRegistroPedido);

                var bytes = Convert.FromBase64String(Constantes.IMAGEN_BANNER_EMAIL);
                var contents = new MemoryStream(bytes);

                correoEnviar.De = dominioRemitentePrincipal.valor;
                correoEnviar.Asunto = domonioAsuntoCorreo.valor;
                correoEnviar.Para = new List<string>() { email };
                correoEnviar.Cuerpo = plantillaCorreo.valor.Replace("@MESSAGE", message);
                correoEnviar.Adjunto = contents;
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
        public List<respuestaActivacionDto> ActivacionUsuario(UsuarioActivoDto usuario)
        {
            List<respuestaActivacionDto> respuesta = new List<respuestaActivacionDto>();            
            int error = 0; 
            if (string.IsNullOrEmpty(usuario.codUsuario))
            {
                respuesta.Add(new respuestaActivacionDto()
                {
                    codRespuesta = Mensajes.codErrorUsuarioVacio,
                    codUsuario = Mensajes.MsgUsuarioVacio,
                    codigoActivacion = null,
                    activo = false                    
                });
                error = 1;
            }            

            if (string.IsNullOrEmpty(usuario.codigoActivacion))
            {
                respuesta.Add(new respuestaActivacionDto()
                {
                    codRespuesta = Mensajes.codErrorTokenVacio,
                    codUsuario = Mensajes.MsgTokenVacio,
                    codigoActivacion = null,
                    activo = false
                });
                error = 1;
            }
            if (error == 0)
            {
                Usuario usuarioEncontrado = this.ConsultarPorId(Convert.ToInt32(usuario.codUsuario));

                if (usuarioEncontrado.IsNull())
                {
                    respuesta.Add(new respuestaActivacionDto()
                    {
                        codRespuesta = Mensajes.codErrorMsgUsuarioNoExistente,
                        codUsuario = Mensajes.MsgUsuarioNoExistente,
                        codigoActivacion = null,
                        activo = false
                    });
                    error = 1;
                }

                if (!usuarioEncontrado.codigoActivacion.Equals(usuario.codigoActivacion))
                {

                    respuesta.Add(new respuestaActivacionDto()
                    {
                        codRespuesta = Mensajes.codMsgcodMsgtokenErroneo,
                        codUsuario = Mensajes.MsgcodMsgtokenErroneo,
                        codigoActivacion = null,
                        activo = false
                    });
                    error = 1;
                }

                if (usuarioEncontrado.codActivacionExpira < DateTime.Now)
                {
                    respuesta.Add(new respuestaActivacionDto()
                    {
                        codRespuesta = Mensajes.codMsgtokenExpiraErroneo,
                        codUsuario = Mensajes.MsgtokenExpiraErroneo,
                        codigoActivacion = null,
                        activo = false
                    });
                    error = 1;
                }

                // pasa todas las validaciones realizo la activacion del usuario           
                if (error == 0)
                {
                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        usuarioEncontrado.codUsuario = usuarioEncontrado.codUsuario;
                        usuarioEncontrado.activo = true;
                        unitOfWork.UsuarioRepositorio.Actualizar(usuarioEncontrado);
                        unitOfWork.Save();

                        respuesta.Add(new respuestaActivacionDto()
                        {
                            codRespuesta = "1",
                            codUsuario = Convert.ToString(usuarioEncontrado.codUsuario),
                            codigoActivacion = usuarioEncontrado.codigoActivacion,
                            activo = true
                        });

                    }
                }

            }   
               
            return respuesta;
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
                    lstUsuarioActivo.Add(new AccesoUsuarioDto()
                    {
                        codUsuario = -1,
                        nombres = null,                        
                        activo = false
                    });
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
        public List<UsuarioDto> UsuarioPreCargue(string tipoDocumento, string documento)
        {
            List<UsuarioDto> lstUsuarioExistente = new List<UsuarioDto>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                lstUsuarioExistente = unitOfWork.UsuarioRepositorio.ObtenerUsuarioExistente(tipoDocumento, documento);

                if (lstUsuarioExistente == null)
                {
                    throw new ExceptionControlada(Mensajes.MsgUsuarioActivoError);
                }

                if (lstUsuarioExistente.Count == 0)
                {
                    lstUsuarioExistente.Add(new UsuarioDto() {
                        codUsuario = -1,
                        nombres = null,
                        apellidos = null,
                        TipoDocumento = null,
                        numeroDocumento = null,
                        celular = null ,
                        codPerfil = -1,
                        codInstitucion = null,
                        correo = null,
                        sexo = false,
                        activo = false
                    });
                }               
            }
            return lstUsuarioExistente;
        }
    }
}

