using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestPick.Repositorio.Configuracoes;

public class HistoricoBuscaConfiguracoes : IEntityTypeConfiguration<HistoricoBusca>
{
    public void Configure(EntityTypeBuilder<HistoricoBusca> builder)
    {
        builder.ToTable("HistoricoBusca").HasKey(x => x.Id);

        builder.Property(nameof(HistoricoBusca.Id)).HasColumnName("HistoricoBuscaId");
        builder.Property(nameof(HistoricoBusca.MotivoCompra)).HasColumnName("MotivoCompra");
        builder.Property(nameof(HistoricoBusca.UsuarioId)).HasColumnName("UsuarioId");
        builder.Property(nameof(HistoricoBusca.ProdutoId)).HasColumnName("ProdutoId");
        builder.Property(nameof(HistoricoBusca.Produto)).HasColumnName("Produto");
        builder.Property(nameof(HistoricoBusca.ValorCompra)).HasColumnName("ValorCompra");
        builder.Property(nameof(HistoricoBusca.Ativo)).HasColumnName("Ativo");

        builder.HasOne(x => x.Usuario)
                       .WithMany()
                       .HasForeignKey(x => x.UsuarioId)
                       .IsRequired();


        builder.HasOne(x => x.Produto)
                        .WithMany()
                        .HasForeignKey(x => x.ProdutoId)
                        .IsRequired();


    }
}