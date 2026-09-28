using BestPick.Dominio.Entidades;
using BestPick.Aplicacao.Models;

namespace BestPick.Aplicacao.Interfaces;

public interface IUsuarioAplicacao
{
    Task<Usuario> AdicionarAsync(UsuarioCriar usuario);
    Task<UsuarioRespostaDto> ObterPorIdAsync(int id);
    Task AtualizarAsync(UsuarioAtualizar usuario, int usuarioId);
    Task RemoverAsync(int id);
    Task<LoginResposta> LoginAsync(Login login);
}