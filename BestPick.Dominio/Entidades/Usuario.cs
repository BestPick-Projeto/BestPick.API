namespace BestPick.Dominio.Entidades;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set; }
      public Usuario()
    {
        Ativo = true;
    }
}