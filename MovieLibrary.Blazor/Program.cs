using MovieLibrary.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseHttpsRedirection();


    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
}

app.MapGet("/home", () => "Hello World!");

app.Run();
