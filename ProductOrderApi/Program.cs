using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductOrderApi.Helpers;
using ProductOrderApi.Infrastructure;
using ProductOrderApi.Infrastructure.Interfaces;
using ProductOrderApi.Middleware;
using ProductOrderApi.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Protobuf.WellKnownTypes;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is missing");

var mqHost = builder.Configuration["RabbitMq:Host"] ?? throw new InvalidOperationException("RabbitMq:Host is missing");
var mqQueue = builder.Configuration["RabbitMq:Queue"] ?? throw new InvalidOperationException("RabbitMq:Queue is missing");

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");


builder.Services.AddControllers(setup =>
{
    setup.Filters.Add<ValidateModelStateFilterAttribute>();
})
.ConfigureApiBehaviorOptions(setup =>
{
    setup.SuppressModelStateInvalidFilter = true;
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ITransaction, TransactionHandler>();

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionDependentBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserDependentBehavior<,>));

// Add MediatR
builder.Services.AddMediatR(setup =>
{
    setup.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// --- Configure Authentication ---

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        )
    };
});

// --- Configure DB Context ---

builder.Services.AddDbContext<AppDbContext>(o => o.UseMySQL(connectionString));

// --- Configure JSON Serialization ---

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(setup => setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// --- Configure Swagger ---

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Include the XML documentation file for Swagger
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "ProductOrderApi.xml");
    setup.IncludeXmlComments(xmlFile);

    // Configure Authorization Schema
    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insert your token"
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
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



// --- RabbitMq ---

builder.Services.AddSingleton<RabbitMqSender>(
    setup => new RabbitMqSender(mqHost, mqQueue)
);

// --- Build Application ---

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// --- Development Environment Setup ---

if (app.Environment.IsDevelopment())
{
    bool enableSwagger = builder.Configuration.GetValue<bool>("Swagger:Enabled");
    
    if (enableSwagger)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");

            c.InjectJavascript("/swagger.js");
        });
    }
}

// --- Middleware Setup ---

app.UseMiddleware<ExceptionHandlingMiddleware>();

// --- Authentication and Authorization ---

app.UseAuthentication();
app.UseAuthorization();

// --- Map Controllers ---
app.UseStaticFiles();
app.MapControllers();

// --- Run Application ---

app.Run();