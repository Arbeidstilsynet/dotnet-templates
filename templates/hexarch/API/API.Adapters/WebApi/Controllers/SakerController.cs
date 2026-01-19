using Arbeidstilsynet.Common.FeatureFlags.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.WebApi.Controllers;

/// <summary>
/// Saker related endpoints.
/// </summary>
/// <param name="tilsynssakService"></param>
/// <param name="featureFlags"></param>
[Authorize]
[Route("[controller]")]
[ApiController]
public class SakerController(ITilsynssakService tilsynssakService, IFeatureFlags featureFlags) : ControllerBase
{
    /// <summary>
    /// Create a new Sak.
    /// </summary>
    /// <param name="tilsynssakDto"></param>
    /// <returns></returns>
    [HttpPost()]
    public async Task<ActionResult<Tilsynssak>> CreateSak([FromBody] CreateTilsynssakDto tilsynssakDto)
    {
        using var activity = Tracer.Source.StartActivity();
        var result = await tilsynssakService.CreateNewSak(tilsynssakDto);
        return Ok(result);
    }

    /// <summary>
    /// Get all Saker.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Tilsynssak>>> Get()
    {
        var saker = await tilsynssakService.GetAllSaker();
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
    public async Task<ActionResult<Tilsynssak>> GetById([FromRoute] Guid sakId)
    {
        return Ok(await tilsynssakService.GetSakById(sakId));
    }
}
