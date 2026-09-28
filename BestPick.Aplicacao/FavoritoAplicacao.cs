using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
using BestPick.Dominio.Enumeradores;

namespace BestPick.Aplicacao.Aplicacoes;

public class FavoritoAplicacao : IFavoritoAplicacao
{
    private readonly IFavoritoRepositorio _favoritoRepositorio;
    public FavoritoAplicacao(IFavoritoRepositorio favoritorepositorio)
    {
        _favoritoRepositorio = favoritorepositorio;
    }
    private static FavoritoRespostaDto ParaDto(Favorito favorito)
    {
        return new FavoritoRespostaDto
        {
            Id = favorito.Id,
            NomeProduto = favorito.Produto.Nome,
            Categoria = favorito.Produto.Categoria,
            Preco = favorito.Produto.Preco,
            ImgUrl = favorito.Produto.ImgUrl,
            Url = favorito.Produto.Url,
            Beneficios = favorito.Produto.Beneficios,
            Status = favorito.Status.ToString()
        };
    }

    public async Task<Favorito> AdicionarAsync(FavoritoCriar favorito, int usuarioId)
    {
        if (favorito == null)
            throw new Exception("Dados do favorito são obrigatórios.");

        var favoritoDominio = new Favorito
        {
            ProdutoId = favorito.ProdutoId,
            UsuarioId = usuarioId,
            Status = StatusFavorito.Favoritado,
            Ativo = true
        };

        return await _favoritoRepositorio.AdicionarAsync(favoritoDominio);
    }
    public async Task<FavoritoRespostaDto> ObterPorIdAsync(int id, int usuarioId)
    {
        var obtido = await _favoritoRepositorio.ObterPorIdAsync(id, usuarioId);

        if (obtido == null)
            throw new Exception("Id não encontrado!");

        return ParaDto(obtido);
    }
    public async Task<IEnumerable<FavoritoRespostaDto>> ListarPorUsuarioAsync(int usuarioId)
    {
        var obtidos = await _favoritoRepositorio.ListarPorUsuarioAsync(usuarioId);

        if (obtidos == null)
            throw new Exception("Usuário não possui favoritos!");

        return obtidos.Select(ParaDto);
    }
    public async Task AtualizarStatusAsync(FavoritoAtualizar favorito, int usuarioId)
    {

        var obtido = await _favoritoRepositorio.ObterPorIdAsync(favorito.FavoritoId, usuarioId);

        if (obtido == null)
            throw new Exception("Id de favorito não existe!");

        obtido.Status = favorito.NovoStatus;
        await _favoritoRepositorio.AtualizarStatusAsync(obtido);
    }
    public async Task RemoverAsync(int id, int usuarioId)
    {
        var obtido = await _favoritoRepositorio.ObterPorIdAsync(id, usuarioId);

        if (obtido == null)
            throw new Exception("Id de favorito não existe!");

        await _favoritoRepositorio.RemoverAsync(obtido.Id);
    }
}

