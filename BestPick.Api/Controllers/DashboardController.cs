using BestPick.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BestPick.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardAplicacao _dashboardAplicacao;

    public DashboardController(IDashboardAplicacao dashboardAplicacao)
    {
        _dashboardAplicacao = dashboardAplicacao;
    }

    [HttpGet]
    public async Task<ActionResult> Obter()
    {
        try
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var resultado = await _dashboardAplicacao.ObterAsync(usuarioId);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}