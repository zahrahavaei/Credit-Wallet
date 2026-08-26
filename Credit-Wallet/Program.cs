using Credit_Wallet.Data;

using Credit_Wallet.Features.AddCreditToWallet;

using Credit_Wallet.Features.DeductFromWallet;

using Credit_Wallet.Features.MakeWallet;

using Credit_Wallet.Features.GetuserWallet;

using Credit_Wallet.Repositories;

using Credit_Wallet.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Credit_Wallet.Services;
using Credit_Wallet.Features.GetTransaction;
using Credit_Wallet.Features.GetTransactionHistory;
using Credit_Wallet.Features.GetTransactionHistoryByUserId;
using Credit_Wallet.Features.GetTransactionHistoryByWalletId;
using Credit_Wallet.Features.UserRegistration;
using Microsoft.AspNetCore.Identity;
using Credit_Wallet.Features.UserLogin;
using Credit_Wallet.Controllers;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter());
                });
                
             
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IMakeWalletService, MakeWalletService>();
builder.Services.AddScoped<AddCreditToWalletHandler>();
builder.Services.AddScoped<AddCreditToWalletValidator>();
builder.Services.AddScoped<DeductFromWalletValidator>();
builder.Services.AddScoped<DeductFromWalletHandler>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<GetUserWalletHandler>();
builder.Services.AddScoped<HmacService>();
builder.Services.AddScoped<WalletIntegrityService>();
builder.Services.AddScoped<TransactionIntegrityService>();
builder.Services.AddScoped<GetTransactionHandler>();
builder.Services.AddScoped<GetTransactionHistoryByWalletIdHandler>();
builder.Services.AddScoped<GetTransactionHistoryByUserIdHandler>();
builder.Services.AddScoped<GetTransactionHistoryByUserIdValidator>();
builder.Services.AddScoped<GetTransactionHistoryByWalletIdValidator>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
builder.Services.AddScoped<UserRegistrationHandler>();
builder.Services.AddScoped<UserLoginHandler>();
builder.Services.AddScoped<AuthController>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthorization();
app.MapControllers();

app.Run();
