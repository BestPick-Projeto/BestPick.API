using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestPick.Repositorio.Configuracoes;

public class ProdutoConfiguracao : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produto").HasKey(x => x.Id);

        builder.Property(nameof(Produto.Id)).HasColumnName("ProdutoId");
        builder.Property(nameof(Produto.ImgUrl)).HasColumnName("ImgUrl");
        builder.Property(nameof(Produto.Nome)).HasColumnName("Nome");
        builder.Property(nameof(Produto.Preco)).HasColumnName("Preco");
        builder.Property(nameof(Produto.Url)).HasColumnName("Url");
        builder.Property(nameof(Produto.Categoria)).HasColumnName("Categoria");
        builder.Property(nameof(Produto.HistoricoBuscaId)).HasColumnName("HistoricoBuscaId");

        builder.Property(x => x.Beneficios)
           .HasColumnName("Beneficios")
           .HasConversion(
               lista => string.Join(';', lista),
               texto => texto.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
           );

        builder.Property(x => x.Maleficios)
            .HasColumnName("Maleficios")
            .HasConversion(
                lista => string.Join(';', lista),
                texto => texto.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

        builder.HasOne(x => x.HistoricoBusca)
       .WithMany()
       .HasForeignKey(x => x.HistoricoBuscaId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);

    }
}