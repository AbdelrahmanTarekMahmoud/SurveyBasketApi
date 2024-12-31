

using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/dashboard", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangFireSettings:UserName"),
            Pass = app.Configuration.GetValue<string>("HangFireSettings:Password"),
        }
    ],
    //DashboardTitle = "Survey Basket DashBoard for Background Jobs"
});

//first we need to get the services
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var AllServices = scopeFactory.CreateScope();
var NotificationService = AllServices.ServiceProvider.GetRequiredService<INotificationService>();
RecurringJob.AddOrUpdate("NewPollsNotification", () => NotificationService.NewPollsNotification(), Cron.Daily);
//must be before Authorization
app.UseCors("AllowAll");
app.UseAuthorization();


app.MapControllers();
app.UseExceptionHandler();

app.UseRateLimiter();
//"health" is the path
app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
