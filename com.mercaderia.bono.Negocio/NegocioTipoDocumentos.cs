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
    }
}
