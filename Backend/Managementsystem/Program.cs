using Managementsystem.Helper;
using Managementsystem.Interface;
using Managementsystem.Service;
using Managementsystem.Validaton;
using FluentValidation;
using Managementsystem.Model;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidator>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<Registration>, RegistrationValidator>();
builder.Services.AddScoped<ModelValidator>();

builder.Services.AddScoped<SqlHelper>();
builder.Services.AddScoped<CommonHelper>();
builder.Services.AddScoped<IRegistrationService,RegistrationService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
