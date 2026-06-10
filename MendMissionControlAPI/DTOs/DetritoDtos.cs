namespace MendMissionControl.Api.DTOs;

public record CriarDetritoOrbitalDto(
    string CodigoCatalogo,
    double MassaKg,
    double TamanhoCm,
    double VelocidadeKmh,
    double AltitudeKm);

public record DetritoOrbitalResponseDto(
    int Id,
    string CodigoCatalogo,
    double MassaKg,
    double TamanhoCm,
    double VelocidadeKmh,
    double AltitudeKm,
    string NivelRisco,
    DateTime DataIdentificacao,
    bool Removido);
