using MendMissionControl.Api.Models;

namespace MendMissionControl.Api.Services;

public interface IMissaoService
{
    Task<MissaoRemocao> CriarMissaoAsync(MissaoRemocao missao);
    Task<MissaoRemocao?> IniciarMissaoAsync(int missaoId);
    Task<MissaoRemocao?> ConcluirMissaoAsync(int missaoId);
    Task<MissaoRemocao?> CancelarMissaoAsync(int missaoId, string motivo);
}
