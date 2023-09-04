var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// We need the app.useStaticFiles() method middleware in order to use the static files in wwwroot
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
