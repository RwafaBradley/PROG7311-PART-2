// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database context is the organised filing system used by the back office.

using GLMS.Web.Data;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GlmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "GLMS.Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrManager", policy => policy.RequireRole("Admin", "Manager"));
});

builder.Services.AddSingleton<InMemoryStore>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IFileValidationService, FileValidationService>();
builder.Services.AddSingleton<IContractFactory, ContractFactory>();
builder.Services.AddScoped<IAppDataService, AppDataService>();
builder.Services.AddSingleton<INotificationObserver, DashboardNotificationObserver>();
builder.Services.AddSingleton<INotificationObserver, AuditNotificationObserver>();
builder.Services.AddSingleton<INotificationSubject, NotificationCenter>();
builder.Services.AddSingleton<ICurrencyConversionStrategy>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var useLive = config.GetValue<bool>("ExternalApis:UseLiveApi");
    return useLive ? new PlaceholderApiCurrencyStrategy(config) : new FixedRateCurrencyStrategy(config);
});
builder.Services.AddScoped<IServiceRequestWorkflowService, ServiceRequestWorkflowService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<GlmsDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
