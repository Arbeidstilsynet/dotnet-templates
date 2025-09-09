using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ActionsController(ISakService sakService) : ControllerBase
{
    // POST actions/start-sak
    [HttpPost("start-sak")]
    public async Task<ActionResult<Sak>> StartSak([FromQuery(Name = "SakId")] [Required] Guid sakId)
    {
        var result = await sakService.StartSak(sakId);
        return Ok(result);
    }

    // POST actions/archive-sak
    [HttpPost("end-sak")]
    public async Task<ActionResult<Sak>> EndSak([FromQuery(Name = "SakId")] [Required] Guid sakId)
    {
        var result = await sakService.EndSak(sakId);
        return Ok(result);
    }

    // POST actions/archive-sak
    [HttpPost("archive-sak")]
    public async Task<ActionResult<Sak>> ArchiveSak(
        [FromQuery(Name = "SakId")] [Required] Guid sakId
    )
    {
        var result = await sakService.ArchiveSak(sakId);
        return Ok(result);
    }
}
