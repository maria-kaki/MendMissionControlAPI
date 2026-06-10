using MendMissionControl.Api.Enums;

namespace MendMissionControl.Api.DTOs;

public record CriarMissaoRemocaoDto(
    string NomeMissao,
    int DetritoOrbitalId,
    int NaveLimpezaOrbitalId,
    MetodoRemocao MetodoRemocao,
    DateTime DataPlanejada);

public record CancelarMissaoDto(string Motivo);

public record MissaoRemocaoResponseDto(
    int Id,
    string NomeMissao,
    int DetritoOrbitalId,
    int NaveLimpezaOrbitalId,
    string MetodoRemocao,
    string Status,
    DateTime DataPlanejada,
    DateTime? DataInicio,
    DateTime? DataConclusao,
    string? Observacao);
