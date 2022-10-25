using AutoMapper;
using Payroll.Core;
using Payroll.Core.Engine;
using Payroll.Domain.AppSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<TaxSettingOptions>(builder.Configuration.GetSection(TaxSettingOptions.TaxSettings));
builder.Services.AddSingleton<ITaxProcessor, TaxProcessor>();
builder.Services.AddSingleton<IRuleEngine, RuleEngine>();
builder.Services.AddAutoMapper(typeof(Payroll.Domain.Models.PayslipRequestProfile).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
