using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Repositorios;
using BestPick.Dominio.Enumeradores;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BestPick.Aplicacao.Aplicacoes;

public class UsuarioAplicacao : IUsuarioAplicacao
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IConfiguration _configuration;

    public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio, IConfiguration configuration)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _configuration = configuration;
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

        obtido.Nome = usuario.Nome;
        obtido.Email = usuario.Email;

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
    public async Task<LoginResposta> LoginAsync(Login login)
    {
        var usuario = await _usuarioRepositorio.ObterPorEmailAsync(login.Email);

        if (usuario == null)
            throw new Exception("Email ou senha inválidos.");

        if (usuario.Senha != login.Senha)
            throw new Exception("Email ou senha inválidos.");

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Email, usuario.Email)
    };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credenciais
        );

        return new LoginResposta
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}