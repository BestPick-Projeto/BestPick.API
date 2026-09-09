using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BestPick.Repositorio.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly BestPickContexto contexto;
    public UsuarioRepositorio(BestPickContexto bestPickContexto)
    {
        contexto = bestPickContexto;
    }

    public async Task<Usuario> ObterPorIdAsync(int id)
    {
        return await contexto.Usuarios
        .Where(f => f.Id == id)
        .Where(f => f.Ativo == true)
        .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Usuario>> ListarTodosAsync()
    {
        return await contexto.Usuarios
        .Where(f => f.Ativo == true)
        .ToListAsync();
    }
    public async Task<Usuario> AdicionarAsync(Usuario usuario)
    {
        await contexto.AddAsync(usuario);
        await contexto.SaveChangesAsync();
        return usuario;
    }
    public async Task RemoverAsync(int id)
    {
        var obtido = await ObterPorIdAsync(id);
        obtido.Ativo = false;
        await contexto.SaveChangesAsync();
    }
    public async Task AtualizarAsync(Usuario usuario)
    {
        contexto.Usuarios.Update(usuario);
        await contexto.SaveChangesAsync();
    }
    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await contexto.Usuarios
            .FirstOrDefaultAsync(x => x.Email == email && x.Ativo);
    }
}