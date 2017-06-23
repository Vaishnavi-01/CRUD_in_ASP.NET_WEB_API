using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace WebApplication4.Controllers
{
    public class CheckController : ApiController
    {
        private DataTable dataTable = new DataTable();

        [HttpGet]
        [Route ("api/doit")]
        public IEnumerable<Employee> Get()
        {
            string CS = ConnectionString();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("Select * from dbo.Employee", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                con.Close();
                da.Dispose();
            }
           
            List<DataRow> list = dataTable.AsEnumerable().ToList();

            foreach (DataRow row in dataTable.Rows)
            {
                yield return new Employee
                {
                    ID = Convert.ToInt32(row["ID"]),
                    FullName = Convert.ToString(row["FullName"]),
                    Age = Convert.ToInt32(row["Age"]),
                    Email = Convert.ToString(row["Email"])
                };
            }
        }

        [HttpPost]
        [Route("api/createnew")]
        public void Post(Employee employee)
        {
     
           
                string CT = ConnectionString();
                using (SqlConnection connec = new SqlConnection(CT))
                {
                    using (SqlCommand cmd = new SqlCommand("insertToDatabase", connec))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                        cmd.Parameters.AddWithValue("@Age", employee.Age);
                        cmd.Parameters.AddWithValue("@Email", employee.Email);
                        
                        connec.Open();
                        cmd.ExecuteNonQuery();
                    }
                    connec.Close();
                }

        }
        
        [HttpGet]
        [Route("api/sendID/{ID}")]
        public IHttpActionResult Get(string ID)
        {
            string CU = ConnectionString();
            using (SqlConnection conn = new SqlConnection(CU))
            {
                using (SqlCommand cmd = new SqlCommand("spGetUserDataByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                }
            }
           
            List<DataRow> list = dataTable.AsEnumerable().ToList();

            Employee emp = new Employee();


            foreach(DataRow row in dataTable.Rows)
            {
                emp.Age = Convert.ToInt32(row["Age"]);
                emp.FullName = Convert.ToString(row["FullName"]);
                emp.ID = Convert.ToInt32(row["ID"]);
                emp.Email = Convert.ToString(row["Email"]);
            }
            return Ok(emp);
        }






        public string ConnectionString()
        {
            return @"Server=.\SQLEXPRESS16; Database= form1; Integrated security =TRUE;";
        }
    }
}



          