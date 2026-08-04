using Credit_Wallet.Data;

using Credit_Wallet.Features.AddCreditToWallet;

using Credit_Wallet.Features.DeductFromWallet;

using Credit_Wallet.Features.MakeWallet;

using Credit_Wallet.Features.GetuserWallet;

using Credit_Wallet.Repositories;

using Credit_Wallet.Data.Entities;

using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IMakeWalletService, MakeWalletService>();
builder.Services.AddScoped<AddCreditToWalletHandler>();
builder.Services.AddScoped<AddCreditToWalletValidator>();
builder.Services.AddScoped<DeductFromWalletValidator>();
builder.Services.AddScoped<DeductFromWalletHandler>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<GetUserWalletHandler>();


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
