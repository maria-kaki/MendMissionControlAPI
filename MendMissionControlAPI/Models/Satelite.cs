namespace MendMissionControl.Api.Models;

public class Satelite : EquipamentoEspacial
{
    public string Operador { get; set; } = string.Empty;
    public bool EstaAtivo { get; set; }

    public override string ObterTipoEquipamento()
    {
        return "Satélite";
    }
}
