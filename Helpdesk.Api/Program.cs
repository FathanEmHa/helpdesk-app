using System.Text.Json.Serialization;
using System.Text;
using Scalar.AspNetCore;
using Helpdesk;
using Helpdesk.Services;
using Helpdesk.Data;
using Helpdesk.Data.Interceptors;
using Helpdesk.Middleware;
using Helpdesk.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Helpdesk API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Masukkan JWT tanpa kata Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unauthorized."
                });
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Access denied."
                });
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ActivityLogService>();

builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    options.AddInterceptors(
        serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Helpdesk API")
            .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
    await DataSeeder.SeedAdminAsync(db);
};

app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();