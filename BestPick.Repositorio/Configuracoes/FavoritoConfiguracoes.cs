using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestPick.Repositorio.Configuracoes;

public class FavoritoConfiguracoes : IEntityTypeConfiguration<Favorito>
{
    public void Configure(EntityTypeBuilder<Favorito> builder)
    {
        builder.ToTable("Favorito").HasKey(x => x.Id);

        builder.Property(nameof(Favorito.Id)).HasColumnName("FavoritoId");
        builder.Property(nameof(Favorito.Status)).HasColumnName("Status");
        builder.Property(nameof(Favorito.UsuarioId)).HasColumnName("UsuarioId");
        builder.Property(nameof(Favorito.ProdutoId)).HasColumnName("ProdutoId");

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Produto)
            .WithMany()
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}