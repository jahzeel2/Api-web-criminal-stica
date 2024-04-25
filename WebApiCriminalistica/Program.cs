using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiCriminalistica.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebApiCriminalisticaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiCriminalisticaContext") ?? throw new InvalidOperationException("Connection string 'WebApiCriminalisticaContext' not found.")));

// Add services to the container.

//variable para error de cors
string cors = "ConfigurarCors";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//para error de cords
builder.Services.AddCors(options => {
    options.AddPolicy(name: cors, builder =>
    {
        builder.WithMethods("*");
        builder.WithHeaders("*");
        builder.WithOrigins("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//agregue para los cords
app.UseCors(cors);

app.MapControllers();

app.Run();
