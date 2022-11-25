using System;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public CustomerController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            string query = @"select * from customer";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No Customer.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);

        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetCustomer2(int id)
        {
            string query = @"SELECT * FROM customer
                             WHERE customer_id = @id";

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

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("Customer doesn't exist.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }


        [Route("signup")]
        [HttpPost]
        public async Task<ActionResult<Customer>> CustomerSignUp(CustomerSignUp newCustomer)
        {
            DateTime dob = DateTime.Parse(newCustomer.dob);

            string[] chars = newCustomer.height.Split("'");
            double newHeight = Convert.ToDouble(chars[0]) + (Convert.ToDouble(chars[1]) / 12);

            string query = @"INSERT INTO customer(fname, lname, email, password, height, DOB, tickets_bought)
                             VALUES (@fname, @lname, @email, @password, @height, @DOB, @tickets_bought)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@fname", newCustomer.fname);
                    myCommand.Parameters.AddWithValue("@lname", newCustomer.lname);
                    myCommand.Parameters.AddWithValue("@email", newCustomer.email);
                    myCommand.Parameters.AddWithValue("@password", newCustomer.password);
                    myCommand.Parameters.AddWithValue("@height", newHeight);
                    myCommand.Parameters.AddWithValue("@DOB", dob);
                    myCommand.Parameters.AddWithValue("@tickets_bought", 0);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Successfully sign up!");
        }

        [Route("signin")]
        [HttpPost]
        public async Task<ActionResult> CustomerSignIn(CustomerLogin request)
        {
            string query = @"SELECT * FROM customer
                             WHERE email = @email AND password = @password";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@email", request.email);
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


        [HttpPut]
        public async Task<ActionResult<List<Customer>>> ModifyCustomer(Customer request)
        {
            var dbCustomer = await _context.customer.FirstOrDefaultAsync(h => h.email == request.email);

            if (dbCustomer == null)
            {
                return BadRequest("Customer not existed");
            }

            dbCustomer.password = request.password;
            dbCustomer.height = request.height;
            dbCustomer.DOB = request.DOB;

            await _context.SaveChangesAsync();

            return Ok(await _context.customer.ToListAsync());
        }

        
    }
}

