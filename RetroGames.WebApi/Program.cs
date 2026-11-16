using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Middlewares;

// Carrega o .env da raiz do repositório (procurando nas pastas acima) ANTES de criar o
// builder: assim ConnectionStrings__Default vira a configuração ConnectionStrings:Default.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string não encontrada. Crie o arquivo .env na raiz do repositório " +
        "(copie o .env.example) com a linha ConnectionStrings__Default.");
}

// Serviços (injeção de dependência)
builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Pipeline: a sequência de etapas por onde toda requisição passa.
// O tratamento de exceções vem primeiro, para pegar o erro de qualquer etapa seguinte.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opcoes => opcoes.SwaggerEndpoint("/openapi/v1.json", "RetroGames API"));
}

app.MapControllers();

app.Run();
