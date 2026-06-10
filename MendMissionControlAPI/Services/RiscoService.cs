using MendMissionControl.Api.Enums;
using MendMissionControl.Api.Models;

namespace MendMissionControl.Api.Services;

public class RiscoService : IRiscoService
{
    public NivelRisco CalcularRisco(DetritoOrbital detrito)
    {
        if (detrito.VelocidadeKmh > 25000 && detrito.TamanhoCm > 50)
            return NivelRisco.Critico;

        if (detrito.VelocidadeKmh > 20000 && detrito.TamanhoCm > 20)
            return NivelRisco.Alto;

        if (detrito.TamanhoCm > 10 || detrito.MassaKg > 5)
            return NivelRisco.Medio;

        return NivelRisco.Baixo;
    }
}
