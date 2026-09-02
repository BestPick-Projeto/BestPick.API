using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Repositorios;

namespace BestPick.Aplicacao.Aplicacoes;

public class FavoritoAplicacao : IFavoritoAplicacao
{
    private readonly IFavoritoRepositorio _favoritoRepositorio;
    public FavoritoAplicacao(IFavoritoRepositorio favortioRepositorio)
    {
        _favoritoRepositorio = favortioRepositorio;
    }

    public async Task<Favorito> AdicionarAsync(Favorito favorito)
    {
        if (favorito == null)
            throw new Exception("O favorito não pode ser vazio!");


        return await _favoritoRepositorio.AdicionarAsync(favorito);

    }
    public async Task<Favorito> ObterPorIdAsync(int id)
    {
        var obtido = await _favoritoRepositorio.ObterPorIdAsync(id);

        if (obtido == null)
            throw new Exception("Id não encontrado!");

        return obtido;
    }
    public async Task<IEnumerable<Favorito>> ListarPorUsuarioAsync(int usuarioId)
    {
        var obtidos = await _favoritoRepositorio.ListarPorUsuarioAsync(usuarioId);

        if (obtidos == null)
            throw new Exception("Usuário não possui favoritos!");

        return obtidos;
    }
    public async Task AtualizarStatusAsync(Favorito favorito)
    {
        var obtido = await _favoritoRepositorio.ObterPorIdAsync(favorito.Id);

        if (obtido == null)
            throw new Exception("Id de favorito não existe!");

        obtido.Status = favorito.Status;
        await _favoritoRepositorio.AtualizarStatusAsync(obtido);
    }
    public async Task RemoverAsync(int id)
    {
        var obtido = await _favoritoRepositorio.ObterPorIdAsync(id);
        
        if(obtido == null)
            throw new Exception("Id de favorito não existe!");

        await _favoritoRepositorio.RemoverAsync(obtido.Id);
    }
}

