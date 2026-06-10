using MendMissionControl.Api.Enums;
using MendMissionControl.Api.Models;

namespace MendMissionControl.Api.Services;

public interface IRiscoService
{
    NivelRisco CalcularRisco(DetritoOrbital detrito);
}
