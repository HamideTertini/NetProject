using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ProductApi.Middleware;
using ProductApi.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddScoped<IProductService, ProductService>();

var key = Encoding.UTF8.GetBytes("super_secret_jwt_key_12345_67890_ABCDE");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

SeedData(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Products.Any())
        return; 

    var products = new List<Product>
    {
        new Product
        {
            Name = "Wireless Mouse",
            Category = "Electronics",
            Price = 19.99m,
            StockQuantity = 120,
            CreatedAt = DateTime.UtcNow
        },
        new Product
        {
            Name = "Mechanical Keyboard",
            Category = "Electronics",
            Price = 49.99m,
            StockQuantity = 80,
            CreatedAt = DateTime.UtcNow
        },
        new Product
        {
            Name = "Bluetooth Headphones",
            Category = "Electronics",
            Price = 59.99m,
            StockQuantity = 45,
            CreatedAt = DateTime.UtcNow
        },
        new Product
        {
            Name = "LED Monitor 24-inch",
            Category = "Electronics",
            Price = 129.99m,
            StockQuantity = 40,
            CreatedAt = DateTime.UtcNow
        }
    };

    db.Products.AddRange(products);
    db.SaveChanges();
}
