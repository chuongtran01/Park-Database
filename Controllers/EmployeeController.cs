using System;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Route("role/{title}")]
        [HttpGet]
        public async Task<ActionResult> GetByRole(string title)
        {
            string query = @"select * from amusement_park.employee where job_title=(@role)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@role", title);
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No employee exist.");
                    }
                    

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from amusement_park.employee";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);

        }

        [Route("signin")]
        [HttpPost]
        public async Task<ActionResult> EmployeeLogin(EmployeeLogin request)
        {
            string query = @"SELECT * FROM amusement_park.employee
                             WHERE username = @username AND password = @password";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@username", request.username);
                    myCommand.Parameters.AddWithValue("@password", request.password);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("Email or password is incorrect.");
                    }
                    

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }


        [Route("position")]
        [HttpGet]
        public async Task<ActionResult<Employee>> EmployeePosition(EmployeeLogin request)
        {
            return Ok("Updated!");
        }


        [Route("{id}")]
        [HttpDelete]
        public JsonResult EmployeeDelete(int id)
        {
            string query = @"DELETE FROM employee
                             WHERE employee_id = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(Get());

        }


        [Route("update")]
        [HttpPost]
        public JsonResult UpdateEmployee(EmployeeUpdate request)
        {
            int id = Convert.ToInt32(request.employee_id);
            int sup_id = Convert.ToInt32(request.newSupervisorID);
            DateTime DOB = DateTime.Parse(request.newDOB);

            string query = @"UPDATE employee
                             SET username = @username, fname = @fname, lname = @lname, DOB = @DOB, supervisor_id = @sup_id, job_title = @job_title 
                             WHERE employee_id = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@username", request.newUsername);
                    myCommand.Parameters.AddWithValue("@fname", request.newFirstName);
                    myCommand.Parameters.AddWithValue("@lname", request.newLastName);
                    myCommand.Parameters.AddWithValue("@DOB", DOB);
                    myCommand.Parameters.AddWithValue("@sup_id", sup_id);
                    myCommand.Parameters.AddWithValue("@job_title", request.newJobTitle);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(Get());

        }


        [HttpPost]
        public JsonResult AddEmployee(EmployeeAdd newEmployee)
        {
            DateTime dob = DateTime.Parse(newEmployee.DOB);
            string query = @"INSERT INTO employee(fname, lname, DOB, supervisor_id, job_title, username, password)
                             VALUES (@fname, @lname, @DOB, @supervisor_id, @job_title, @username, @password)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@fname", newEmployee.fname);
                    myCommand.Parameters.AddWithValue("@lname", newEmployee.lname);
                    myCommand.Parameters.AddWithValue("@DOB", dob);
                    myCommand.Parameters.AddWithValue("@supervisor_id", newEmployee.supervisor_id);
                    myCommand.Parameters.AddWithValue("@job_title", newEmployee.job_title);
                    myCommand.Parameters.AddWithValue("@username", newEmployee.username);
                    myCommand.Parameters.AddWithValue("@password", newEmployee.password);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(Get());

        }


        
    }
}

