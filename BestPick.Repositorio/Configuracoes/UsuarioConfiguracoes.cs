using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestPick.Repositorio.Configuracoes;

public class UsuarioConfiguracoes : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario").HasKey(x => x.Id);

        builder.Property(nameof(Usuario.Id)).HasColumnName("UsuarioId");
        builder.Property(nameof(Usuario.Nome)).HasColumnName("Nome");
        builder.Property(nameof(Usuario.Email)).HasColumnName("Email");
        builder.Property(nameof(Usuario.Ativo)).HasColumnName("Ativo");
        builder.Property(nameof(Usuario.Senha)).HasColumnName("Senha");

    }
}