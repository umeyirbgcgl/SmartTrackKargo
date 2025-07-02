using Microsoft.AspNetCore.Server.IIS;
using SmartTrackKargo.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Swagger/OpenAPI yapılandırması
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "SmartTrackKargo API", 
        Version = "v1",
        Description = "SmartTrackKargo için REST API dokümantasyonu",
        Contact = new OpenApiContact
        {
            Name = "SmartTrackKargo Team",
            Email = "contact@smarttrackkargo.com"
        }
    });
    c.EnableAnnotations();
});

// Service registrations
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<PredictionService>();
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<AIAgent>();

// IIS Configuration
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = true;
});

var app = builder.Build();

// Swagger UI'ı her zaman etkinleştir
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartTrackKargo API V1");
    c.RoutePrefix = string.Empty; // Swagger UI'ı ana sayfada göster
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});

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

// API ve MVC route'larını ekle
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
