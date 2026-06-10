using MendMissionControl.Api.Models;

namespace MendMissionControl.Api.Services;

public interface IMendCreditService
{
    Task<MendCredit> EmitirCreditoAsync(int missaoRemocaoId, string cliente, decimal valorCredito);
}
