using FarmApi.Data;
using FarmApi.Repositories;
using FarmApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Kestrel to listen on the specified port ---
var port = Environment.GetEnvironmentVariable("PORT") ?? "5022";
builder.WebHost.UseUrls($"http://*:{port}");

var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("Startup");

// --- Add Services ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Authentication Configuration (Phase 2 Extension) ---
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? Environment.GetEnvironmentVariable("SUPABASE_URL");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // World-class engineering: ใช้ Authority เพื่อรองรับการดึง Public Key (ES256) อัตโนมัติจาก Supabase
        options.Authority = $"{supabaseUrl}/auth/v1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{supabaseUrl}/auth/v1",
            ValidateAudience = true,
            ValidAudience = "authenticated", // Supabase tokens ใช้ 'authenticated' เป็น Audience
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                logger.LogError("❌ Authentication Failed: {ErrorMessage}", context.Exception.Message);
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    logger.LogError("Token has expired.");
                }
                return Task.CompletedTask;
            }
        };
    });

// --- Database Configuration (Phase 1) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL"); // Support for Railway Env Var

if (string.IsNullOrEmpty(connectionString))
{
    logger.LogWarning("⚠️ Database Connection String is missing. Check appsettings.json or Environment Variables.");
    
    if (!builder.Environment.IsDevelopment()) // ถ้าไม่ใช่เครื่อง Local ให้ระบุว่าเป็นปัญหาที่ Config บน Cloud
    {
        logger.LogError("Critical: DATABASE_URL variable is not set on the hosting environment.");
        
        throw new InvalidOperationException(
            "⚠️ Database Connection String 'DefaultConnection' is missing. " +
            "Please provide it in appsettings.json or via the 'DATABASE_URL' environment variable.");
    }
}

if (builder.Environment.IsDevelopment() && connectionString?.Contains("[YOUR_") == true)
{
    logger.LogCritical("❌ You are using a placeholder in your Connection String! " +
                       "Please update appsettings.json with your actual Supabase Database Password.");
}

if (builder.Environment.IsDevelopment() && !string.IsNullOrEmpty(connectionString))
{
    logger.LogInformation("Attempting to connect to database...");
}

builder.Services.AddDbContext<FarmDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention()); // helper for matching C# PascalCase to PostgreSQL snake_case

// --- Supabase Client Configuration ---
var supabaseKey = builder.Configuration["Supabase:Key"] ?? Environment.GetEnvironmentVariable("SUPABASE_KEY");

if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    logger.LogError("❌ Supabase URL or Key is missing. AuthController will fail to initialize.");
    
    if (!builder.Environment.IsDevelopment())
    {
        throw new InvalidOperationException("Supabase configuration is required for production.");
    }
}

builder.Services.AddScoped<Client>(_ =>
    new Client(supabaseUrl ?? string.Empty, supabaseKey ?? string.Empty, new SupabaseOptions { AutoRefreshToken = true }));

// --- Dependency Injection (Phase 4: Repositories & Services) ---
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<IFarmService, FarmService>();

// Add CORS policy for Next.js integration (Phase 3)
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                     ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Configure Swagger (Development only) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Open Swagger UI at the root URL
    });
    app.UseDeveloperExceptionPage();
}

app.UseRouting(); // เพิ่มการจัดการ Routing ให้ชัดเจน
app.UseCors("AllowNextJs");
app.UseAuthentication(); // ตรวจสอบว่าใครเรียกมา
app.UseAuthorization();  // ตรวจสอบว่าเขามีสิทธิ์ไหม
app.MapControllers();

// Simple health check to verify the server is actually up
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Port = port }));

await app.RunAsync();