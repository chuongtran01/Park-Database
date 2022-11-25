using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace amusement_park.Controllers
{


    [Route("api/[controller]")]
    public class DateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> GetDates()
        {
            string query = @"select * from amusement_park.date";

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
                        return BadRequest("No data exist.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("{request}")]
        [HttpGet]
        public async Task<ActionResult> GetDate(string request)
        {
            DateTime date = DateTime.Parse(request);
            string query = @"select * from amusement_park.date where amusement_park.date.date = @date";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@date", date);
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data exist.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("{start_time}/{end_time}")]
        [HttpGet]
        public async Task<ActionResult> GetDateInRange(string start_time, string end_time)
        {
            DateTime start = DateTime.Parse(start_time);
            DateTime end = DateTime.Parse(end_time);

            string query = @"SELECT * FROM amusement_park.date
                             WHERE date BETWEEN @start_time AND @end_time";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@start_time", start);
                    myCommand.Parameters.AddWithValue("@end_time", end);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No date exist.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("data/{month}/{year}")]
        [HttpGet]
        public async Task<ActionResult> GetDataInAMonth(string month, int year)
        {

            string query = @"SELECT monthname(date) as month,
                            EXTRACT(YEAR FROM date) as year,
                            AVG(entries) AS average_entries,
                            AVG(revenue) AS average_revenue,
                            AVG(new_breakdowns) AS average_breakdown,
                            SUM(entries) AS total_entries,
                            sum(revenue) AS total_revenue,
                            sum(new_breakdowns) AS total_breakdowns,
                            sum(rainy_date) AS total_rainy_days
                            FROM amusement_park.date
                            WHERE (EXTRACT(YEAR FROM date) = @year) AND (monthname(date) = @month)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@year", year);
                    myCommand.Parameters.AddWithValue("@month", month);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("range/{start}/{end}")]
        [HttpGet]
        public async Task<ActionResult> GetDataInRange(string start, string end)
        {

            string query = @"SELECT DATE(@start) as start_date,
                            DATE(@end) as end_date,
                            AVG(entries) AS average_entries,
                            AVG(revenue) AS average_revenue,
                            AVG(new_breakdowns) AS average_breakdowns,
                            sum(entries) AS total_entries,
                            sum(revenue) AS total_revenue,
                            sum(new_breakdowns) AS total_breakdowns,
                            sum(rainy_date) AS total_rainy_days
                            FROM amusement_park.date
                            WHERE (date between DATE(@start) and DATE(@end))";
             
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@start", start);
                    myCommand.Parameters.AddWithValue("@end", end);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("overall")]
        [HttpGet]
        public async Task<ActionResult> GetDataOverall()
        {

            string query = @"SELECT AVG(t.avge) AS average_entries,
                            TRUNCATE(AVG(t.avgr),2) AS average_revenue,
                            AVG(t.avgb) AS average_breakdowns,
                            AVG(t.avgrd) AS average_rainouts FROM
                            (SELECT MONTH(date), YEAR(date),
                            SUM(entries) AS avge,
                            SUM(revenue) AS avgr,
                            SUM(new_breakdowns) AS avgb,
                            SUM(rainy_date) AS avgrd FROM amusement_park.date
                            WHERE MONTH(date) < MONTH(CURDATE()) AND YEAR(date) <= YEAR(CURDATE())
                            GROUP BY YEAR(date), MONTH(date)) t";

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
                        return BadRequest("No data found.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("overall/range/{start}/{end}")]
        [HttpGet]
        public async Task<ActionResult> GetDataOverallInRange(string start, string end)
        {

            DateTime start_time = DateTime.Parse(start);
            DateTime end_time = DateTime.Parse(end);
            string query = @"select * from date
                        where date between DATE(@start_time) and DATE(@end_time)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@start_time", start_time);
                    myCommand.Parameters.AddWithValue("@end_time", end_time);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("overall/year/{year}")]
        [HttpGet]
        public async Task<ActionResult> GetDataYear(string year)
        {
            string query = @"SELECT MONTHNAME(date) AS month, YEAR(date) AS year,
                            SUM(entries) AS entries,
                            SUM(revenue) AS revenue,
                            SUM(new_breakdowns) AS breakdowns,
                            SUM(rainy_date) AS rainouts FROM amusement_park.date
                            WHERE YEAR(date) = @year
                            GROUP BY YEAR(date), MONTH(date)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@year", year);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }
    }
}

