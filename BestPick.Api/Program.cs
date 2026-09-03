using BestPick.Aplicacao.Aplicacoes;
using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IFavoritoAplicacao, FavoritoAplicacao>();
builder.Services.AddScoped<IHistoricoAplicacao, HistoricoAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();


// Add interfaces to the databases
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IFavoritoRepositorio, FavoritoRepositorio>();
builder.Services.AddScoped<IHistoricoRepositorio, HistoricoRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

