using DotNet.Docker.Properties.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço Swagger ao container de dependências
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra o Supabase.Client como Singleton
const string url = "https://eabruaevckpvepqqqmfv.supabase.co";
const string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVhYnJ1YWV2Y2twdmVwcXFxbWZ2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjU4MjczOTgsImV4cCI6MjA0MTQwMzM5OH0.Eczsx_ij7fybFr3cZFBP0jtmmxbZMZYwP-FeuBjciyE";
var options = new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
};

var supabaseClient = new Client(url, key, options);
await supabaseClient.InitializeAsync();
builder.Services.AddSingleton(supabaseClient);

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");  // Teste com qualquer origem permitida

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/Test",() => 1);

app.UseHttpsRedirection();
app.Run();