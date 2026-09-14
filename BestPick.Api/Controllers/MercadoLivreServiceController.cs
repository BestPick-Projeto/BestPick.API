using BestPick.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace BestPick.Api;

[ApiController]
[Route("[controller]")]
public class MercadoLivreServiceController : ControllerBase
{
    private readonly MercadoLivreService _mercadoLivreService;

    public MercadoLivreServiceController(MercadoLivreService mercadoLivreService)
    {
        _mercadoLivreService =
            mercadoLivreService;
    }

    [HttpGet]
    [Route("Buscar")]
    public async Task<ActionResult> Buscar(
        string produto,
        decimal valorMaximo)
    {
        try
        {
            var resultado =
                await _mercadoLivreService.BuscarProdutosAsync(
                    produto,
                    valorMaximo
                );

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new
                {
                    erro = ex.Message
                }
            );
        }
    }
}