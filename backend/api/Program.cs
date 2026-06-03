using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// new
IConfiguration _config = builder.Configuration;
var jwtSettings = _config.GetSection("JwtSettings");
var allowedHosts = builder.Configuration["AllowedHosts"];
allowedHosts = string.IsNullOrEmpty(allowedHosts) ? "*" : allowedHosts;
var AllowedHosts = allowedHosts.Split(',').Select(h => h.Trim()).ToArray();
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        x.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse(); // Prevent default 401 response
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Authentication required. Please provide a valid token.",
                    data = (object?)null
                });
                await context.Response.WriteAsync(result);
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});
builder.Services.AddTransient<AuthService>();
// builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// end new

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// new
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIGAP API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
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
    };

    c.AddSecurityRequirement(securityRequirement);
    c.OperationFilter<FormDataArrayFixFilter>();
    // c.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()        // ← add this
           .EnableDetailedErrors()
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder.WithOrigins(AllowedHosts)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
});
// end new
var app = builder.Build();

// Auto -migration & seeder
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    SigapSeeder.Seed(db);
}

// new
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
// end new

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// end new
app.MapControllers();
// app.MapControllers().RequireAuthorization();

app.MapGet("/", () => $"Environment: {app.Environment.EnvironmentName}");
if (app.Environment.IsDevelopment())
{
    app.MapGet("/check-db", async (ApplicationDbContext dbContext) =>
    {
        try
        {
            bool canConnect = await dbContext.Database.CanConnectAsync();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            return Results.Ok(new
            {
                status = canConnect ? "✅ Database connection successful" : "❌ Cannot connect to the database",
                connectionString = MaskConnectionString(connectionString ?? string.Empty) // Masked for security
            });
        }
        catch (Exception ex)
        {
            return Results.Problem($"❌ Database error: {ex.Message}");
        }
    });
}
string MaskConnectionString(string connStr)
{
    if (string.IsNullOrEmpty(connStr)) return connStr;
    return $"{connStr}";
    // Replace password with *****
    // return System.Text.RegularExpressions.Regex.Replace(
    //     connStr,
    //     @"(Password|Pwd)\s*=\s*[^;]+",
    //     "$1=*****",
    //     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
}
app.Run();
