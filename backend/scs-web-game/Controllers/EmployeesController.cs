using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scs_web_game.DTOs.Employee;
using scs_web_game.Models;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;
namespace scs_web_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(WebGameContext context, ILogger logger, IEmployee em) : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            return await context.Employee.ToListAsync();
        }

        [HttpGet("randomNameListAndEmployee")]
        public async Task<ActionResult<IEnumerable<RandomEmployeeAndNameListDto>>?> GetRandomNameListAndRandomEmployee()
        {
            try
            {
                var getEmployee = await em.GetRandomEmployeeAndNameList();
                
                var response = new RandomEmployeeAndNameListDto
                {
                    EmployeeId = getEmployee.EmployeeId,
                    CorrectName = getEmployee.CorrectName,
                    RandomNameList = getEmployee.RandomNameList.Select(nameTuple => new EmployeeNameDto
                    {
                        FirstName = nameTuple.FirstName,
                        LastName = nameTuple.LastName
                    }).ToList(),
                    Image = getEmployee.Image
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Not enough employees found in the database.");
                return BadRequest("An unexpected error occurred." + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            context.Entry(employee).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var getEmployee = await context.Employee.FindAsync(id);
            if (getEmployee == null)
            {
                return NotFound();
            }

            context.Employee.Remove(getEmployee);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return context.Employee.Any(e => e.EmployeeId == id);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<Employee>>> PostEmployees(IEnumerable<Employee> employees)
        {
            var enumerable = employees as Employee[] ?? employees.ToArray();
            if (!enumerable.Any())
            {
                return BadRequest("The employee list cannot be null or empty.");
            }

            try
            {
                context.Employee.AddRange(enumerable);
                await context.SaveChangesAsync();
                return Ok(new { Message = "Employees successfully created.", Employees = enumerable });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while creating employees.", Error = ex.Message });
            }
        }

        [HttpGet("filterByRole/{role}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role cannot be null or empty.");
            }

            if (!Enum.TryParse<Role>(role, true, out var parsedRole))
            {
                return BadRequest($"Invalid role '{role}'.");
            }

            var employees = await context.Employee
                .Where(e => e.Role == parsedRole)
                .ToListAsync();

            if (!employees.Any())
            {
                return NotFound($"No employees found with the role '{role}'.");
            }

            return Ok(employees);
        }
    }
}
