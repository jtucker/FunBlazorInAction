open System.Net.Http


#nowarn "0020"

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Fun.Blazor
open ModeratelySized

let builder = WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())

#if DEBUG
builder.AddFunBlazor("#app", html.hotReloadComp(app, "ModeratelySized.App.app"))
#else
builder.AddFunBlazor("#app", app)
#endif

builder.Services.AddScoped<HttpClient>(fun _ ->
    new HttpClient(BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)) )
builder.Services.AddFunBlazorWasm()
builder.Build().RunAsync()
