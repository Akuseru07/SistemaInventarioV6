using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class KardexInventarioConfiguracion : IEntityTypeConfiguration<KardexInventario>
    {
        public void Configure(EntityTypeBuilder<KardexInventario> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BodegaProductoId).IsRequired();
            builder.Property(x => x.Tipo).IsRequired();
            builder.Property(x => x.Detalle).IsRequired();
            builder.Property(x => x.StockAnterior).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();

            //Relaciones
            //HasOne es de uno y withmany a muchos

            builder.HasOne(x => x.BodegaProducto).WithMany()
                .HasForeignKey(x => x.BodegaProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UsuarioAplicacion).WithMany()
                .HasForeignKey(x => x.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);
            //Si yo no quiero que me afecte el caso de borrado en cascada se le pone el on delete no action, si si quiero, no mas le cambio a Cascade

        }
    }
}
