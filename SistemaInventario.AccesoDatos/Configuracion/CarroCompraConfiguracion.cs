using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class CarroCompraConfiguracion : IEntityTypeConfiguration<CarroCompra>
    {
        public void Configure(EntityTypeBuilder<CarroCompra> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();

            //Relaciones
            builder.HasOne(x => x.UsuarioAplicacion).WithMany()
                   .HasForeignKey(x => x.UsuarioAplicacionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                   .HasForeignKey(x => x.ProductoId)
                   .OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}
