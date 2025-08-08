using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SakerController(ISakService _sakService) : ControllerBase
{
    // POST
    [HttpPost()]
    public async Task<ActionResult<Sak>> CreateSak([FromBody] CreateSakDto sakDto)
    {
        using var activity = Tracer.Source.StartActivity("received CreateSakRequest");
        var result = await _sakService.CreateNewSak(sakDto);
        return Ok(result);
    }

    // GET saker
    [HttpGet]
    public async Task<ActionResult<List<Sak>>> Get()
    {
        return Ok(await _sakService.GetAllSaker());
    }

    // GET saker/{sakId}
    [HttpGet("{sakId:guid}")]
    public async Task<ActionResult<Sak>> GetById([FromRoute] Guid sakId)
    {
        return Ok(await _sakService.GetSakById(sakId));
    }
}
