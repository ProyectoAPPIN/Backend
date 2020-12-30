using com.mercaderia.bono.DAL.Repositorios;
using com.mercaderia.bono.Entidades.ModeloEntidades;
using System;

namespace com.mercaderia.bono.DAL
{
    public class UnitOfWork : IDisposable
    {
        private BilleteraEntities context = new BilleteraEntities();
        
        private TipoDocumentoRepositorio tipoDocumentoRepositorio;
        private InstitucionRepositorio institucionRepositorio;
        private UsuarioRepositorio usuarioRepositorio;
        private DominioRepositorio dominioRepositorio;
        private RolesRepositorio rolesRepositorio;
        private SintomasRepositorio sintomasRepositorio;
        private RegistroSintomasRepositorio registroSintomasRepositorio;
        private EventosRepositorio eventosRepositorio;
        private RegistroLavadoRepositorio registroLavadoRepositorio;
        private LavamanosRepositorio lavamanosRepositorio;

        public TipoDocumentoRepositorio TipoDocumentoRepositorio
        {
            get
            {
                if (this.tipoDocumentoRepositorio == null)
                    this.tipoDocumentoRepositorio = new TipoDocumentoRepositorio(context);
                return tipoDocumentoRepositorio;
            }
        }

        public InstitucionRepositorio InstitucionRepositorio
        {
            get
            {
                if (this.institucionRepositorio == null)
                    this.institucionRepositorio = new InstitucionRepositorio(context);
                return institucionRepositorio;
            }
        }

        public UsuarioRepositorio UsuarioRepositorio
        {
            get
            {
                if (this.usuarioRepositorio == null)
                    this.usuarioRepositorio = new UsuarioRepositorio(context);
                return usuarioRepositorio;
            }
        }

        public DominioRepositorio DominioRepositorio
        {
            get
            {
                if (this.dominioRepositorio == null)
                    this.dominioRepositorio = new DominioRepositorio(context);
                return dominioRepositorio;
            }
        }

        public RolesRepositorio RolesRepositorio
        {
            get
            {
                if (this.rolesRepositorio == null)
                    this.rolesRepositorio = new RolesRepositorio(context);
                return rolesRepositorio;
            }
        }

        public SintomasRepositorio SintomasRepositorio
        {
            get
            {
                if (this.sintomasRepositorio == null)
                    this.sintomasRepositorio = new SintomasRepositorio(context);
                return sintomasRepositorio;
            }
        }

        public RegistroSintomasRepositorio RegistroSintomasRepositorio
        {
            get
            {
                if (this.registroSintomasRepositorio == null)
                    this.registroSintomasRepositorio = new RegistroSintomasRepositorio(context);
                return registroSintomasRepositorio;
            }
        }

        public EventosRepositorio EventosRepositorio
        {
            get
            {
                if (this.eventosRepositorio == null)
                    this.eventosRepositorio = new EventosRepositorio(context);
                return eventosRepositorio;
            }
        }

        public RegistroLavadoRepositorio RegistroLavadoRepositorio
        {
            get
            {
                if (this.registroLavadoRepositorio == null)
                    this.registroLavadoRepositorio = new RegistroLavadoRepositorio(context);
                return registroLavadoRepositorio;
            }
        }

        public LavamanosRepositorio LavamanosRepositorio
        {
            get
            {
                if (this.lavamanosRepositorio == null)
                    this.lavamanosRepositorio = new LavamanosRepositorio(context);
                return lavamanosRepositorio;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
