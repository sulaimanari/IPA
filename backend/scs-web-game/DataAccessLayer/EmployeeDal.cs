using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
using scs_web_game.Models;

namespace scs_web_game.DataAccessLayer
{
    public class EmployeeDal(WebGameContext context, ILogger logger)
    {
        private const string ImageFolder = @"D:\scs-fotos";
        public async Task<Employee> FetchRandomEmployee()
        {
            var employees = await context.Employee
                .Where(e => e.Role == Role.ProjectManager)
                .OrderBy(e => Guid.NewGuid())
                .ToListAsync();
            if (employees.Count == 0) throw new InvalidOperationException("No employees found in the database.");
            logger.Information("get random Employee" + employees);
            return employees.First();
        }

        public string CompileAndValidateEmployeeImagePath(Employee employee)
        {
            var imagePath = Path.Combine(ImageFolder, employee.ImgFileName);
            logger.Information("path was created {imagePath} ", imagePath);

            if (File.Exists(imagePath)) return imagePath;
            logger.Warning("Image not found at path: {ImagePath}", imagePath);
            throw new FileNotFoundException($"Image not found at path: {imagePath}");
        }

        public async Task<List<(string FirstName, string LastName)>> GenerateRandomNameList(Employee randomEmployee)
        {
            var employees = await context.Employee
                .Where(e => e.Role == Role.ProjectManager && e.EmployeeId != randomEmployee.EmployeeId)
                .OrderBy(e => Guid.NewGuid())
                .Take(4)
                .ToListAsync();

            if (employees.Count < 4)
                throw new InvalidOperationException("Not enough employees in the database to generate a name list.");

            var nameList = new List<(string FirstName, string LastName)>
            {
                (randomEmployee.FirstName, randomEmployee.LastName)
            };

            nameList.AddRange(employees.Select(e => (e.FirstName, e.LastName)));
            nameList = nameList.OrderBy(_ => Guid.NewGuid()).ToList();
            logger.Information("Random name list generated successfully.");
            return nameList;
        }
    }
}
