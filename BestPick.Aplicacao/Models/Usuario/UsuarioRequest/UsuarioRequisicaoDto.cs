namespace BestPick.Aplicacao.Models;

public class UsuarioAtualizar
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

}

public class UsuarioCriar
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
}