using BlogLab.Models.Settings;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<CloudinarySettings>(cloudinarySettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
