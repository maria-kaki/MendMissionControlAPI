namespace MendMissionControl.Api.Models;

public class NaveLimpezaOrbital : EquipamentoEspacial
{
    public double CapacidadeCombustivel { get; set; }
    public bool PossuiLaserAblacao { get; set; }
    public bool PossuiGarrasCaptura { get; set; }

    public override string ObterTipoEquipamento()
    {
        return "Nave de Limpeza Orbital";
    }
}
