using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EventEase;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// In-memory event service
builder.Services.AddSingleton<EventEase.Services.EventService>();

// Toast service for lightweight notifications
builder.Services.AddSingleton<EventEase.Services.ToastService>();

// User session service (loads/stores session in localStorage)
builder.Services.AddScoped<EventEase.Services.UserSessionService>();

await builder.Build().RunAsync();
