using MendMissionControl.Api.Data;
using MendMissionControl.Api.DTOs;
using MendMissionControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Controllers;

[ApiController]
[Route("api/equipamentos")]
public class EquipamentosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<EquipamentosController> _logger;

    public EquipamentosController(AppDbContext db, ILogger<EquipamentosController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpPost("satelites")]
    public async Task<IActionResult> CriarSatelite([FromBody] CriarSateliteDto dto)
    {
        try
        {
            if (await _db.EquipamentosEspaciais.AnyAsync(e => e.CodigoIdentificacao == dto.CodigoIdentificacao))
                return Conflict("Já existe um equipamento com esse código de identificação.");

            var satelite = new Satelite
            {
                Nome = dto.Nome,
                CodigoIdentificacao = dto.CodigoIdentificacao,
                DataLancamento = dto.DataLancamento,
                Operador = dto.Operador,
                EstaAtivo = dto.EstaAtivo
            };

            _db.Satelites.Add(satelite);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Buscar), new { id = satelite.Id }, MapResponse(satelite));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao criar satélite.");
            return StatusCode(500, "Erro ao salvar o satélite no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar satélite.");
            return StatusCode(500, "Erro inesperado ao criar satélite.");
        }
    }

    [HttpPost("naves-limpeza")]
    public async Task<IActionResult> CriarNaveLimpeza([FromBody] CriarNaveLimpezaOrbitalDto dto)
    {
        try
        {
            if (await _db.EquipamentosEspaciais.AnyAsync(e => e.CodigoIdentificacao == dto.CodigoIdentificacao))
                return Conflict("Já existe um equipamento com esse código de identificação.");

            var nave = new NaveLimpezaOrbital
            {
                Nome = dto.Nome,
                CodigoIdentificacao = dto.CodigoIdentificacao,
                DataLancamento = dto.DataLancamento,
                CapacidadeCombustivel = dto.CapacidadeCombustivel,
                PossuiLaserAblacao = dto.PossuiLaserAblacao,
                PossuiGarrasCaptura = dto.PossuiGarrasCaptura
            };

            _db.NavesLimpezaOrbital.Add(nave);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Buscar), new { id = nave.Id }, MapResponse(nave));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao criar nave de limpeza orbital.");
            return StatusCode(500, "Erro ao salvar a nave no banco de dados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar nave.");
            return StatusCode(500, "Erro inesperado ao criar nave.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var equipamentos = await _db.EquipamentosEspaciais.ToListAsync();
        return Ok(equipamentos.Select(MapResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var equipamento = await _db.EquipamentosEspaciais.FindAsync(id);
        if (equipamento == null) return NotFound("Equipamento não encontrado.");
        return Ok(MapResponse(equipamento));
    }

    private static EquipamentoResponseDto MapResponse(EquipamentoEspacial e)
    {
        return new EquipamentoResponseDto(
            e.Id,
            e.Nome,
            e.CodigoIdentificacao,
            e.ObterTipoEquipamento(),
            e.CalcularTempoEmOperacao());
    }
}
