using MendMissionControl.Api.Enums;

namespace MendMissionControl.Api.Models;

public class DetritoOrbital
{
    public int Id { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public double MassaKg { get; set; }
    public double TamanhoCm { get; set; }
    public double VelocidadeKmh { get; set; }
    public double AltitudeKm { get; set; }
    public NivelRisco NivelRisco { get; set; }
    public DateTime DataIdentificacao { get; set; } = DateTime.UtcNow;
    public bool Removido { get; set; }

    public ICollection<MissaoRemocao> Missoes { get; set; } = new List<MissaoRemocao>();
}
