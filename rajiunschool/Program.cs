using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Data Protection to use a persistent key storage location
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\User\Desktop\rajiunschool\rajiunschool\DataProtection-Keys")) // Replace with your desired path
    .SetApplicationName("rajiunschool"); // Ensure consistent application name

// Configure session
builder.Services.AddDistributedMemoryCache(); // Session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(10);  // Session timeout (10 days)
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
    options.Cookie.Name = "StudentSession"; // Custom session cookie name
});

// Configure the database context
builder.Services.AddDbContext<UmanagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UmanagementDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); // Add session middleware to the pipeline

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();