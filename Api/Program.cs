using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços à aplicação 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativação do Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de Tratamento Global de Erros
app.Use(async (context, next) =>
{
    try
    {
        await next(); 
    }
    catch (Exception ex)
    {
        // Define o status HTTP como 500 (Erro Interno).
        context.Response.StatusCode = 500;

        // Retorna uma resposta JSON padrão.
        await context.Response.WriteAsJsonAsync(new
        {
            Error = "Ocorreu um erro inesperado no servidor.",
            Details = ex.Message // Inclua detalhes apenas em desenvolvimento.
        });
    }
});

// Middleware de redirecionamento HTTPS (opcional, mas recomendado).
app.UseHttpsRedirection();

// Endpoint de exemplo (substitua pelos seus endpoints reais).
app.MapGet("/", () => "API Minimal com Tratamento Global de Erros!");

app.Run();