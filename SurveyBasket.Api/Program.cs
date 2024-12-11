

using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SurveyBasket.Api.Presistence;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
app.Run();
