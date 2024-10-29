using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Utilidades;
using SistemaInventarioV6.AccesoDatos.Data;

namespace SistemaInventarioV6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddErrorDescriber<ErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //Aca se agrega el otro builder para que reedirecciones a login o logout o acceso denegado si el usuario no esta autorizado
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Login";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false; //Pa que no me pida numeros
                options.Password.RequireLowercase = true; //Para que aunque sea una letra sea minuscula
                options.Password.RequireNonAlphanumeric = false; //Pa que no pida los caracteres especials
                options.Password.RequireUppercase = false; //Pa que no pida letras mayusculas
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1; // Pa que se pueda repetir al menos una vez alguno de los caracteres 
            });

            //Aca se agreaga el serviciog
            builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

            builder.Services.AddRazorPages();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            //Servicio para sesiones
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //para sesiones
            app.UseSession();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            //agregado para la rotativa y poder usar las cosas
            IWebHostEnvironment env = app.Environment;
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "..\\Rotativa\\Windows\\");



            app.Run();
        }
    }
}
