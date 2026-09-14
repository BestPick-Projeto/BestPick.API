using BestPick.Servicos.Models;
using BestPick.Servicos;
using Microsoft.AspNetCore.Mvc;
using BestPick.Api.Models;

namespace BestPick.Api;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    [Route("Mensagem")]
    public async Task<ActionResult> EnviarMensagem(
        [FromBody] ChatRequisicao dto)
    {
        try
        {
            if (dto == null)
                return BadRequest(new { erro = "DTO não recebido." });

            if (string.IsNullOrWhiteSpace(dto.Mensagem))
                return BadRequest(new { erro = "Mensagem não informada." });

            var resposta =
                await _chatService.EnviarMensagemAsync(
                    dto.Mensagem,
                    dto.Produto,
                    dto.ValorCompra,
                    dto.MotivoCompra
                );

            return Ok(resposta);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}