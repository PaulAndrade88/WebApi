using Microsoft.EntityFrameworkCore;

namespace WebApiCursos.Models
{
    public class APICoursesContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\PAAG\source\repos\ASPCore_API_REST_plus_EF\ListaCursos\courses.db");

        public APICoursesContext()
        {
            Courses.Add(new Course()
            {
                Id = 1,
                Name = "Azure Devops y VSTS esencial",
                Author = "Paul Armando Andrade García",
                Description = "Conoce los principios basicos",
                Uri = "https://learning"
            });
            Courses.Add(new Course()
            {
                Id = 2,
                Name = "Azure Functions esencial",
                Author = "Paul Armando Andrade García",
                Description = "Conoce los principios basicos",
                Uri = "https://learning"
            });
            Courses.Add(new Course()
            {
                Id = 3,
                Name = "Azure Internet de las cosas",
                Author = "Paul Armando Andrade García",
                Description = "Conoce los principios basicos",
                Uri = "https://learning"
            });
        }
    }
}
