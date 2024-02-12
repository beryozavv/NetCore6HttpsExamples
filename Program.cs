using System.Security.Authentication;
using NetCore6Https.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ConfigureHttpsDefaults(adapterOptions =>
    {
        adapterOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
    });
});

builder.WebHost.ConfigureAppConfiguration((context, configurationBuilder) =>
{
    configurationBuilder.AddProtectedJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
        new[] { "Kestrel:Certificates:Default:Password"},
        optional: false,
        reloadOnChange: false);
});

builder.WebHost.UseKestrel(options => { options.UseSystemd(); });

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, policyBuilder =>
        policyBuilder.AllowAnyOrigin());
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();