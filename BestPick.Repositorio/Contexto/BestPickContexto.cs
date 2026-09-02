using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Configuracoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class BestPickContexto : DbContext
{
    private readonly DbContextOptions _options;

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }
    public DbSet<HistoricoBusca> Historicos { get; set; }

    public BestPickContexto(DbContextOptions options) : base(options)
    {
        _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_options == null)
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-58UEHOH\SQLEXPRESS;Database=BestPick;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProdutoConfiguracao());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
        modelBuilder.ApplyConfiguration(new FavoritoConfiguracoes());
        modelBuilder.ApplyConfiguration(new HistoricoBuscaConfiguracoes());
    }
}