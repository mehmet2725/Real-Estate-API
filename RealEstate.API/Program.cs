using RealEstate.Business.Profiles;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Concrete;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Abstract;
using RealEstate.Business.Abstract;
using RealEstate.Business.Concrete;
using RealEstate.Data.Abstract;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//  Database Connection
builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Identity (User Management)
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<RealEstateDbContext>();

//  AutoMapper
builder.Services.AddAutoMapper(typeof(RealEstate.Business.Profiles.MappingProfile));

//  Repository & UnitOfWork
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//  Services (Business)
builder.Services.AddScoped<IPropertyService, PropertyManager>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageManager>();

builder.Services.AddScoped<IPropertyTypeService, PropertyTypeManager>();

builder.Services.AddScoped<IInquiryService, InquiryManager>();

// JWT TOKEN AYARLARI 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 1. Controller Sistemi
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 2. FluentValidation Modern Entegrasyon
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});

// Validator'ları Business katmanından bul ve sisteme yükle
builder.Services.AddValidatorsFromAssemblyContaining<RealEstate.Business.ValidationRules.PropertyCreateValidator>();

// SWAGGER KİLİT BUTONU AYARLARI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RealEstate API", Version = "v1" });

    // Kilit (Authorize) Butonu Tanımı
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Token'ı 'Bearer {token}' formatında giriniz.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  Authentication -> Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Uygulama ayağa kalkarken Admin kullanıcısı var mı kontrol et
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Yazdığımız SeedData sınıfını çalıştır
        await RealEstate.API.Tools.SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Admin kullanıcısı oluşturulurken hata çıktı.");
    }
}

app.Run();