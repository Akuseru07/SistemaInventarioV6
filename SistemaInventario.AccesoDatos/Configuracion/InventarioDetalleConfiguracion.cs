using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class InventarioDetalleConfiguracion : IEntityTypeConfiguration<InventarioDetalle>
    {
        public void Configure(EntityTypeBuilder<InventarioDetalle> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.InventarioId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.StockAnterior).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();

            //Relaciones
            //HasOne es de uno y withmany a muchos

            builder.HasOne(x => x.Inventario).WithMany()
                .HasForeignKey(x => x.InventarioId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);
            //Si yo no quiero que me afecte el caso de borrado en cascada se le pone el on delete no action, si si quiero, no mas le cambio a Cascade

        }
    }
}
