using MendMissionControl.Api.Data;
using MendMissionControl.Api.DTOs;
using MendMissionControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Controllers;

[ApiController]
[Route("api/telemetrias")]
public class TelemetriasController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<TelemetriasController> _logger;

    public TelemetriasController(AppDbContext db, ILogger<TelemetriasController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarTelemetriaDto dto)
    {
        try
        {
            var nave = await _db.NavesLimpezaOrbital.FindAsync(dto.NaveLimpezaOrbitalId);
            if (nave == null) return NotFound("Nave de limpeza orbital não encontrada.");

            if (dto.NivelBateria < 0 || dto.NivelBateria > 100)
                return BadRequest("O nível de bateria deve estar entre 0 e 100.");

            var telemetria = new Telemetria
            {
                NaveLimpezaOrbitalId = dto.NaveLimpezaOrbitalId,
                PosicaoX = dto.PosicaoX,
                PosicaoY = dto.PosicaoY,
                VelocidadeKmh = dto.VelocidadeKmh,
                NivelBateria = dto.NivelBateria,
                TemperaturaInterna = dto.TemperaturaInterna,
                Timestamp = dto.Timestamp ?? DateTime.UtcNow
            };

            _db.Telemetrias.Add(telemetria);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Buscar), new { id = telemetria.Id }, MapResponse(telemetria));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao registrar telemetria.");
            return StatusCode(500, "Erro ao salvar telemetria no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao registrar telemetria.");
            return StatusCode(500, "Erro inesperado ao registrar telemetria.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var telemetrias = await _db.Telemetrias.ToListAsync();
        return Ok(telemetrias.Select(MapResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var telemetria = await _db.Telemetrias.FindAsync(id);
        if (telemetria == null) return NotFound("Telemetria não encontrada.");
        return Ok(MapResponse(telemetria));
    }

    [HttpGet("nave/{naveId}")]
    public async Task<IActionResult> BuscarPorNave(int naveId)
    {
        var telemetrias = await _db.Telemetrias
            .Where(t => t.NaveLimpezaOrbitalId == naveId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();

        return Ok(telemetrias.Select(MapResponse));
    }

    [HttpGet("janela")]
    public async Task<IActionResult> BuscarPorJanela(DateTime inicio, DateTime fim)
    {
        if (inicio > fim)
            return BadRequest("A data inicial não pode ser maior que a data final.");

        var telemetrias = await _db.Telemetrias
            .Where(t => t.Timestamp >= inicio && t.Timestamp <= fim)
            .OrderBy(t => t.Timestamp)
            .ToListAsync();

        return Ok(telemetrias.Select(MapResponse));
    }

    private static TelemetriaResponseDto MapResponse(Telemetria t)
    {
        return new TelemetriaResponseDto(
            t.Id,
            t.NaveLimpezaOrbitalId,
            t.PosicaoX,
            t.PosicaoY,
            t.VelocidadeKmh,
            t.NivelBateria,
            t.TemperaturaInterna,
            t.Timestamp,
            t.EstaCritica());
    }
}
