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
        builder.Property(nameof(HistoricoBusca.ProdutoNome)).HasColumnName("ProdutoNome");
        builder.Property(nameof(HistoricoBusca.ValorCompra)).HasColumnName("ValorCompra");
        builder.Property(nameof(HistoricoBusca.Ativo)).HasColumnName("Ativo");
        builder.Property(nameof(HistoricoBusca.DataBusca)).HasColumnName("DataBusca");

        builder.HasOne(x => x.Usuario)
                       .WithMany()
                       .HasForeignKey(x => x.UsuarioId)
                       .IsRequired();

    }
}