using System.Text.Json.Serialization;
using Microsoft.OpenApi;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

internal static class OpenApiExtensions
{
    public static Microsoft.AspNetCore.OpenApi.OpenApiOptions ConfigureOpenApiSpec(
        this Microsoft.AspNetCore.OpenApi.OpenApiOptions openApiOptions
    )
    {
        return openApiOptions
            .AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    var appName = IAssemblyInfo.AppName;
                    document.Info = new OpenApiInfo
                    {
                        Title = $"{appName} API",
                        Version = "v1",
                        Description = $"Common entrypoints to interact with {appName}.",
                    };

                    return Task.CompletedTask;
                }
            )
            .AddSchemaTransformer(
                (schema, context, ct) =>
                {
                    schema.EnumsAsStringUnions();

                    return Task.CompletedTask;
                }
            );
    }

    public static Microsoft.AspNetCore.OpenApi.OpenApiOptions ConfigureAuthSpec(
        this Microsoft.AspNetCore.OpenApi.OpenApiOptions openApiOptions,
        AuthConfiguration authConfiguration
    )
    {
        return openApiOptions.AddDocumentTransformer(
            (document, context, cancellationToken) =>
            {
                if (!authConfiguration.DangerousDisableAuth)
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                    };
                    document.Components.SecuritySchemes["OAuth2"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            ClientCredentials = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri(
                                    $"https://login.microsoftonline.com/{authConfiguration.EntraTenantId}/oauth2/v2.0/token"
                                ),
                                Scopes = new Dictionary<string, string>
                                {
                                    { authConfiguration.EntraScope, "Access API" },
                                },
                            },
                        },
                    };
                }
                return Task.CompletedTask;
            }
        );
    }
}

file static class Extensions
{
    /// <summary>
    /// Treat enums as string unions.
    /// </summary>
    /// <param name="schema"></param>
    /// <remarks>Requires <see cref="JsonStringEnumConverter"/> to work</remarks>
    /// <returns></returns>
    public static OpenApiSchema EnumsAsStringUnions(this OpenApiSchema schema)
    {
        if (schema.Enum is { Count: > 0 })
        {
            for (var i = schema.Enum.Count - 1; i >= 0; i--)
            {
                var value = schema.Enum[i];
                if (
                    value is null
                    || string.Equals(value.ToString(), "null", StringComparison.OrdinalIgnoreCase)
                )
                    schema.Enum.RemoveAt(i);
            }

            if (schema.Enum.Count > 0)
            {
                schema.Type ??= JsonSchemaType.String;
            }
        }

        return schema;
    }
}
