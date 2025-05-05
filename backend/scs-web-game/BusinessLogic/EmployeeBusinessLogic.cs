using scs_web_game.DataAccessLayer;
using scs_web_game.DTOs.Employee;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;
namespace scs_web_game.BusinessLogic
{
    public class EmployeeBusinessLogic(ILogger logger, EmployeeDal employeeDal): IEmployee
    {
       public async Task<RandomEmployeeAndNameListDto> GetRandomEmployeeAndNameList()
        {
            try
            {
                logger.Information("Fetching a random employee...");
                var randomEmployee = await employeeDal.FetchRandomEmployee();
                logger.Information("Generating list of random names...");

                var nameTuples = await employeeDal.GenerateRandomNameList(randomEmployee);

                var nameList = nameTuples.Select(tuple => new EmployeeNameDto
                {
                    FirstName = tuple.FirstName,
                    LastName = tuple.LastName
                }).ToList();

                var imagePath = employeeDal.CompileAndValidateEmployeeImagePath(randomEmployee);
                var imageBytes = await File.ReadAllBytesAsync(imagePath);
                var correctName = $"{randomEmployee.FirstName} {randomEmployee.LastName}";

                logger.Information("Successfully fetched random employee with ID: {EmployeeId}", randomEmployee.EmployeeId);

                return new RandomEmployeeAndNameListDto
                {
                    EmployeeId = randomEmployee.EmployeeId,
                    Image = imageBytes,
                    RandomNameList = nameList,
                    CorrectName = correctName
                };
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, "Not enough employees found in the database.");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected error occurred.");
                throw;
            }
        }
    }
}
