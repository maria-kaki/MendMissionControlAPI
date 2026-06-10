using MendMissionControl.Api.Data;
using MendMissionControl.Api.DTOs;
using MendMissionControl.Api.Enums;
using MendMissionControl.Api.Models;
using MendMissionControl.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Controllers;

[ApiController]
[Route("api/missoes")]
public class MissoesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMissaoService _missaoService;
    private readonly ILogger<MissoesController> _logger;

    public MissoesController(AppDbContext db, IMissaoService missaoService, ILogger<MissoesController> logger)
    {
        _db = db;
        _missaoService = missaoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarMissaoRemocaoDto dto)
    {
        try
        {
            var detrito = await _db.DetritosOrbitais.FindAsync(dto.DetritoOrbitalId);
            if (detrito == null) return NotFound("Detrito orbital não encontrado.");

            var nave = await _db.NavesLimpezaOrbital.FindAsync(dto.NaveLimpezaOrbitalId);
            if (nave == null) return NotFound("Nave de limpeza orbital não encontrada.");

            if (detrito.Removido)
                return BadRequest("Esse detrito já foi removido e não pode receber nova missão.");

            if (dto.DataPlanejada < DateTime.UtcNow.AddMinutes(-5))
                return BadRequest("A data planejada da missão não pode estar no passado.");

            var missao = new MissaoRemocao
            {
                NomeMissao = dto.NomeMissao,
                DetritoOrbitalId = dto.DetritoOrbitalId,
                NaveLimpezaOrbitalId = dto.NaveLimpezaOrbitalId,
                MetodoRemocao = dto.MetodoRemocao,
                DataPlanejada = dto.DataPlanejada,
                Status = StatusMissao.Planejada
            };

            await _missaoService.CriarMissaoAsync(missao);
            return CreatedAtAction(nameof(Buscar), new { id = missao.Id }, MapResponse(missao));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao criar missão de remoção.");
            return StatusCode(500, "Erro ao salvar a missão no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar missão.");
            return StatusCode(500, "Erro inesperado ao criar missão.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var missoes = await _db.MissoesRemocao.ToListAsync();
        return Ok(missoes.Select(MapResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var missao = await _db.MissoesRemocao.FindAsync(id);
        if (missao == null) return NotFound("Missão não encontrada.");
        return Ok(MapResponse(missao));
    }

    [HttpPut("{id}/iniciar")]
    public async Task<IActionResult> Iniciar(int id)
    {
        try
        {
            var missao = await _missaoService.IniciarMissaoAsync(id);
            if (missao == null) return NotFound("Missão não encontrada.");
            return Ok(MapResponse(missao));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar missão.");
            return StatusCode(500, "Erro inesperado ao iniciar missão.");
        }
    }

    [HttpPut("{id}/concluir")]
    public async Task<IActionResult> Concluir(int id)
    {
        try
        {
            var missao = await _missaoService.ConcluirMissaoAsync(id);
            if (missao == null) return NotFound("Missão não encontrada.");
            return Ok(MapResponse(missao));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao concluir missão.");
            return StatusCode(500, "Erro inesperado ao concluir missão.");
        }
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromBody] CancelarMissaoDto dto)
    {
        try
        {
            var missao = await _missaoService.CancelarMissaoAsync(id, dto.Motivo);
            if (missao == null) return NotFound("Missão não encontrada.");
            return Ok(MapResponse(missao));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar missão.");
            return StatusCode(500, "Erro inesperado ao cancelar missão.");
        }
    }

    private static MissaoRemocaoResponseDto MapResponse(MissaoRemocao m)
    {
        return new MissaoRemocaoResponseDto(
            m.Id,
            m.NomeMissao,
            m.DetritoOrbitalId,
            m.NaveLimpezaOrbitalId,
            m.MetodoRemocao.ToString(),
            m.Status.ToString(),
            m.DataPlanejada,
            m.DataInicio,
            m.DataConclusao,
            m.Observacao);
    }
}
