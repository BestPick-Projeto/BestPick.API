using System.Security.Claims;
using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterAsync()
    {
        try
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var resultado = await _usuarioAplicacao.ObterPorIdAsync(usuarioId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> AtualizarAsync(
        [FromBody] UsuarioAtualizar usuario)
    {
        try
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            await _usuarioAplicacao.AtualizarAsync(usuario, usuarioId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoverAsync()
    {
        try
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            await _usuarioAplicacao.RemoverAsync(usuarioId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}