using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    public class UsageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsages()
        {
            string query = @"SELECT * FROM amusement_park.usage_per_day";


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
                        return BadRequest("No data.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }
    }
}

