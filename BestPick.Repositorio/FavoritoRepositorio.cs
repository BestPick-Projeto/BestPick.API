using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using BestPick.Dominio.Enumeradores;
using BestPick.Repositorio.Queries;
using Microsoft.Data.SqlClient;

namespace BestPick.Repositorio.Repositorios;

public class FavoritoRepositorio : IFavoritoRepositorio
{
    private readonly BestPickContexto contexto;
    public FavoritoRepositorio(BestPickContexto bestPickContexto)
    {
        contexto = bestPickContexto;
    }

    public async Task<Favorito> AdicionarAsync(Favorito favorito)
    {
        await contexto.Favoritos.AddAsync(favorito);
        await contexto.SaveChangesAsync();

        return favorito;
    }

    public async Task<Favorito> ObterPorIdAsync(int id, int usuarioId)
    {
        return await contexto.Favoritos
            .Include(x => x.Produto)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UsuarioId == usuarioId &&
                x.Ativo);
    }

    public async Task RemoverAsync(int id)
    {
        var obtido = await contexto.Favoritos
            .FirstOrDefaultAsync(x => x.Id == id && x.Ativo);

        if (obtido == null)
            throw new Exception("Favorito não encontrado.");

        obtido.Ativo = false;

        await contexto.SaveChangesAsync();
    }

    public async Task AtualizarStatusAsync(Favorito favorito)
    {
        contexto.Update(favorito);
        await contexto.SaveChangesAsync();
    }

    public async Task<IEnumerable<Favorito>> ListarPorUsuarioAsync(int usuarioId)
    {
        var parametro = new SqlParameter("@UsuarioId", usuarioId);

        var obtidos = await contexto.Favoritos.FromSqlRaw
        (FavoritoQueries.ListarPorUsuario, parametro)
        .ToListAsync();

        if (obtidos == null)
            throw new Exception("Usuário não possui favoritos!");

        foreach (var favorito in obtidos)
        {
            await contexto.Entry(favorito)
                .Reference(f => f.Produto)
                .LoadAsync();
        }

        return obtidos;
    }
}