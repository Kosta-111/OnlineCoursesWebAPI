using Data;
using Core.Interfaces;
using Core.MapperProfiles;
using OnlineCoursesWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesWebAPI;
using FluentValidation;
using FluentValidation.AspNetCore;
using Core.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("LocalDb")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<OnlineCoursesDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<OnlineCoursesDbContext>();

builder.Services.AddAutoMapper(typeof(AppProfile));
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();

//SMTP
var smtpConfig = builder.Configuration.GetSection("SmtpConfig").Get<SmtpConfiguration>()
    ?? throw new Exception("missing 'SmtpConfig' configuration item in appsettings.json");
builder.Services.AddSingleton(smtpConfig);
builder.Services.AddSingleton<ISendMailService, SendMailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
