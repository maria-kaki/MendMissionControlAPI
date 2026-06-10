namespace MendMissionControl.Api.DTOs;

public record EmitirMendCreditDto(
    int MissaoRemocaoId,
    string Cliente,
    decimal ValorCredito);

public record MendCreditResponseDto(
    int Id,
    int MissaoRemocaoId,
    string Cliente,
    decimal ValorCredito,
    string Status,
    DateTime DataEmissao);
