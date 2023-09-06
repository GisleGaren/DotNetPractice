using Microsoft.EntityFrameworkCore;
using MyShop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

var app = builder.Build();

// We need the app.useStaticFiles() method middleware in order to use the static files in wwwroot
app.UseStaticFiles();
app.MapDefaultControllerRoute();



app.Run();
