using System.Text.Json.Serialization;
using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Microsoft.OpenApi;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

internal static class OpenApiExtensions
{
    public static Microsoft.AspNetCore.OpenApi.OpenApiOptions ConfigureOpenApiSpec(
        this Microsoft.AspNetCore.OpenApi.OpenApiOptions openApiOptions
    )
    {
        return openApiOptions.ConfigureBasicOpenApiSpec(appName: IAssemblyInfo.AppName);
    }
}
