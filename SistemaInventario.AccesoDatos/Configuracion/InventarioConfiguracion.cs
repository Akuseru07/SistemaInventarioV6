using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;

namespace SistemaInventarioV6.Configuracion
{
    //FLUENT API
    public class InventarioConfiguracion : IEntityTypeConfiguration<Inventario>
    {
        public void Configure(EntityTypeBuilder<Inventario> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BodegaId).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();
            builder.Property(x => x.FechaFinal).IsRequired();
            builder.Property(x => x.FechaInicial).IsRequired();
            builder.Property(x => x.Estado).IsRequired();

            //Relaciones
            //HasOne es de uno y withmany a muchos

            builder.HasOne(x => x.Bodega).WithMany()
                .HasForeignKey(x => x.BodegaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UsuarioAplicacion).WithMany()
                .HasForeignKey(x => x.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);
            //Si yo no quiero que me afecte el caso de borrado en cascada se le pone el on delete no action, si si quiero, no mas le cambio a Cascade

        }
    }
}
