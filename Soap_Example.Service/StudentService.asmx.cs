using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services;

namespace Soap_Example.Service
{
    /// <summary>
    /// Summary description for StudentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class StudentService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public int Insert(Student obj)
        {
            using (IDbConnection db = new SqlConnection
                   (ConfigurationManager.ConnectionStrings["SchollDbConnection"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                //new student record
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.AddDynamicParams(new { obj.FullName, obj.Email, obj.Address, obj.Gender, obj.BirthDay, obj.ImageUrl });
                int result = db.Execute("sp_Students_Insert", parameters, commandType: CommandType.StoredProcedure);
                if (result != 0)
                    return parameters.Get<int>("@StudentID");
                return 0;
            }
        }

        [WebMethod]
        public bool Update(Student obj)
        {
            using (IDbConnection db = new SqlConnection
                   (ConfigurationManager.ConnectionStrings["SchollDbConnection"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                //update student record
                int result = db.Execute("sp_Students_Update", new { obj.StudentID, obj.FullName, obj.Email, obj.Address, obj.Gender, obj.BirthDay, obj.ImageUrl }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        [WebMethod]
        public List<Student> GetAll()
        {
            using (IDbConnection db =
                new SqlConnection(ConfigurationManager.ConnectionStrings["SchollDbConnection"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                return db.Query<Student>("SELECT * FROM STUDENTS", commandType: CommandType.Text).ToList();
            }
        }

        [WebMethod]
        public bool Delete(int studentId)
        {
            using (IDbConnection db =
              new SqlConnection(ConfigurationManager.ConnectionStrings["SchollDbConnection"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("Delete from Students where StudentID=@StudentID", new { studentId }, commandType: CommandType.Text);
                return result != 0;
            }
        }
    }
}