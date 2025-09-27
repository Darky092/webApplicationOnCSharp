
using System.Reflection;
using BusinessLogic.Services;
using DataAcces.Wrapper;
using Domain.Interfaces;
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
using webApplication.Contracts.room_equipment;



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


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LSM - Learning Management System API",
                    Description = "Learning Management System API It is used to create classes and mark attendance. It can also be used to attend additional classes.",
                    Contact = new OpenApiContact
                    {
                        Name = "Contacts",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                String xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LDBContext>();
                context.Database.Migrate();
            }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();

            app.UseAuthorization();

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