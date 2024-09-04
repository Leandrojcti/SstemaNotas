using Microsoft.EntityFrameworkCore;
using SistemaNote.BancoDados;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//string de conexao com banco de dados
builder.Services.AddDbContext<ClaseContext>(Options => Options.UseSqlServer

//string de conexão database server remote azure
("Server=tcp:notasdb.database.windows.net,1433;Initial Catalog=NotasDataBase;Persist Security Info=False;User ID=leandro;Password=Le841626;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;Connection Timeout=30;\r\n"));

//String de conexão database local
// ("Server=DESKTOP-E98EF1T\\SQLEXPRESS; Database=NotasDataBase; trusted_connection=true; trustservercertificate=true"));

// Adiciona os serviços de sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Torna o cookie de sessão inacessível via JavaScript
    options.Cookie.IsEssential = true; // Necessário para a conformidade com o GDPR
});

// Adiciona serviços MVC ou Razor Pages
builder.Services.AddControllersWithViews(); // Ou AddRazorPages()

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
