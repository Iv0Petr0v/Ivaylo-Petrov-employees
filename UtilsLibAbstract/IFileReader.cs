using DTO;
using Microsoft.AspNetCore.Http;

namespace UtilsLibAbstract
{
    public interface IFileReader
    {
        IEnumerable<EmployeeProjectInputModel> ReadEmployeeProjects(IFormFile file);
    }
}