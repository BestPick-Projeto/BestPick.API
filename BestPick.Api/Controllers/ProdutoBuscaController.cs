using BestPick.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace BestPick.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoBuscaController : ControllerBase
{
    private readonly ProdutoBuscaService _produtoBuscaService;

    public ProdutoBuscaController(
        ProdutoBuscaService produtoBuscaService)
    {
        _produtoBuscaService = produtoBuscaService;
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarAsync([FromQuery] string produto, [FromQuery] decimal valorCompra)
    {
        try
        {
            var resultado =
                await _produtoBuscaService.BuscarAsync(produto, valorCompra);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}