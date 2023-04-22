using FinanceAccounting;
using FinanceAccounting.Entities;
using FinanceAccounting.Exceptions;
using FinanceAccounting.Services;
using FinanceAccounting.Interfaces;
using FinanceAccounting.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IIncomeSourceService, IncomeSourceService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IExpenseSourceService, ExpenseSourceService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IValidator<RegistrationData>, AuthValidator>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

var app = builder.Build();

app.UseMiddleware<ExceptionsMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
