using Application.Interfaces;
using Application.DependanceInjection;
using Web.Components;
using Infrastructure.DependanceInjection;
using MudBlazor.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

   

//register Service
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddMudServices();

 builder.Services.AddControllers();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer-when-downgrade";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=()";
    // Content-Security-Policy: adjust sources as needed for your app (inline scripts/styles blocked)
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline' https:; style-src 'self' 'unsafe-inline' https:; img-src 'self' data: https:; font-src 'self' https:; connect-src 'self' https: ws: wss:;";
    await next.Invoke();
});

app.UseAntiforgery();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // Account login/logout endpoints


app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
