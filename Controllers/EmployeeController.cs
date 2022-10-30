using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            return Ok(await _context.employee.ToListAsync());
        }



        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(Employee newEmployee)
        {
            var oldEmployee = await _context.employee.FirstOrDefaultAsync(h => h.username == newEmployee.username);

            if (oldEmployee != null)
            {
                return BadRequest("Duplicate Email");
            }

            _context.employee.Add(newEmployee);
            await _context.SaveChangesAsync();

            return Ok(await _context.employee.ToListAsync());
        }

        
    }
}

