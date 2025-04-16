using APIDevSteam.Data;
using APIDevSteam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Adicionar serviço do Banco de dados
// Adicionar Servico de banco de dados
builder.Services.AddDbContext<ApiDevsteamContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Adicionando o serviço do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
// Adicionar o serviço de autenticação
// Serviço de EndPoints do Identity Framework
builder.Services.AddIdentityApiEndpoints<Usuario>(options =>
{
    options.SignIn.RequireConfirmedEmail = false; // Exige confirmação de email
    options.SignIn.RequireConfirmedAccount = false; // Exige confirmação de conta
    options.User.RequireUniqueEmail = true; // Exige email único
    options.Password.RequireNonAlphanumeric = false; // Exige caracteres não alfanuméricos
    options.Password.RequireLowercase = false; // Exige letras minúsculas
    options.Password.RequireUppercase = false; // Exige letras maiúsculas
    options.Password.RequireDigit = false; // Exige dígitos numéricos
    options.Password.RequiredLength = 4; // Exige comprimento mínimo da senha
})
.AddRoles<IdentityRole>() // Adicionando o serviço de roles
.AddEntityFrameworkStores<ApiDevsteamContext>() // Adicionando o serviço de EntityFramework
.AddDefaultTokenProviders(); // Adiocionando o provedor de tokens padrão

// Swagger com Autenticação JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "APIDevSteam", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Adicionar os Serviços de Autenticação e Autorização
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();//gerar o swagger

app.UseSwaggerUI();//gerar a interface do swagger

app.UseHttpsRedirection();

app.UseCors("AllowAll");//configuração do CORS

app.UseAuthentication();// Autenticação

app.UseAuthorization();// Autorização

app.MapControllers();// Mapear os controllers

app.MapGroup("/Usuario").MapIdentityApi<Usuario>();// Mapear os endpoints de autenticação

app.Run();