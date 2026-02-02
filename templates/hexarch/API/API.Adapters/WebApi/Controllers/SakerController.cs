using Arbeidstilsynet.Common.FeatureFlags.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.WebApi.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class SakerController(IProcessSakEvents sakService, IFeatureFlags featureFlags) : ControllerBase
{
    // POST
    [HttpPost()]
    public async Task<ActionResult<Sak>> CreateSak([FromBody] CreateSakDto sakDto)
    {
        using var activity = Tracer.Source.StartActivity();
        var result = await sakService.CreateNewSak(sakDto);
        return Ok(result);
    }

    // GET saker
    [HttpGet]
    public async Task<ActionResult<List<Sak>>> Get()
    {
        var saker = await sakService.GetAllSaker();
        if (featureFlags.IsEnabled("newestFirst"))
        {
            saker = saker.OrderByDescending(s => s.CreatedAt).ToList();
        }
        return Ok(saker);
    }

    // GET saker/{sakId}
    [HttpGet("{sakId:guid}")]
    public async Task<ActionResult<Sak>> GetById([FromRoute] Guid sakId)
    {
        return Ok(await sakService.GetSakById(sakId));
    }
}
