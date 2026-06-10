namespace MendMissionControl.Api.DTOs;

public record CriarSateliteDto(
    string Nome,
    string CodigoIdentificacao,
    DateTime DataLancamento,
    string Operador,
    bool EstaAtivo);

public record CriarNaveLimpezaOrbitalDto(
    string Nome,
    string CodigoIdentificacao,
    DateTime DataLancamento,
    double CapacidadeCombustivel,
    bool PossuiLaserAblacao,
    bool PossuiGarrasCaptura);

public record EquipamentoResponseDto(
    int Id,
    string Nome,
    string CodigoIdentificacao,
    string TipoEquipamento,
    int TempoEmOperacaoAnos);
