using com.mercaderia.bono.Entidades.Dto;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace com.mercaderia.bono.DAL
{
    public sealed partial class UsuarioRepositorio : Repositorio<Usuario>
    {
        public UsuarioRepositorio(BilleteraEntities context) : base(context)
        {

        }
        public Usuario ObtenerPorCedula(string tipoIdentificacion, string identificacion)
        {
            return dbSet.Where(x => x.TipoDocumento == tipoIdentificacion && x.numeroDocumento == identificacion && x.activo == false).FirstOrDefault();
        }
        public List<UsuarioDto> ObtenerUsuarioExistente(string tipoIdentificacion, string identificacion)
        {
            var usuarioExistente = context.Usuario.Where(x => x.TipoDocumento == tipoIdentificacion
                                              && x.numeroDocumento == identificacion && x.activo == false);

            var consulta = (from usu in usuarioExistente
                            select new UsuarioDto()
                            {
                                codUsuario = usu.codUsuario,
                                nombres = usu.nombres,
                                apellidos = usu.apellidos,
                                numeroDocumento = usu.numeroDocumento,
                                TipoDocumento = usu.TipoDocumento,
                                codInstitucion = usu.codInstitucion,
                                codPerfil = usu.codPerfil,
                                correo = usu.correo,
                                celular = usu.celular,                                
                                sexo = false
                            }).ToList();

            return consulta.ToList();


            //return dbSet.Where(x => x.TipoDocumento == tipoIdentificacion && x.numeroDocumento == identificacion && x.activo == false).FirstOrDefault();
        }
        public Usuario ObtenerPorID(int codUsuario)
        {
            return dbSet.Where(x => x.codUsuario == codUsuario).FirstOrDefault();
        }
        public List<AccesoUsuarioDto> ObtenerUsuarioActivo(string tipoIdentificacion, string identificacion, string idUniversidad)
        {
            var usuarioActivo =  context.Usuario.Where(x => x.TipoDocumento == tipoIdentificacion
                                                 && x.numeroDocumento == identificacion
                                                 && x.codInstitucion == idUniversidad);

            int codUsuario = usuarioActivo.Select(x => x.codUsuario).FirstOrDefault();
            Nullable<DateTime> fechaIngreso = DateTime.Now.Date;

            var ingresoActivo = context.RegistroIngreso.Where(x => x.codUsuario == codUsuario && x.fecha == fechaIngreso);
            int totRegistros = ingresoActivo.Count();
            int codRegistro = 0;

            if (totRegistros > 0)
            {
                codRegistro = ingresoActivo.Max(x => x.codRegistro);
            }

            string ingActivo = "0";
            if (codRegistro > 0)
            {
                ingActivo = "1";
            }

            var consulta = (from usu in usuarioActivo
                            select new AccesoUsuarioDto()
                            {
                                codUsuario = usu.codUsuario,
                                nombres = usu.nombres + " " + usu.apellidos,
                                activo = usu.activo,
                                ingresoActivo = ingActivo

                            }).ToList();

            return consulta.ToList(); 
        }
        public List<Usuario> ObtenerListaUsuariosActivo()
        {
            var listaUsuariosActivo = context.Usuario.Where(x => x.activo == true && x.tokenDispositivo != null);            

            return listaUsuariosActivo.ToList();
        }

        public List<Usuario> ObtenerListaUsuariosActivoEmail()
        {
            var listaUsuariosActivo = context.Usuario.Where(x => x.activo == true && x.correo != null);

            return listaUsuariosActivo.ToList();
        }
    }
}
