

using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Presistence;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDependencies(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//must be before Authorization
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
app.Run();
