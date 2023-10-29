
using Miksoft.QuizWithSignalR.Hub;
using Miksoft.QuizWithSignalR.Storage;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton(configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<InMemoryStorage>();
builder.Services.AddSignalR().AddAzureSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<QuizHub>(QuizHub.EndpointPath);

app.Run();
