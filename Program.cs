using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;

using Api_HabeisEducacional.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do contexto do banco de dados usando MySQL
// Obtém a string de conexão do arquivo appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)) // Especifica a versão do MySQL
    )
);

// Registro dos serviços da aplicação
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();
builder.Services.AddScoped<ICertificadoService, CertificadoService>();

// Configuração do CORS para permitir requisições de origens diferentes
// Isso é essencial para aplicações frontend que acessam esta API de outros domínios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        builder => builder
            .AllowAnyOrigin()     // Permite qualquer origem
            .AllowAnyMethod()     // Permite qualquer método HTTP (GET, POST, PUT, DELETE)
            .AllowAnyHeader());   // Permite qualquer cabeçalho HTTP
});

// Adiciona os controladores para a API
builder.Services.AddControllers();

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do middleware
// Em ambiente de desenvolvimento, habilita o Swagger para testes e documentação
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS por segurança
app.UseHttpsRedirection();

// Aplica a política CORS definida anteriormente
app.UseCors("AllowAll");

// Adiciona suporte a autenticação
app.UseAuthorization();

// Configuração das rotas dos controladores
app.MapControllers();

// Inicia a aplicação
app.Run();
