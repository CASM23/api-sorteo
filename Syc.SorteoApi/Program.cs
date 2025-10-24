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

// =============================
// 🔹 CONFIGURACIÓN INICIAL
// =============================
var builder = WebApplication.CreateBuilder(args);

// =============================
// 🔹 CONTROLLERS + JSON
// =============================
builder.Services.AddControllers()
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

    // 🔹 Habilitar JWT en Swagger
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

// Validadores FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<InscripcionValidator>();

// =============================
// 🔹 CONFIGURACIÓN JWT
// =============================
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Cambia a true en producción
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
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
            ClaveHash = PasswordHasher.Hash("Admin123!"),
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

app.MapControllers();

app.Run();
