using MediaShop.Business.AutoMapper;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Services;
using MediaShop.Business.Validation;
using MediaShop.Configuration;
using MediaShop.Data;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Data.Repositories;
using MediaShop.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("Azure:BlobStorage"));
builder.Services.AddDbContext<MediaShopContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(90);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
        .AddEntityFrameworkStores<MediaShopContext>()
        .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Secret"))),
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        ValidateLifetime = true,
    };
});
RegisterServices(builder);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(
    new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger(),
    dispose: true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("https://create-mediashop.azurewebsites.net", "https://mediashop.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("Access-Control-Allow-Origin")
                .SetIsOriginAllowed(origin => true);
        });
});
builder.Services.AddControllers(options =>
{

    options.EnableEndpointRouting = false;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;

});
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 

    builder.Services.AddScoped<RegisterDtoValidator>();
    builder.Services.AddScoped<ProductDtoValidator>();
    builder.Services.AddScoped<CategoryDtoValidator>();
    builder.Services.AddScoped<UpdateUserDtoValidator>();
    builder.Services.AddScoped<IMediaService, MediaService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IReviewService, ReviewService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IProductService, ProductService>();
}