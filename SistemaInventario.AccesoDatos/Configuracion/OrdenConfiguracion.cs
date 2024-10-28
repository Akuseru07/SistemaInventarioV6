using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class OrdenConfiguracion : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();
            builder.Property(x => x.FechaOrden).IsRequired();
            builder.Property(x => x.TotalOrden).IsRequired();
            builder.Property(x =>x.EstadoOrden).IsRequired();
            builder.Property(x => x.EstadoPago).IsRequired();
            builder.Property(x => x.TotalOrden).IsRequired();
            builder.Property(x => x.NombresCliente).IsRequired();
            builder.Property(x => x.NumeroEnvio).IsRequired(false);
            builder.Property(x => x.Carrier).IsRequired();
            builder.Property(x => x.TransaccionId).IsRequired();
            builder.Property(x => x.Telefono).IsRequired();
            builder.Property(x => x.Direccion).IsRequired();
            builder.Property(x => x.Ciudad).IsRequired();
            builder.Property(x => x.Pais).IsRequired();

            //Relaciones
            builder.HasOne(x => x.UsuarioAplicacion).WithMany()
                   .HasForeignKey(x => x.UsuarioAplicacionId)
                   .OnDelete(DeleteBehavior.NoAction);

            

            
        }
    }
}
