using Azure;
using Microsoft.EntityFrameworkCore;

namespace Capa_de_datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tablas principales
        public DbSet<UsuarioDAL> Usuario { get; set; }
        public DbSet<ClienteDAL> Cliente { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<DetalleFactura> DetalleFactura { get; set; }
        public DbSet<Cantidad> Cantidad { get; set; }
        public DbSet<PagoDAL> Pago { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Condicion> Condiciones { get; set; }
        public DbSet<ProductoPresentacion> ProductoPresentaciones { get; set; }
        public DbSet<LoginLogDAL> LoginLogs { get; set; }
        public DbSet<FormaPago> FormaPago { get; set; }

     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Claves primarias con nombres personalizados
            modelBuilder.Entity<ClienteDAL>().HasKey(c => c.ClienteID);
            modelBuilder.Entity<UsuarioDAL>().HasKey(u => u.UsuarioID);
            modelBuilder.Entity<Producto>().HasKey(p => p.ProductoID);
            modelBuilder.Entity<Factura>().HasKey(f => f.FacturaID);
            modelBuilder.Entity<PagoDAL>().HasKey(p => p.FormaPagoID);
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaID);
            modelBuilder.Entity<Condicion>().HasKey(c => c.ID_Condicion);
            modelBuilder.Entity<Cantidad>().HasKey(c => c.CantidadID);
            modelBuilder.Entity<ProductoPresentacion>().HasKey(pp => pp.PresentacionID);
            modelBuilder.Entity<FormaPago>()
               .HasKey(f => f.PagoID);
            modelBuilder.Entity<Categoria>()
                .HasKey(c => c.CategoriaID);
            modelBuilder.Entity<ClienteDAL>()
                .HasKey(c => c.ClienteID);
            modelBuilder.Entity<Producto>()
                .HasKey(p => p.ProductoID);


            // LoginLog: nombre de tabla + PK + relación con Usuario
            modelBuilder.Entity<LoginLogDAL>(e =>
            {
                e.ToTable("LoginLog");
                e.HasKey(x => x.LoginID);
                e.HasOne(x => x.Usuario)
                 .WithMany(u => u.LoginLogs)
                 .HasForeignKey(x => x.UsuarioID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
