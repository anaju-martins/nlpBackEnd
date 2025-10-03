using Microsoft.EntityFrameworkCore;
using nplBackEnd.Data;
using nplBackEnd.Services.Abstractions;
using nplBackEnd.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Serviços para Injeção de Dependência (DI)

// Adicionar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Adicionar serviços da aplicação (com escopo por requisição)
builder.Services.AddScoped<IAnalysisService, AnalysisService>();
builder.Services.AddScoped<INlpService, NlpService>();

// Configurar o HttpClient para o serviço Python
builder.Services.AddHttpClient("NlpService", client =>
{
    var baseUrl = builder.Configuration.GetSection("NlpService:BaseUrl").Value;
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("NLP Service BaseUrl is not configured.");
    }
    client.BaseAddress = new Uri(baseUrl);
});


builder.Services.AddControllers();

// Adicionar Swagger para documentação e testes da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar CORS para permitir que o Frontend (Next.js) acesse a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL do seu app Next.js
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// 2. Configurar o Pipeline de Middlewares HTTP

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowNextApp"); // Aplicar a política de CORS

app.UseAuthorization();

app.MapControllers();

app.Run();