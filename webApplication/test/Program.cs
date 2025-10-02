
using System.Reflection;
using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using BusinessLogic.Models.Accounts;
using BusinessLogic.Services;
using DataAcces.Wrapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Migrations;
using Domain.Models;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore;
using Validators;
using Validators.Interefaces;
using Validators.Validators;
using webApplication.Authorization;
using webApplication.Contracts.room_equipment;
using webApplication.Helpers;




namespace test
{
    public class Program
    {
        public static void Main(string [] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<LDBContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IInstitutionService, InstitutionService>();
            builder.Services.AddScoped<ILectureService, LectureService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IportfolioService, PortfolioService>();
            builder.Services.AddScoped<IRoomEquipmentService, RoomEquipmentService>();
            builder.Services.AddScoped<IStudentsGroupService, StudentsGroupService>();
            builder.Services.AddScoped<ILecturesGroupsService, LectureGroupsService>();
            builder.Services.AddScoped<IUserValidator, CreateUserValidator>();
            builder.Services.AddScoped<IStudentGroupValidator, CreateStudentGroupValidator>();
            builder.Services.AddScoped<IRoomValidator, CreateRoomValidator>();
            builder.Services.AddScoped<IRoomEquipmentValidator, CreateRoomEquipmentValidator>();
            builder.Services.AddScoped<IPortfolioValidator, CreatePortfolioValidator>();
            builder.Services.AddScoped<INotificationValidator, CreateNotificationValidator>();
            builder.Services.AddScoped<ILectureValidator, CreateLectureValidator>();
            builder.Services.AddScoped<ILectureGroupValidator, CreateLecturGroupValidator>();
            builder.Services.AddScoped<IInstitutionValidator, CreateInstitutionValidator>();
            builder.Services.AddScoped<IGroupValidator, CreateGroupValidator>();
            builder.Services.AddScoped<ICityValidator, CreateCityValidator>();
            builder.Services.AddScoped<IAttendanceValidator, CreateAttendanceValidator>();
            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddMapster();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Backend API",
                    Description = "Backend API ASP .NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Internet Shop",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Internet Shop",
                        Url = new Uri("https://example.com/license")
                    },
                });

                
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token.\nExample: \"Bearer eyJhbGciOi...\""
                });

               
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });








            builder.Services.AddSingleton<AppSettings>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var settings = new AppSettings();
                config.GetSection("AppSettings").Bind(settings);
                return settings;
            });

            var app = builder.Build();

            app.UseCors(builder => builder.WithOrigins(new [] { "https://localhost:7174", }).AllowAnyHeader().AllowAnyMethod());

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LDBContext>();
                context.Database.Migrate();
            }

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseRouting(); 

            
            app.UseHttpsRedirection();

           
            app.UseMiddleware<JwtMiddleware>();

            
            app.UseAuthorization();

            
            app.UseMiddleware<ErrorHandlerMiddleware>();

           
            app.MapControllers();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var response = new
                        {
                            error = "Not Found",
                            message = error.Error.Message
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });

            app.Run();
        }
    }
}