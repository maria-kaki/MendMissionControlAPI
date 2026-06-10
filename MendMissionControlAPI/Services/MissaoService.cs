using MendMissionControl.Api.Data;
using MendMissionControl.Api.Enums;
using MendMissionControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Services;

public class MissaoService : IMissaoService
{
    private readonly AppDbContext _db;

    public MissaoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MissaoRemocao> CriarMissaoAsync(MissaoRemocao missao)
    {
        _db.MissoesRemocao.Add(missao);
        await _db.SaveChangesAsync();
        return missao;
    }

    public async Task<MissaoRemocao?> IniciarMissaoAsync(int missaoId)
    {
        var missao = await _db.MissoesRemocao.FindAsync(missaoId);
        if (missao == null) return null;

        if (missao.Status != StatusMissao.Planejada)
            throw new InvalidOperationException("Somente missões planejadas podem ser iniciadas.");

        missao.IniciarMissao();
        await _db.SaveChangesAsync();
        return missao;
    }

    public async Task<MissaoRemocao?> ConcluirMissaoAsync(int missaoId)
    {
        var missao = await _db.MissoesRemocao
            .Include(m => m.DetritoOrbital)
            .FirstOrDefaultAsync(m => m.Id == missaoId);

        if (missao == null) return null;

        if (missao.Status != StatusMissao.EmAndamento)
            throw new InvalidOperationException("Somente missões em andamento podem ser concluídas.");

        missao.ConcluirMissao();
        missao.DetritoOrbital.Removido = true;
        await _db.SaveChangesAsync();
        return missao;
    }

    public async Task<MissaoRemocao?> CancelarMissaoAsync(int missaoId, string motivo)
    {
        var missao = await _db.MissoesRemocao.FindAsync(missaoId);
        if (missao == null) return null;

        missao.CancelarMissao(motivo);
        await _db.SaveChangesAsync();
        return missao;
    }
}
