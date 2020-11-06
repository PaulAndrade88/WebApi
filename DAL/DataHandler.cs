using WebApiCursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCursos.DAL
{
    public class DataHandler
    {
        public List<Course> DataReaderHandler(SqlDataReader data)
        {
            List<Course> list = new List<Course>(); 

            while(data.Read())
            {
                list.Add(
                    new Course { 
                        Id = (int)data["courseId"],
                        Author = (string)data["courseName"],
                        Description = (string)data["courseDescription"],
                        Name = (string)data["courseAuthor"],
                        Uri = (string)data["courseUri"]
                    });
            }


            return list;
        }
    }
}
