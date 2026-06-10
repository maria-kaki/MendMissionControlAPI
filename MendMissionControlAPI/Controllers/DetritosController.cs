using MendMissionControl.Api.Data;
using MendMissionControl.Api.DTOs;
using MendMissionControl.Api.Models;
using MendMissionControl.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Controllers;

[ApiController]
[Route("api/detritos")]
public class DetritosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IRiscoService _riscoService;
    private readonly ILogger<DetritosController> _logger;

    public DetritosController(AppDbContext db, IRiscoService riscoService, ILogger<DetritosController> logger)
    {
        _db = db;
        _riscoService = riscoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarDetritoOrbitalDto dto)
    {
        try
        {
            if (dto.MassaKg <= 0 || dto.TamanhoCm <= 0 || dto.VelocidadeKmh <= 0 || dto.AltitudeKm <= 0)
                return BadRequest("Massa, tamanho, velocidade e altitude devem ser maiores que zero.");

            if (await _db.DetritosOrbitais.AnyAsync(d => d.CodigoCatalogo == dto.CodigoCatalogo))
                return Conflict("Já existe um detrito cadastrado com esse código de catálogo.");

            var detrito = new DetritoOrbital
            {
                CodigoCatalogo = dto.CodigoCatalogo,
                MassaKg = dto.MassaKg,
                TamanhoCm = dto.TamanhoCm,
                VelocidadeKmh = dto.VelocidadeKmh,
                AltitudeKm = dto.AltitudeKm,
                DataIdentificacao = DateTime.UtcNow
            };

            detrito.NivelRisco = _riscoService.CalcularRisco(detrito);

            _db.DetritosOrbitais.Add(detrito);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Buscar), new { id = detrito.Id }, MapResponse(detrito));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao criar detrito orbital.");
            return StatusCode(500, "Erro ao salvar o detrito no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar detrito.");
            return StatusCode(500, "Erro inesperado ao criar detrito.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var detritos = await _db.DetritosOrbitais.ToListAsync();
        return Ok(detritos.Select(MapResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var detrito = await _db.DetritosOrbitais.FindAsync(id);
        if (detrito == null) return NotFound("Detrito orbital não encontrado.");
        return Ok(MapResponse(detrito));
    }

    [HttpGet("risco/{nivel}")]
    public async Task<IActionResult> BuscarPorRisco(string nivel)
    {
        var detritos = await _db.DetritosOrbitais
            .Where(d => d.NivelRisco.ToString().ToLower() == nivel.ToLower())
            .ToListAsync();

        return Ok(detritos.Select(MapResponse));
    }

    private static DetritoOrbitalResponseDto MapResponse(DetritoOrbital d)
    {
        return new DetritoOrbitalResponseDto(
            d.Id,
            d.CodigoCatalogo,
            d.MassaKg,
            d.TamanhoCm,
            d.VelocidadeKmh,
            d.AltitudeKm,
            d.NivelRisco.ToString(),
            d.DataIdentificacao,
            d.Removido);
    }
}
