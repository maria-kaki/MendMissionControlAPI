namespace MendMissionControl.Api.DTOs;

public record RegistrarTelemetriaDto(
    int NaveLimpezaOrbitalId,
    double PosicaoX,
    double PosicaoY,
    double VelocidadeKmh,
    double NivelBateria,
    double TemperaturaInterna,
    DateTime? Timestamp);

public record TelemetriaResponseDto(
    int Id,
    int NaveLimpezaOrbitalId,
    double PosicaoX,
    double PosicaoY,
    double VelocidadeKmh,
    double NivelBateria,
    double TemperaturaInterna,
    DateTime Timestamp,
    bool Critica);
