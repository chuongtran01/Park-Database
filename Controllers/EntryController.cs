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
    public class EntryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EntryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("{month}/{year}")]
        [HttpGet]
        public async Task<ActionResult> GetEntryBasedOnDate(string month, string year)
        {
            string query = @"SELECT c.fname, c.lname, t.ticket_name 
                             FROM amusement_park.customer as c, amusement_park.entry as e, amusement_park.ticket as t
                             WHERE monthname(e.entry_date) = @month
                             AND YEAR(e.entry_date) = @year
                             AND e.customer_id = c.customer_id
                             AND e.ticket_id = t.ticket_id";

            int y = Convert.ToInt16(year);

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@month", month);
                    myCommand.Parameters.AddWithValue("@year", y);


                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("Entry doesn't exist.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }


    }
}

