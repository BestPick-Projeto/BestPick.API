using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BestPick.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class HistoricoController : ControllerBase
{
    private readonly IHistoricoAplicacao _historicoAplicacao;

    public HistoricoController(IHistoricoAplicacao historicoAplicacao)
    {
        _historicoAplicacao = historicoAplicacao;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] HistoricoCriar historicoBusca)
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var resultado = await _historicoAplicacao.AdicionarAsync(historicoBusca, usuarioId);

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

            var resultado = await _historicoAplicacao.ObterPorIdAsync(id, usuarioId);

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

            var resultado = await _historicoAplicacao.ListarPorUsuarioAsync(usuarioId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{historicoId}")]
    public async Task<IActionResult> RemoverAsync([FromRoute] int historicoId)
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _historicoAplicacao.RemoverAsync(historicoId, usuarioId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}