using HumanResourcesMVC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient("https://projectwebapi220240824154733.azurewebsites.net/api/Login/login/").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
 {
     ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
 });

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.Run();