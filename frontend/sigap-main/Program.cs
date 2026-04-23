using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Shared data protection key folder (outside app folder so persists across rebuilds)
var projectRoot = Directory.GetParent(builder.Environment.ContentRootPath)!.FullName;
var keysPath = Path.Combine(projectRoot, "SharedKeys");
Directory.CreateDirectory(keysPath);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddControllers()
        // .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);
        .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true)
        .AddJsonOptions(o => o.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping);
}
else
{
    builder.Services.AddControllers();
}

// MVC / Controllers
builder.Services.AddControllersWithViews(options =>
{
    // Require authentication globally
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

// Http and services
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ApiService>();
builder.Services.AddScoped<ApiService>();

// Authorization infrastructure (custom providers/handlers)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminBypassHandler>();

// DataProtection: store keys in shared folder so cookie encryption survives hot reload and container restarts
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("LexaApp");

// Authentication: cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Error403";
        options.Cookie.HttpOnly = true;
        // For development on localhost with https dev certs, use SameAsRequest or Conditional:
        if (builder.Environment.IsDevelopment())
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        else
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // set SlidingExpiration=false to control refresh via our refresh endpoint
        options.SlidingExpiration = false;
        // options.ExpireTimeSpan is used only when sliding expiration is enabled
    });

// builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permission:transaksi:riwayat:read", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.HasClaim("Permission", "transaksi:riwayat_transaksi:read") ||
            ctx.User.HasClaim("Permission", "transaksi:riwayat_stock:read")
        ));

    options.AddPolicy("Permission:transaksi:addtransaksi", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.HasClaim("Permission", "transaksi:permintaan") ||
            ctx.User.HasClaim("Permission", "transaksi:pemasukan")
        ));
});


// Session Service
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
// app.UseTokenExpiryCheck();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
