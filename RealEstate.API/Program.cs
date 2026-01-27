
using RealEstate.Business.Profiles;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Concrete;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Abstract;
using RealEstate.Business.Abstract;
using RealEstate.Business.Concrete;
using RealEstate.Data.Abstract;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

//  Database Connection(PostgreSQL)
builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Identity(User Managment)
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<RealEstateDbContext>();

// AutoMapper
// RealEstate Business katmanındaki mapping progile sınıfını referans alarak tarama yapar
builder.Services.AddAutoMapper(typeof(RealEstate.Business.Profiles.MappingProfile));

// Repository & UnitOfWork
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- Services (Business) ---
builder.Services.AddScoped<IPropertyService, PropertyManager>();

builder.Services.AddScoped<IPropertyImageService, PropertyImageManager>();

//  Controller System
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//  Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer(); // Endpoint'leri keşfet
builder.Services.AddSwaggerGen();           // Swagger jeneratörünü ekle

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Controller'ları endpoint olarak haritala
app.MapControllers();

app.Run();