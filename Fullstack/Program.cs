using Microsoft.AspNetCore.ResponseCompression;
using FullstackApp.Hubs;
using Fullstack.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login/Logout";
        options.LogoutPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.Cookie.Name = "COCKIES";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DenyAuthenticated", policy =>
    {
        policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated);
    });
});

var app = builder.Build();
DatabaseHandler.Init(builder.Configuration, app.Environment.IsDevelopment());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseResponseCompression();
app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
