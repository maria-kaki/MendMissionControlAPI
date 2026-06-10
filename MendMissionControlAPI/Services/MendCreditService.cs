using MendMissionControl.Api.Data;
using MendMissionControl.Api.Enums;
using MendMissionControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MendMissionControl.Api.Services;

public class MendCreditService : IMendCreditService
{
    private readonly AppDbContext _db;

    public MendCreditService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MendCredit> EmitirCreditoAsync(int missaoRemocaoId, string cliente, decimal valorCredito)
    {
        var missao = await _db.MissoesRemocao.FindAsync(missaoRemocaoId);

        if (missao == null)
            throw new KeyNotFoundException("Missão de remoção não encontrada.");

        if (missao.Status != StatusMissao.Concluida)
            throw new InvalidOperationException("Só é possível emitir Mend Credit para missão concluída.");

        var creditoJaEmitido = await _db.MendCredits.AnyAsync(c => c.MissaoRemocaoId == missaoRemocaoId);

        if (creditoJaEmitido)
            throw new InvalidOperationException("Já existe um Mend Credit emitido para essa missão.");

        var credito = new MendCredit
        {
            MissaoRemocaoId = missaoRemocaoId,
            Cliente = cliente,
            ValorCredito = valorCredito,
            Status = StatusCredito.Emitido,
            DataEmissao = DateTime.UtcNow
        };

        _db.MendCredits.Add(credito);
        await _db.SaveChangesAsync();
        return credito;
    }
}
