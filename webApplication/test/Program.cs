
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
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });




            TypeAdapterConfig<RegisterRequest, user>
    .NewConfig()
    .Map(dest => dest.name, src => src.Name)
    .Map(dest => dest.surname, src => src.Surname)
    .Map(dest => dest.patronymic, src => src.Patronymic)
    .Map(dest => dest.email, src => src.Email)
    .Map(dest => dest.telephonnumber, src => src.TelephoneNumber ?? "0000000000")
    .Map(dest => dest.AcceptTerms, src => src.AcceptTerms)
    .Map(dest => dest.passwordhash, src => (string)null)
    .Map(dest => dest.RoleT, src => RoleT.User) 
    .Map(dest => dest.role, src => "User");

            TypeAdapterConfig<CreateRequest, user>
    .NewConfig()
    .Map(dest => dest.name, src => src.Name)
    .Map(dest => dest.surname, src => src.Surname)
    .Map(dest => dest.patronymic, src => src.Patronymic)
    .Map(dest => dest.email, src => src.Email)
    .Map(dest => dest.passwordhash, src => (string)null)
    .Map(dest => dest.RoleT, src => src.RoleT ?? RoleT.User)
    .Map(dest => dest.role, src => (src.RoleT ?? RoleT.User).ToString());

            TypeAdapterConfig<UpdateRequest, user>
                .NewConfig()
                .Map(dest => dest.name, src => src.Name)
                .Map(dest => dest.surname, src => src.Surname)
                .Map(dest => dest.patronymic, src => src.Patronymic)
                .Map(dest => dest.email, src => src.Email)
                .Map(dest => dest.telephonnumber, src => src.TelephoneNumber)
                .AfterMapping((src, dest) =>
                {
                    if (src.RoleT.HasValue)
                    {
                        dest.RoleT = src.RoleT.Value;
                        dest.role = src.RoleT.Value.ToString();
                    }
                })
                .Ignore(dest => dest.passwordhash)
                .IgnoreNullValues(true);

            TypeAdapterConfig<user, AccountResponse>
                .NewConfig()
                .Map(dest => dest.Id, src => src.userid)
                .Map(dest => dest.Name, src => src.name)
                .Map(dest => dest.Surname, src => src.surname)
                .Map(dest => dest.Patronymic, src => src.patronymic)
                .Map(dest => dest.Email, src => src.email)
                .Map(dest => dest.Role, src => src.role) 
                .Map(dest => dest.Created, src => src.Created ?? DateTime.UtcNow)
                .Map(dest => dest.Updated, src => src.Updated)
                .Map(dest => dest.IsVerified, src => src.IsVerified);




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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }





            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMiddleware<JwtMiddleware>();

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