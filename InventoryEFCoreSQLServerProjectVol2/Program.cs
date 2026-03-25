using Inventory.Api.Data;
using Inventory.Api.Repositories;
using Inventory.Api.Services;
using Inventory.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// MVC + API (включва controllers и views)
builder.Services.AddControllersWithViews();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

// FluentValidation за DTO валидация
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Logging конфигурация за development/debug balance
builder.Services.Configure<LoggerFilterOptions>(options =>
{
    options.AddFilter("Microsoft", LogLevel.Warning);
    options.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    options.AddFilter("Inventory.Api", LogLevel.Information);
});

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inventory API",
        Version = "v1"
    });
});

// EF Core DbContext с SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection за business logic
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Static files - ЗАДЪЛЖИТЕЛНО за картинки, CSS, JS
app.UseStaticFiles();

// Database initialization (OK за local/dev; за production предпочитам migration scripts)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (dbContext.Database.IsRelational())
    {
        var hasMigrations = dbContext.Database.GetMigrations().Any();
        if (hasMigrations)
            dbContext.Database.Migrate();
        else
            dbContext.Database.EnsureCreated();
    }
    else
    {
        dbContext.Database.EnsureCreated();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SupportedSubmitMethods(
            SubmitMethod.Get,
            SubmitMethod.Post,
            SubmitMethod.Put,
            SubmitMethod.Delete,
            SubmitMethod.Patch);
    });
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature is null)
                return;

            await Results.Problem(
                title: "Възникна неочаквана грешка.",
                detail: "Опитай отново по-късно или провери входните данни.",
                statusCode: StatusCodes.Status500InternalServerError
            ).ExecuteAsync(context);
        });
    });
}

// HTTPS redirection - временно изключено за локално тестване без dev certificate
// app.UseHttpsRedirection();

// Routing - ЗАДЪЛЖИТЕЛНО за MVC controllers
app.UseRouting();

// Authorization (без authentication - OK за текущия сценарий)
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Explicit root route
app.MapGet("/", () => Results.Redirect("/products"));

// Status page route
app.MapControllerRoute(
    name: "status",
    pattern: "status",
    defaults: new { controller = "Status", action = "Index" });

// Additional route for products
app.MapControllerRoute(
    name: "products",
    pattern: "products",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Make Program accessible for integration tests
public partial class Program { }
