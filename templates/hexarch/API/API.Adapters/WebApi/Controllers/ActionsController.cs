using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.WebApi.Controllers;


/// <summary>
/// Actions related to Sak management
/// </summary>
/// <param name="sakService"></param>
[Route("[controller]")]
[ApiController]
public class ActionsController(ISakService sakService) : ControllerBase
{
    /// <summary>
    /// Start a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("start-sak")]
    public async Task<ActionResult<Sak>> StartSak([FromQuery(Name = "SakId")] [Required] Guid sakId)
    {
        var result = await sakService.StartSak(sakId);
        return Ok(result);
    }

    /// <summary>
    /// End a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("end-sak")]
    public async Task<ActionResult<Sak>> EndSak([FromQuery(Name = "SakId")] [Required] Guid sakId)
    {
        var result = await sakService.EndSak(sakId);
        return Ok(result);
    }

    /// <summary>
    /// Archive a Sak
    /// </summary>
    /// <param name="sakId">Id of the sak</param>
    /// <returns></returns>
    [HttpPost("archive-sak")]
    public async Task<ActionResult<Sak>> ArchiveSak(
        [FromQuery(Name = "SakId")] [Required] Guid sakId
    )
    {
        var result = await sakService.ArchiveSak(sakId);
        return Ok(result);
    }
}
