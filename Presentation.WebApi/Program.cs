using Application;

using Infrastructure;

using Infrastructure.DB;

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

// builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Log all requests
app.UseSerilogRequestLogging();

// app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

// The ASP.NET Core templates call UseStaticFiles before calling UseAuthorization.
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

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

app.Run();
