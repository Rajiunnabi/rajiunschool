using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure session
builder.Services.AddDistributedMemoryCache(); // Session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(10);  // Session timeout (30 minutes)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseSession(); // Add session middleware to the pipeline

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
