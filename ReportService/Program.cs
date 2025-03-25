using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var keycloakAuthority = "http://localhost:8080/realms/MyRealm";
var keycloakClientId = "report-service";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Додаємо Swagger із підтримкою OAuth2 (Keycloak)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Report Service API", Version = "v1" });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/auth"),
                TokenUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect scope" },
                    { "profile", "User profile" },
                    { "email", "User email" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new List<string> { "openid", "profile", "email" }
        }
    });
});

// Додаємо аутентифікацію через Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.Audience = keycloakClientId;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", () => "Report Service");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Service API V1");
        options.OAuthClientId(keycloakClientId);
        options.OAuthAppName("Report Service API - Swagger");
        options.OAuthUsePkce();
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

builder.WebHost.UseUrls("http://*:81");
app.Run();
