using MendMissionControl.Api.Enums;

namespace MendMissionControl.Api.Models;

public class MendCredit
{
    public int Id { get; set; }
    public int MissaoRemocaoId { get; set; }
    public MissaoRemocao MissaoRemocao { get; set; } = null!;
    public string Cliente { get; set; } = string.Empty;
    public decimal ValorCredito { get; set; }
    public StatusCredito Status { get; set; } = StatusCredito.Emitido;
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
}
