using System.Threading.Tasks;
using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BestPick.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAplicacao _usuarioAplicacao;

    public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
    {
        _usuarioAplicacao = usuarioAplicacao;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] UsuarioCriar usuario)
    {
        try
        {
            var resultado = await _usuarioAplicacao.AdicionarAsync(usuario);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorIdAsync([FromRoute] int id)
    {
        try
        {
            var resultado = await _usuarioAplicacao.ObterPorIdAsync(id);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosAsyncAsync()
    {
        try
        {
            var resultado = await _usuarioAplicacao.ListarTodosAsync();

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverAsync([FromRoute] int id)
    {
        try
        {
            await _usuarioAplicacao.RemoverAsync(id);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarAsync([FromBody] UsuarioAtualizar usuario)
    {
        try
        {
            await _usuarioAplicacao.AtualizarAsync(usuario);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Login login)
    {
        try
        {
            var resultado = await _usuarioAplicacao.LoginAsync(login);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}