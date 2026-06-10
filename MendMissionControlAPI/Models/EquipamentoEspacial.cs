namespace MendMissionControl.Api.Models;

public abstract class EquipamentoEspacial
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoIdentificacao { get; set; } = string.Empty;
    public DateTime DataLancamento { get; set; }

    public abstract string ObterTipoEquipamento();

    public int CalcularTempoEmOperacao()
    {
        var hoje = DateTime.UtcNow;
        var anos = hoje.Year - DataLancamento.Year;

        if (DataLancamento.Date > hoje.AddYears(-anos).Date)
            anos--;

        return anos < 0 ? 0 : anos;
    }
}
