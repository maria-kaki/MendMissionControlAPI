using MendMissionControl.Api.Enums;

namespace MendMissionControl.Api.Models;

public class MissaoRemocao
{
    public int Id { get; set; }
    public string NomeMissao { get; set; } = string.Empty;
    public int DetritoOrbitalId { get; set; }
    public DetritoOrbital DetritoOrbital { get; set; } = null!;
    public int NaveLimpezaOrbitalId { get; set; }
    public NaveLimpezaOrbital NaveLimpezaOrbital { get; set; } = null!;
    public MetodoRemocao MetodoRemocao { get; set; }
    public StatusMissao Status { get; set; } = StatusMissao.Planejada;
    public DateTime DataPlanejada { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string? Observacao { get; set; }

    public void IniciarMissao()
    {
        Status = StatusMissao.EmAndamento;
        DataInicio = DateTime.UtcNow;
    }

    public void ConcluirMissao()
    {
        Status = StatusMissao.Concluida;
        DataConclusao = DateTime.UtcNow;
    }

    public void CancelarMissao(string motivo)
    {
        Status = StatusMissao.Cancelada;
        Observacao = motivo;
    }
}
