using MediSync.Data;
using MediSync.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURACIÓN DE SERVICIOS ====================
builder.Services.AddHttpClient();

// Conexión a la base de datos MediSync
builder.Services.AddDbContext<MediSyncContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MediSyncConnection")));

// Inyección del servicio de IA
builder.Services.AddScoped<IAService>();

// Controladores y páginas Razor
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Sesiones
builder.Services.AddSession();

// Autenticación con Google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

// Swagger (solo desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==================== PIPELINE DE APLICACIÓN ====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();
app.MapRazorPages();

app.Run();
