using BankingApi.Data;
using Microsoft.EntityFrameworkCore;
using BankingApi.Contracts;
using BankingApi.Repositories;
using BankingApi.Services;




var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();






builder.Services.AddScoped<IBankAccountRepository, EfBankAccountRepository>();
builder.Services.AddScoped<IBankService, BankService>();

builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlite(@"Data Source=C:\temp\banking.db"));


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
