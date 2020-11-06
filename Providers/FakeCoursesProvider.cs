using WebApiCursos.Interfaces;
using WebApiCursos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using WebApiCursos.DAL;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using System.Data;

namespace WebApiCursos.Providers
{
    public class FakeCoursesProvider : ICoursesProvider
    {
        private readonly APICoursesContext repo = new APICoursesContext();
        private readonly IConfiguration _configuration;
        private readonly DataAccessModule _dam;

        public FakeCoursesProvider (IConfiguration config)
        {
            _configuration = config;
            _dam = new DataAccessModule(_configuration);
        }
                        
        public Task<ICollection<Course>> GetAllAsync()
        {
            return Task.FromResult((ICollection<Course>)
                _dam.GetCourses("BD_Destino", "spGetAllCourses", new ArrayList())
                .Result
                .OrderBy(x => x.Id)
                .ToList());
        }

        public Task<Course> GetAsync(int id)
        {            
            return Task.FromResult(_dam.GetCourses("BD_Destino", "spGetCourseById",
                new ArrayList { 
                    new SqlParameter { 
                        ParameterName = "courseId", Value = id, DbType = DbType.Int32 } 
                })
                .Result[0]);            
        }

        public Task<ICollection<Course>> SearchAsync(string search)
        {
            return Task.FromResult((ICollection<Course>)
                _dam.GetCourses("BD_Destino", "spGetCourseBySearchFilter",
                new ArrayList {
                    new SqlParameter {
                        ParameterName = "textChain", Value = search, DbType = DbType.Int32 } 
                })
                .Result
                .OrderBy(c => c.Id)
                .ToList());
        }

        public Task<(bool IsSuccess, int? Id)> AddAsync(Course course)
        {
            //course.Id = repo.Courses.Local.Max(x => x.Id) + 1;
            //repo.Courses.Add(course);

            var paramList = new ArrayList {
                    new SqlParameter {
                        ParameterName = "courseName", Value = course.Name, DbType = DbType.String },
                    new SqlParameter {
                        ParameterName = "courseDescription", Value = course.Description, DbType = DbType.String },
                    new SqlParameter {
                        ParameterName = "courseAuthor", Value = course.Author, DbType = DbType.String },
                    new SqlParameter {
                        ParameterName = "courseUri", Value = course.Uri, DbType = DbType.String }
                    };

            var newId = _dam.Fill("BD_Destino", "spAddCourse", paramList).ExtendedProperties["NewId"];
            
            
            return Task.FromResult((true, (int?)newId));
        }

        public Task<bool> UpdateAsync(int id, Course course)
        {
            var paramList = new ArrayList {
                new SqlParameter {
                    ParameterName = "courseId", Value = course.Id, DbType = DbType.Int32 },
                new SqlParameter {
                    ParameterName = "courseName", Value = course.Name, DbType = DbType.String },
                new SqlParameter {
                        ParameterName = "courseDescription", Value = course.Description, DbType = DbType.String },
                new SqlParameter {
                        ParameterName = "courseAuthor", Value = course.Author, DbType = DbType.String },
                new SqlParameter {
                        ParameterName = "courseUri", Value = course.Uri, DbType = DbType.String }
                    };

            var result = _dam.Fill("BD_Destino", "spUpdateCourseById", paramList);

            if (result != null)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}
