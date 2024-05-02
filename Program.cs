using Microsoft.EntityFrameworkCore;

using IceSync;
using IceSync.Services;
using IceSync.DataAccess;
using IceSync.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get the connection string from the configuration
string? connString = builder.Configuration.GetConnectionString("IceSyncConnectionString");

// Add DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>{
    options.UseSqlServer(connString);
});

// Add services
builder.Services.AddScoped<IUniversalLoaderService, UniversalLoaderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IStorageService, StorageService>();
builder.Services.AddTransient<JwtHandler>();
builder.Services.AddHostedService<SyncService>();

// Bind configuration settings for Universal Loader API credentials to the DI service container. 
builder.Services.Configure<UniversalLoaderApiOptoins>(builder.Configuration.GetSection(UniversalLoaderApiOptoins.Credentials));

// Add typed HttpClient as trarnsient
builder.Services.AddHttpClient<IUniversalLoaderService, UniversalLoaderService>(client =>
{    
    // Add the BaseAddress to the client obtained from the config file. 
    string? universalLoaderApiBaseUrl = builder.Configuration.GetValue("ApiBaseUrl", "https://api-test.universal-loader.com");
    if(string.IsNullOrWhiteSpace(universalLoaderApiBaseUrl))
    {
        universalLoaderApiBaseUrl = "https://api-test.universal-loader.com";
    }

    client.BaseAddress = new Uri(universalLoaderApiBaseUrl);    
}).AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpClient("AuthClient", client =>
{    
    // Add the BaseAddress to the client obtained from the config file. 
    string? universalLoaderApiBaseUrl = builder.Configuration.GetValue("ApiBaseUrl", "https://api-test.universal-loader.com");
    if(string.IsNullOrWhiteSpace(universalLoaderApiBaseUrl))
    {
        universalLoaderApiBaseUrl = "https://api-test.universal-loader.com";
    }

    client.BaseAddress = new Uri(universalLoaderApiBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Handles exceptions globally via middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
