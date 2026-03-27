using Microsoft.OpenApi;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

internal static class OpenApiExtensions
{
    extension(Microsoft.AspNetCore.OpenApi.OpenApiOptions openApiOptions)
    {
        public Microsoft.AspNetCore.OpenApi.OpenApiOptions ConfigureOpenApiSpec()
        {
            return openApiOptions.AddSchemaTransformer(
                (schema, context, ct) =>
                {
                    schema.AdditionalPropertiesAllowed = false;

                    // Remove null from enum value lists. Nullability is already
                    // expressed via oneOf at the property level; the null in
                    // enum values is a .NET OpenAPI generator quirk that breaks
                    // NSwag TypeScript code generation.
                    if (schema.Enum is { Count: > 0 })
                    {
                        for (var i = schema.Enum.Count - 1; i >= 0; i--)
                        {
                            var value = schema.Enum[i];
                            if (
                                value is null
                                || string.Equals(
                                    value.ToString(),
                                    "null",
                                    StringComparison.OrdinalIgnoreCase
                                )
                            )
                                schema.Enum.RemoveAt(i);
                        }
                    }

                    return Task.CompletedTask;
                }
            );
        }

        public Microsoft.AspNetCore.OpenApi.OpenApiOptions ConfigureAuthSpec(
            string appName,
            AuthConfiguration authConfiguration
        )
        {
            return openApiOptions.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = appName,
                        Version = "v1",
                        Description = $"Common entrypoints to interact with {appName}.",
                    };
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
}
