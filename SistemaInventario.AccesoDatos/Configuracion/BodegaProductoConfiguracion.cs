using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class BodegaProductoConfiguracion : IEntityTypeConfiguration<BodegaProducto>
    {
        public void Configure(EntityTypeBuilder<BodegaProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BodegaId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();

            //Relaciones
            //HasOne es de uno y withmany a muchos

            builder.HasOne(x => x.Bodega).WithMany()
                .HasForeignKey(x => x.BodegaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);
            //Si yo no quiero que me afecte el caso de borrado en cascada le agregamos el ondelete o algo asi

        }
    }
}
