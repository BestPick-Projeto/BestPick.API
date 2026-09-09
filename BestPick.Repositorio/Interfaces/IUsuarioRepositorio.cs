using BestPick.Dominio.Entidades;

namespace BestPick.Repositorio.Interfaces;

public interface IUsuarioRepositorio
{
    Task<Usuario> AdicionarAsync(Usuario usuario);
    Task<IEnumerable<Usuario>> ListarTodosAsync();
    Task RemoverAsync(int id);
    Task<Usuario> ObterPorIdAsync(int id);
    Task AtualizarAsync(Usuario usuario);
    Task<Usuario> ObterPorEmailAsync(string email);
}