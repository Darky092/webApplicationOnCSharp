
using DataAcces.Wrapper;
using System.Reflection;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Domain.Interfaces;
using BusinessLogic.Services;
using Domain.Models;



namespace test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<LDBContext>(
                options => options.UseNpgsql("Host=localhost;Port=5432;Database=LDB;Username=postgres;Password=root;"));
               
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
