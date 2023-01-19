using Application;
using Application.Interface.SPI;

using Carter;

using Infrastructure;

using Infrastructure.DB;
using Infrastructure.Services;

using Presentation.WebApi.Filter;

using Serilog;

//create the logger and setup your sinks, filters and properties
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

// Add different layer services to the container.
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigureApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Enable CORS//Cross site resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

var app = builder.Build();

// Seed Simple Demo Database with Role and Policy
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    try
    {
        var context = service.GetRequiredService<DBGenerator>();
        await context.InitializeAsync();
    }
    catch (Exception ex)
    {
        throw ex;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

// Log all requests
app.UseSerilogRequestLogging();

// app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseExceptionHandler("/error");

// Enforce HTTPS
app.UseHttpsRedirection();

// The ASP.NET Core templates call UseStaticFiles before calling UseAuthorization.
app.UseStaticFiles();

// Enable CORS
app.UseCors("CorsPolicy");

app.UseAuthentication();

// app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();

app.MapCarter();

// mapping health check endpoint
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }