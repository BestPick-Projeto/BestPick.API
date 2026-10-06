namespace BestPick.Aplicacao.Models;

using System.ComponentModel.DataAnnotations;
public class Login
{
    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    public string Senha { get; set; }
}