using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Controllers;
using MossadAgentsAPI.Servise;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MossadAgentsAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'MossadAgentsAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MossadAgentsAPIContext>();
builder.Services.AddScoped<TargetMissionServise>();
builder.Services.AddScoped<AgentMissionServise>();
builder.Services.AddScoped<UpdateServise>();
builder.Services.AddScoped<SetMissionStatus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapAgentEndpoints();

app.Run();
