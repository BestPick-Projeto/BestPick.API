using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestPick.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoAplicacao _produtoAplicacao;

    public ProdutoController(IProdutoAplicacao produtoAplicacao)
    {
        _produtoAplicacao = produtoAplicacao;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] ProdutoCriar produto)
    {
        try
        {
            var resultado = await _produtoAplicacao.AdicionarAsync(produto);

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
            var resultado = await _produtoAplicacao.BuscarPorIdAsync(id);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("historico/{historicoBuscaId}")]
    public async Task<IActionResult> BuscarPorHistoricoIdAsync([FromRoute] int historicoBuscaId)
    {
        try
        {
            var resultado = await _produtoAplicacao.BuscarPorHistoricoAsync(historicoBuscaId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}