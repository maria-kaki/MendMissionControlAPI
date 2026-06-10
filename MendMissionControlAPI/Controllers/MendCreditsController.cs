using MendMissionControl.Api.Data;
using MendMissionControl.Api.DTOs;
using MendMissionControl.Api.Models;
using MendMissionControl.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Controllers;

[ApiController]
[Route("api/mendcredits")]
public class MendCreditsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMendCreditService _creditService;
    private readonly ILogger<MendCreditsController> _logger;

    public MendCreditsController(AppDbContext db, IMendCreditService creditService, ILogger<MendCreditsController> logger)
    {
        _db = db;
        _creditService = creditService;
        _logger = logger;
    }

    [HttpPost("emitir")]
    public async Task<IActionResult> Emitir([FromBody] EmitirMendCreditDto dto)
    {
        try
        {
            if (dto.ValorCredito <= 0)
                return BadRequest("O valor do crédito deve ser maior que zero.");

            var credito = await _creditService.EmitirCreditoAsync(dto.MissaoRemocaoId, dto.Cliente, dto.ValorCredito);
            return CreatedAtAction(nameof(Buscar), new { id = credito.Id }, MapResponse(credito));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao emitir Mend Credit.");
            return StatusCode(500, "Erro ao salvar Mend Credit no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao emitir Mend Credit.");
            return StatusCode(500, "Erro inesperado ao emitir Mend Credit.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var creditos = await _db.MendCredits.ToListAsync();
        return Ok(creditos.Select(MapResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var credito = await _db.MendCredits.FindAsync(id);
        if (credito == null) return NotFound("Mend Credit não encontrado.");
        return Ok(MapResponse(credito));
    }

    private static MendCreditResponseDto MapResponse(MendCredit c)
    {
        return new MendCreditResponseDto(
            c.Id,
            c.MissaoRemocaoId,
            c.Cliente,
            c.ValorCredito,
            c.Status.ToString(),
            c.DataEmissao);
    }
}
