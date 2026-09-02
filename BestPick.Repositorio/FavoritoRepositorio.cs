using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using BestPick.Dominio.Enumeradores;
using BestPick.Repositorio.Queries;

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

    public async Task<Favorito> ObterPorIdAsync(int favoritoid)
    {
        return await contexto.Favoritos
        .Where(f => f.Id == favoritoid)
        .Where(f => f.Ativo)
        .FirstOrDefaultAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var obtido = await ObterPorIdAsync(id);
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
        var obtidos = await contexto.Favoritos
            .FromSqlInterpolated($"{FavoritoQueries.ListarPorUsuario} {usuarioId}")
            .Include(f => f.Produto)
            .ToListAsync();
        if(obtidos == null)
            throw new Exception("Usuário não possui favoritos!");

        return obtidos;
    }
}