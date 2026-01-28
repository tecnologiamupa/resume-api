using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resume.API.Middlewares;
using Resume.Core;
using Resume.Core.DTOs;
using Resume.Core.ExternalServiceContracts;
using Resume.Core.Mappers;
using Resume.Infrastructure;
using Resume.Infrastructure.ExternalServices;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de infraestructura y core
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Leer la URL base desde appsettings.json
var companyServiceBaseUrl = builder.Configuration["ExternalApis:CompanyService:BaseUrl"];

// Validar que la URL base no sea nula o vacía antes de usarla
if (string.IsNullOrEmpty(companyServiceBaseUrl))
{
    throw new InvalidOperationException("La URL base para el servicio de company no está configurada en appsettings.json.");
}

// Configurar un cliente HTTP para interactuar con el servicio de company externo
builder.Services.AddHttpClient<ICompanyServiceClient, CompanyServiceClient>(client =>
{
    client.BaseAddress = new Uri(companyServiceBaseUrl);
});

// Agregar controladores a la colección de servicios
builder.Services.AddControllers();

// Agregar AutoMapper
builder.Services.AddAutoMapper(typeof(ResumeInfoMapping).Assembly);

// Agregar servicios de explorador de API
builder.Services.AddEndpointsApiExplorer();

// Agregar servicios de generación de Swagger para crear la especificación de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token JWT con el prefijo 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Agregar servicios de Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Validar que la clave JWT no sea nula o vacía antes de usarla
var jwtKey = builder.Configuration["JwtSettings:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("La clave JWT no está configurada en appsettings.json.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validar el emisor del token
            ValidateAudience = true, // Validar la audiencia del token
            ValidateLifetime = true, // Validar la expiración del token
            ValidateIssuerSigningKey = true, // Validar la clave de firma
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], // Emisor esperado
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // Audiencia esperada
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Clave secreta
        };

        options.Events = new JwtBearerEvents();
        options.Events.OnChallenge = context =>
        {
            context.HandleResponse(); // Evita el comportamiento predeterminado de desafío
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<string>
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "No autorizado.",
                IsSuccess = false
            };

            return context.Response.WriteAsJsonAsync(response);
        };
    });

// Agregar un accesorio de contexto HTTP para permitir el acceso al contexto HTTP actual en servicios y otros componentes
builder.Services.AddHttpContextAccessor();

// Agregar servicios de caché en memoria
builder.Services.AddMemoryCache();

// Construir la instancia de la aplicación web
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Enrutamiento
app.UseRouting();
app.UseSwagger(); // Agrega un endpoint que puede servir el swagger.json
app.UseSwaggerUI(); // Agrega la UI de Swagger (página interactiva para explorar y probar los endpoints de la API)
app.UseCors();

// Autenticación
app.UseAuthentication();
app.UseAuthorization();

// Rutas de controladores
app.MapControllers();

app.Run();
