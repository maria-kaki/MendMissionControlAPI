namespace MendMissionControl.Api.Models;

public class Telemetria
{
    public int Id { get; set; }
    public int NaveLimpezaOrbitalId { get; set; }
    public NaveLimpezaOrbital NaveLimpezaOrbital { get; set; } = null!;
    public double PosicaoX { get; set; }
    public double PosicaoY { get; set; }
    public double VelocidadeKmh { get; set; }
    public double NivelBateria { get; set; }
    public double TemperaturaInterna { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public bool EstaCritica()
    {
        return NivelBateria < 15 || TemperaturaInterna > 85;
    }
}
