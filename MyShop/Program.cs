using Microsoft.EntityFrameworkCore;
using MyShop.DAL;
using MyShop.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// The whole point of the code below is that whenever we have navigation properties in our Model classes, they refer to others in an 
// infinite loop. Example: Orders refer to Customers, which refers to Orders, which refers to customers, etc endless loop.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = 
    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
// builder.Services is an instance of IServiceCollection, which is used to configure and register application services, including database contexts.
// AddDbContext<ItemDbContext> method is to register a database context with the dependency injection container (builder.Services which
// is IServiceCollection interface, which is the dependency injection container).

// We do all this so that DbContext will work!
builder.Services.AddDbContext<ItemDbContext>(options => {
    options.UseSqlite(  // Here we are configuring the database provider to be SQLite
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]); // Here we access the configuration setting in our appsettings.json file.
    // Here we access the Connection String under the key ConnectionStrings:ItemDbContextConnection, and this connection string contains information about
    // database file paths and credentials needed to connect to the SQLite database.
});

// When we start the application, the dependency injector (ServiceCollection) associates requests for IItemRepository interface with instances
// of the ItemRepository class. Whenever we request an instance of IItemRepository to be injected into a controller or other parts of the application,
// the dependency injection system creates a new instance of ItemRepository scoped to the current HTTP request (.AddScoped()) and then provide that
// instance. That way we can work with ItemRepository instance without explicitly creating it. 
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// The line below configures and injects the logging dependency into our webapp which helps us with debugging
var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() // levels: Trace< Information < Warning < Error < Fatal. In other words, the minimum level og logging is information
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log"); // Our logging activities are written to our File Directory along with its time format

// We want to filter our logging because we are getting redundant text like "Executed DbCommand" all the time and we want to filter that out.
// Properties.TryGetValue checks if the log event has a property called SourceContext and it is used to identify the source of the log event, which is often
// a class or a component generating the log itself.
loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information && // Checks if the log's event is information.
                            e.MessageTemplate.Text.Contains("Executed DbCommand")); // Filters out messages containing the string "Executed DbCommand"

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Check if the app is running in the development environment, because we're ensuring the database is seeded with data only when the app
// is running in the dev environment. We usually don't want to seed in production, because we usually don't want to overwrite our prod data
// with seed data everytime the app restarts. 
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Provides detailed error messages during development
    DBInit.Seed(app); // This calls the DBInit in Models and calls the Seed method, which seeds the database with initial data.
}

// We need the app.useStaticFiles() method middleware in order to use the static files in wwwroot
app.UseStaticFiles();
app.UseAuthentication();
app.MapDefaultControllerRoute();

app.Run();
