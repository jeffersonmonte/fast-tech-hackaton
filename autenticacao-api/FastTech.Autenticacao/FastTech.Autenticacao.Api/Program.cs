using FastTech.Autenticacao.Application;
using FastTech.Autenticacao.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT")!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Funcionario", policy =>
        policy.RequireClaim("Perfil", "Funcionario"))
    .AddPolicy("Gerente", policy =>
        policy.RequireClaim("Perfil", "Gerente"))
    .AddPolicy("Cliente", policy =>
        policy.RequireClaim("Perfil", "Cliente"));

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
