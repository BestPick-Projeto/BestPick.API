using BestPick.Dominio.Entidades;
using BestPick.Aplicacao.Models;

namespace BestPick.Aplicacao.Interfaces;

public interface IUsuarioAplicacao
{
    Task<Usuario> AdicionarAsync(UsuarioCriar usuario);
    Task<IEnumerable<UsuarioRespostaDto>> ListarTodosAsync();
    Task RemoverAsync(int id);
    Task<UsuarioRespostaDto> ObterPorIdAsync(int id);
    Task AtualizarAsync(UsuarioAtualizar usuario);
    Task<LoginResposta> LoginAsync(Login login);
}