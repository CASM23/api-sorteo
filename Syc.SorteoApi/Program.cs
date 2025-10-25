using Microsoft.EntityFrameworkCore;
using SyC.Sorteo.Infrastructure.Persistence;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Infrastructure.Repositories;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Application.Services;
using SyC.Sorteo.Application.Validations;
using SyC.Sorteo.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SyC.Sorteo.Infrastructure.Identity;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime; 
using SyC.Sorteo.Api.Filters; // 🚨 NUEVO: Namespace del filtro de revocación

// =============================
// 🔹 CONFIGURACIÓN INICIAL
// =============================
var builder = WebApplication.CreateBuilder(args);


// =============================
// 🔹 CONTROLLERS + JSON
// =============================

builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ValidateJtiFilter)); 
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// =============================
// 🔹 SWAGGER
// =============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SyC Sorteo API",
        Version = "v1",
        Description = "API RESTful para gestión de sorteos - Prueba Técnica SyC S.A."
    });

    // 🔹 Habilitar JWT 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
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
            Array.Empty<string>()
        }
    });
});

// =============================
// 🔹 BASE DE DATOS
// =============================
builder.Services.AddDbContext<SorteoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =============================
// 🔹 CORS
// =============================
builder.Services.AddCors(p => p.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

// =============================
// 🔹 DEPENDENCIAS PERSONALIZADAS
// =============================

// Repositorios
builder.Services.AddScoped<IInscripcionRepository, InscripcionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Servicios de aplicación
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// Servicios de identidad
builder.Services.AddSingleton<IJwtService, JwtService>();

// Servicios de email
builder.Services.AddScoped<IEmailService, EmailService>();

// Validadores FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<InscripcionValidator>();

// 🚨 NUEVO: Registramos el filtro de revocación en el contenedor DI (porque usa DbContext)
builder.Services.AddScoped<ValidateJtiFilter>();


// =============================
// 🔹 CONFIGURACIÓN JWT (CON GESTIÓN DE ERRORES)
// =============================
var jwtSection = builder.Configuration.GetSection("Jwt");
// Manejo de nulo seguro para la clave
var jwtKey = jwtSection.GetValue<string>("Key") ?? 
             throw new InvalidOperationException("La clave JWT 'Key' no se encuentra en la configuración."); 


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {

        OnAuthenticationFailed = context =>
        {
      
            var exception = context.Exception; 
            
            if (exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
                context.Fail("El token de acceso ha expirado. Por favor, inicie sesión nuevamente.");
            }
            else if (exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
            {
                context.Fail("La firma del token es inválida. Posiblemente manipulado.");
            }
           

            return Task.CompletedTask;
        },
        
        OnChallenge = context =>
        {
          
            context.HandleResponse(); 
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var message = "Acceso no autorizado.";
            if (string.IsNullOrEmpty(context.Error) && string.IsNullOrEmpty(context.ErrorDescription))
            {
                message = "Token JWT faltante o malformado en el encabezado Authorization (Bearer).";
            }
            else
            {
                message = context.AuthenticateFailure?.Message ?? context.ErrorDescription ?? context.Error;
            }

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new 
            {
                status = 401,
                message = message 
            }));
        },
        
        OnForbidden = context =>
        {
            // Establece el código de estado 403
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            // Escribe el mensaje de error personalizado en el cuerpo de la respuesta
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
            {
                status = 403,
                message = "Acceso Prohibido. El usuario debe ser administrador para acceder a este recurso."
            }));
        }
    };
});

// Política de autorización (solo Admin)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// =============================
// 🔹 CONSTRUCCIÓN APP
// =============================
var app = builder.Build();

// =============================
// 🔹 SEED ADMIN (UNA SOLA VEZ)
// =============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SorteoDbContext>();

    // Si no existe, crear un admin por defecto
    if (!db.Usuarios.Any(u => u.NombreUsuario == "admin"))
    {
        var admin = new SyC.Sorteo.Domain.Entities.Usuario
        {
            NombreUsuario = "admin",
            ClaveHash = PasswordHasher.Hash("Admin1G23!"), 
            Correo = "admin@example.com",
            Rol = "Admin"
        };

        db.Usuarios.Add(admin);
        db.SaveChanges();
        Console.WriteLine("✅ Usuario admin creado: admin / Admin123!");
    }
}

// =============================
// 🔹 PIPELINE HTTP
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// 🔹 Autenticación y autorización JWT
app.UseAuthentication();
app.UseAuthorization();

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});


app.MapControllers();

app.Run();
