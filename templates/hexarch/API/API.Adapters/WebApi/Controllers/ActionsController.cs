using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.WebApi.Controllers;

/// <summary>
/// Actions related to Sak management
/// </summary>
/// <param name="tilsynssakService"></param>
[Authorize]
[Route("[controller]")]
[ApiController]
public class ActionsController(ITilsynssakService tilsynssakService) : ControllerBase
{
    /// <summary>
    /// Start a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("start-sak")]
    public async Task<ActionResult<Tilsynssak>> StartSak(
        [FromQuery(Name = "SakId")] [Required] Guid sakId
    )
    {
        var result = await tilsynssakService.StartSak(sakId);
        return Ok(result);
    }

    /// <summary>
    /// End a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("end-sak")]
    public async Task<ActionResult<Tilsynssak>> EndSak(
        [FromQuery(Name = "SakId")] [Required] Guid sakId
    )
    {
        var result = await tilsynssakService.EndSak(sakId);
        return Ok(result);
    }

    /// <summary>
    /// Archive a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("archive-sak")]
    public async Task<ActionResult<Tilsynssak>> ArchiveSak(
        [FromQuery(Name = "SakId")] [Required] Guid sakId
    )
    {
        var result = await tilsynssakService.ArchiveSak(sakId);
        return Ok(result);
    }
}
