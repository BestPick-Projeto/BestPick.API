using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BestPick.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FavoritoController : ControllerBase
{
    private readonly IFavoritoAplicacao _favoritoAplicacao;

    public FavoritoController(IFavoritoAplicacao favoritoAplicacao)
    {
        _favoritoAplicacao = favoritoAplicacao;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] FavoritoCriar favorito)
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var resultado = await _favoritoAplicacao.AdicionarAsync(favorito, usuarioId);

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
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var resultado = await _favoritoAplicacao.ObterPorIdAsync(id, usuarioId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorUsuarioAsync()
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var resultado = await _favoritoAplicacao.ListarPorUsuarioAsync(usuarioId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarStatusAsync([FromBody] FavoritoAtualizar favorito)
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _favoritoAplicacao.AtualizarStatusAsync(favorito, usuarioId);

            return Ok();
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
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            await _favoritoAplicacao.RemoverAsync(id, usuarioId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}