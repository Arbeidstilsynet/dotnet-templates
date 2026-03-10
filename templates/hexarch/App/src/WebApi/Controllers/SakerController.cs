using Arbeidstilsynet.Common.FeatureFlags.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.WebApi.Controllers;

/// <summary>
/// Saker related endpoints.
/// </summary>
/// <param name="sakService"></param>
/// <param name="featureFlags"></param>
[Authorize]
[Route("[controller]")]
[ApiController]
public class SakerController(ISakService sakService, IFeatureFlags featureFlags) : ControllerBase
{
    /// <summary>
    /// Create a new Sak.
    /// </summary>
    /// <param name="sakDto"></param>
    /// <returns></returns>
    [HttpPost()]
    public async Task<ActionResult<Sak>> CreateSak([FromBody] CreateSakDto sakDto)
    {
        using var activity = Tracer.Source.StartActivity();
        var result = await sakService.CreateNewSak(sakDto);
        return Ok(result);
    }

    /// <summary>
    /// Get all Saker.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Sak>>> Get()
    {
        var saker = await sakService.GetAllSaker();
        if (featureFlags.IsEnabled("newestFirst"))
        {
            saker = saker.OrderByDescending(s => s.CreatedAt);
        }
        return Ok(saker);
    }

    /// <summary>
    /// Get a Sak by Id.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    [HttpGet("{sakId:guid}")]
    public async Task<ActionResult<Sak>> GetById([FromRoute] Guid sakId)
    {
        return Ok(await sakService.GetSakById(sakId));
    }
}
