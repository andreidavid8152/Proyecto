using Proyecto.Configurations;
using Proyecto.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Obtener la configuraci�n del builder
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura y registra ApiSettings en el contenedor de servicios usando la secci�n 'ApiSettings' del archivo de configuraci�n.
builder.Services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

// Agrega el servicio HttpClient al contenedor. Esto permite inyectar y usar HttpClient en cualquier parte de la aplicaci�n.
builder.Services.AddHttpClient();

// Registra el servicio ApiService como 'Scoped', lo que significa que se crear� una instancia por cada solicitud. 
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILocalService, LocalService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();

// Agregar servicios de sesi�n aqu�
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Middleware de sesi�n
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
