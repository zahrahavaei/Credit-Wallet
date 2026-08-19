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


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
