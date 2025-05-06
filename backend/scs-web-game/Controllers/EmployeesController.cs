using Microsoft.AspNetCore.Mvc;
using scs_web_game.DTOs.Employee;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;
namespace scs_web_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(ILogger logger, IEmployee em) : ControllerBase
    {
        
        [HttpGet("randomNameListAndEmployee")]
        public async Task<ActionResult<IEnumerable<RandomEmployeeAndNameListDto>>> GetRandomNameListAndRandomEmployee()
        {
            try
            {
                var getEmployee = await em.GetRandomEmployeeAndNameList();
                
                var response = new RandomEmployeeAndNameListDto
                {
                    EmployeeId = getEmployee.EmployeeId,
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
    }
}
