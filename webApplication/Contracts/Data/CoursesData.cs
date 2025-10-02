using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.courses;

namespace Contracts.Data;

public class CourseData
{
    public List<Course> GetCourses()
    {
        return new()
        {
            new Course
            {
                Id = 1,
                Title = "Основы C# и .NET",
                Description = "Изучите базовые принципы программирования на C#, работу с переменными, циклами и методами.",
                Duration = "4 недели",
                GroupId = 6
            },
            new Course
            {
                Id = 2,
                Title = "Blazor WebAssembly",
                Description = "Создавайте современные веб-приложения на C# без JavaScript. Практика с реальными проектами.",
                Duration = "6 недель",
                GroupId = 7
            },
            new Course
            {
                Id = 3,
                Title = "SQL и базы данных",
                Description = "Научитесь работать с базами данных, писать запросы, проектировать таблицы и оптимизировать производительность.",
                Duration = "3 недели",
                GroupId = 8
            }
        };
    }
}