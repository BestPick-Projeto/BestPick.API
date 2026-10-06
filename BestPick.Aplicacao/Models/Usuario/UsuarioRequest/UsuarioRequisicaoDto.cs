namespace BestPick.Aplicacao.Models;

using System.ComponentModel.DataAnnotations;
public class UsuarioAtualizar
{
    public string Nome { get; set; }

    public string Email { get; set; }
}

public class UsuarioCriar
{
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    public string Senha { get; set; }
}