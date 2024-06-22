using EmployeesWhoWorkedTogether.DTO;
using Microsoft.AspNetCore.Mvc;
using UtilsLibAbstract;

namespace EmployeesWhoWorkedTogether.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeProjectController(
        IFileReader fileReader,
        IEmployeeWorkTimeOverlapService employeeWorkTimeOverlapService)
            : ControllerBase
    {
        private readonly IFileReader fileReader = fileReader;
        private readonly IEmployeeWorkTimeOverlapService employeeWorkTimeOverlapService = employeeWorkTimeOverlapService;

        [HttpPost("upload")]
        public IEnumerable<ResultData> Upload([FromForm] InputFileDTO fileDto = default!)
        {
            if (fileDto.FormFile == null || fileDto.FormFile.Length == 0)
                throw new Exception("Error");

            var projects = fileReader
                .ReadEmployeeProjects(fileDto.FormFile);

            var overlappingDays = employeeWorkTimeOverlapService
                .CalculateOverlappingDays(projects);

            var maxOverlapPair = overlappingDays
                .OrderByDescending(p => p.DaysWorkedTogether)
                .Select(c => new ResultData
                {
                    EmpID1 = c.EmpID1,
                    EmpID2 = c.EmpID2,
                    ProjectID = c.ProjectID,
                    DaysWorkedTogether = c.DaysWorkedTogether
                });

            return maxOverlapPair;
        }
    }
}
