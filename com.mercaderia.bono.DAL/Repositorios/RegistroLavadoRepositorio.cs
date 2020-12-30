using com.mercaderia.bono.Entidades.Dto;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mercaderia.bono.DAL
{
    public sealed partial class RegistroLavadoRepositorio : Repositorio<RegistroLavado>
    {
        public RegistroLavadoRepositorio(BilleteraEntities context) : base(context)
        {
        }
        /// <summary>
        /// Método que obtiene el ultimo registrro de lavado de manos de un usuario
        /// </summary>
        /// <returns></returns>
        public List<ultimoLavadoDTO> ObtenerUltimoLavado(string codUsuario)
        {
            List<ultimoLavadoDTO> lstUltimoLavado = new List<ultimoLavadoDTO>();

            int usuario = Convert.ToInt32(codUsuario);
            var ultimoLavado = context.RegistroLavado.Where(x => x.codUsuario == usuario);
            Int32 registro = 0;

            if (ultimoLavado.Count() == 0)
            {
                lstUltimoLavado.Add(new ultimoLavadoDTO()
                {
                    codUsuario = -1,
                    fecha = null,
                    hora = null
                });
            }

            if (ultimoLavado.Count() > 0)
            {
               
                registro = ultimoLavado.Where(x => x.fechaCierre != null).Select(x => x.codRegistro).Max();
                var consulta = (from lava in ultimoLavado
                            where lava.codRegistro == registro
                            select new ultimoLavadoDTO()
                            {
                                codUsuario = lava.codUsuario,
                                fecha = lava.fecha,
                                hora = lava.hora_cierre
                            }).ToList();

                lstUltimoLavado.Add(new ultimoLavadoDTO()
                {
                    codUsuario = consulta.Select(x => x.codUsuario).FirstOrDefault(),
                    fecha = consulta.Select(x => x.fecha).FirstOrDefault(),
                    hora = consulta.Select(x => x.hora).FirstOrDefault()
                });
            }            
             return lstUltimoLavado;          
        }

        /// <summary>
        /// Método que obtiene el recordatorio de lavado de manos de un usuario
        /// </summary>
        /// <returns></returns>
        public List<RecordatorioDTO> ObtenerRecordatorioLavado(string codUsuario)
        {
            List<RecordatorioDTO> lstRecordatorioLavado = new List<RecordatorioDTO>();

            int usuario = Convert.ToInt32(codUsuario);
            Nullable<DateTime> fechaIngreso = DateTime.Now.Date;

            var recordatorioLavado = context.RegistroLavado.Where(x => x.codUsuario == usuario && x.fecha == fechaIngreso);
            Int32 registro = 0;
            
            if(recordatorioLavado.Count() > 0)
            {
                var consulta = (from lava in recordatorioLavado
                                select new RecordatorioDTO()
                                {
                                    codRegistro = lava.codRegistro,
                                    codUsuario = lava.codUsuario,
                                    fecha = lava.fecha,
                                    hora = lava.hora_registro,
                                    fechaCierre = lava.fechaCierre,
                                    horaCierre = lava.hora_cierre
                                }).ToList();

                foreach (var item in consulta)
                {
                    lstRecordatorioLavado.Add(new RecordatorioDTO()
                    {
                        codRegistro = item.codRegistro,
                        codUsuario = item.codUsuario,
                        fecha = item.fecha,
                        hora = item.hora,
                        fechaCierre = item.fechaCierre,
                        horaCierre = item.horaCierre
                    });
                }

                return lstRecordatorioLavado;

            }
            else
            {
                string hora = DateTime.Now.ToString("HH:mm:ss");
                int[] partes = hora.Split(new char[] { ':' }).Select(x => Convert.ToInt32(x)).ToArray();

                TimeSpan tiempo = new TimeSpan(partes[0], partes[1], partes[2]);
                lstRecordatorioLavado.Add(new RecordatorioDTO()
                {
                    codRegistro = 1,
                    codUsuario = 6723,
                    fecha = DateTime.Now,
                    hora = tiempo,
                    fechaCierre = null,
                    horaCierre = null
                });
            }
           
            return lstRecordatorioLavado;                       
        }

        public List<RecordatorioDTO> ObtenerRecordatorioLavadoEmail(string codUsuario)
        {
            List<RecordatorioDTO> lstRecordatorioLavado = new List<RecordatorioDTO>();

            int usuario = Convert.ToInt32(codUsuario);
            Nullable<DateTime> fechaIngreso = DateTime.Now.Date;

            var recordatorioLavado = context.RegistroLavado.Where(x => x.codUsuario == usuario 
                                                                  && x.fecha == fechaIngreso 
                                                                  && x.fechaCierre != null);            


            var consulta = (from lava in recordatorioLavado
                            select new RecordatorioDTO()
                            {
                                codRegistro = lava.codRegistro,
                                codUsuario = lava.codUsuario,
                                fecha = lava.fecha,
                                hora = lava.hora_registro,
                                fechaCierre = lava.fechaCierre,
                                horaCierre = lava.hora_cierre
                            }).ToList();

            return consulta.ToList();
        }

        public RegistroLavado ObtenerPorCodRegistro(int codRegistro)
        {
            return dbSet.Where(x => x.codRegistro == codRegistro).FirstOrDefault();
        }
    }
}
