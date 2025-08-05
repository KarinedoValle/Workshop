using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Workshop.DB;
using Workshop.Services.API.Workshop;



var builder = WebApplication.CreateBuilder(args);
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new DateTimeLocalJsonConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Workshop API", Version = "v1" });
    c.SchemaFilter<CustomDateSchemaFilter>();
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var controllerActionDescriptor = apiDesc.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        return controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(ApiControllerAttribute), true).Any() ?? false;
    });
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Insira o token JWT no formato: Bearer {token}",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtSecret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services.AddSingleton<SecurityKey>(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("AuthToken"))
                {
                    context.Token = context.Request.Cookies["AuthToken"];
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var path = context.Request.Path.Value ?? "";
                if (!path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)) {
                    context.HandleResponse();
                    context.Response.Redirect("/Erro/401");
                }
                return Task.CompletedTask;

            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5001",
            ValidAudience = "http://localhost:5001",
            IssuerSigningKey = jwtKey
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IWorkshopService, WorkshopService>();
builder.Services.AddScoped<Workshop.Services.API.Usuario.IUsuarioService, Workshop.Services.API.Usuario.UsuarioService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICadastroService, CadastroService>();
builder.Services.AddScoped<Workshop.Services.View.Usuario.IUsuarioService, Workshop.Services.View.Usuario.UsuarioService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "VisualizarPorIdentificador",
    pattern: "{controller}/{action}/{identificador}");


app.MapControllerRoute(
    name: "CadastrarOuVisualizarTodosOsItens",
    pattern: "{controller}/{action}",
    defaults: new { controller = "Login", action = "Logar" });



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workshop API V1");
    c.RoutePrefix = "swagger";
});



app.MapControllers();

app.UseStatusCodePagesWithReExecute("/Erro/{0}");

app.Run();



