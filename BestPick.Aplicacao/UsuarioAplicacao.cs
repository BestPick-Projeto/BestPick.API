using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Repositorios;
using BestPick.Dominio.Enumeradores;

namespace BestPick.Aplicacao.Aplicacoes;

public class UsuarioAplicacao : IUsuarioAplicacao
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    private static UsuarioRespostaDto ParaDto(Usuario usuario)
    {
        return new UsuarioRespostaDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task<Usuario> AdicionarAsync(UsuarioCriar usuario)
    {
        if (usuario == null)
            throw new Exception("Usuário obrigátorio!");

        var usuarioDominio = new Usuario
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = usuario.Senha
        };

        return await _usuarioRepositorio.AdicionarAsync(usuarioDominio);
    }

    public async Task<UsuarioRespostaDto> ObterPorIdAsync(int id)
    {
        var obtido = await _usuarioRepositorio.ObterPorIdAsync(id);

        if (obtido == null)
            throw new Exception("Usuário não existe!");

        return ParaDto(obtido);
    }

    public async Task<IEnumerable<UsuarioRespostaDto>> ListarTodosAsync()
    {
        var obtidos = await _usuarioRepositorio.ListarTodosAsync();

        if (obtidos == null)
            throw new Exception("Usuário não existe!");

        return obtidos.Select(ParaDto);
    }

    public async Task AtualizarAsync(UsuarioAtualizar usuario)
    {
        var obtido = await _usuarioRepositorio.ObterPorIdAsync(usuario.Id);

        if (obtido == null)
            throw new Exception("Usuário não existe!");

        obtido = new Usuario()
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
        };

        await _usuarioRepositorio.AtualizarAsync(obtido);
    }

    public async Task RemoverAsync(int id)
    {
        var obtido = await _usuarioRepositorio.ObterPorIdAsync(id);

        if (obtido == null)
            throw new Exception("Usuário não existe!");

        obtido.Ativo = false;
        await _usuarioRepositorio.AtualizarAsync(obtido);
    }
}