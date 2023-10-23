using DataAcessObjects.DAO;
using Repositories.Repositories.Imple;
using Repositories.Repositories;
using FUCarRentingSystem.Mapper;
using FUCarRentingSystem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
IConfiguration config = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
builder.Services.AddJWTConfiguration(config["AppSettings:SecretKey"]);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<CustomerDAO>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CarInformationDAO>();
builder.Services.AddScoped<ICarInformationRepository, CarInformationRepository>();
builder.Services.AddScoped<SupplierDAO>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ManufacturerDAO>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<RentingTransactionDAO>();
builder.Services.AddScoped<IRentingTransactionRepository, RentingTransactionRepository>();
builder.Services.AddScoped<RentingDetailDAO>();
builder.Services.AddScoped<IRentingDetailRepository, RentingDetailRepository>();
builder.Services.AddSwaggerGenConfiguration();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
