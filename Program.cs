using Microsoft.EntityFrameworkCore;
using Workshop.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "VisualizarPorIdentificador",
    pattern: "{controller}/{action}/{identificador}");


app.MapControllerRoute(
    name: "CadastrarOuVisualizarTodosOsItens",
    pattern: "{controller}/{action}",
    defaults: new { controller = "Home", action = "Index" });



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workshop API V1");
    c.RoutePrefix = "swagger";
});



app.MapControllers();

app.Run();
