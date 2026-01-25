
using RealEstate.Business.Profiles;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Concrete;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Abstract;
using RealEstate.Business.Abstract;
using RealEstate.Business.Concrete;
using RealEstate.Data.Abstract;


var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection(PostgreSQL)
builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity(User Managment)
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

// 3. Controller System
builder.Services.AddControllers();

// 4. Swagger/OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Created swagger JSON
}

app.UseHttpsRedirection();

// Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Controller'ları endpoint olarak haritala
app.MapControllers();

app.Run();