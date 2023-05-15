using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DataAccess.Repository;
using MyVerificationCodeCleanupProject.Services;
using BusinessObject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
     .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // Thêm các cấu hình khác theo nhu cầu của bạn
    });
builder.Services.AddDbContext<MilkDBContext>();
builder.Services.AddScoped<VerificationCodeCleanupService>();

builder.Services.AddSingleton<IRepositoryAccount, AccountRepository>();
builder.Services.AddSingleton<IRepositoryVerificationCode, VerificationCodeRepository>();

var configuration = builder.Configuration;
var secretKey = configuration["Appsettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SubcriptionMilk", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
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
                new string[] { }
            }
        });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors(builder => builder
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed((host) => true)
              .AllowCredentials().WithOrigins("https://localhost:5001",
                                              "https://localhost:3000"
                                              )
          );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

