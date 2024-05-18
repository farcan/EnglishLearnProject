using EnglishLearningProject.Models;
using Microsoft.EntityFrameworkCore;
using EnglishLearningProject.Extensions;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using EnglishLearningProject.OptionsModel;
using EnglishLearningProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.CustomAddIdentityWithExtensions();
builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));



builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "IdentityCookies";
    opt.LoginPath = new PathString("/Main/Login");
    opt.AccessDeniedPath = new PathString("/Main/Index");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(50);
    opt.SlidingExpiration = true;
});




var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var rotativaPath = Path.Combine(app.Environment.WebRootPath, "Rotativa");
var wkhtmltopdfPath = Path.Combine(app.Environment.WebRootPath, "Rotativa", "wkhtmltopdf.exe");



app.Run();
