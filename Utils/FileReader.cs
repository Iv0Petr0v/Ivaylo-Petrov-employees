using System.Globalization;
using EmployeesWhoWorkedTogether.Models;
using Microsoft.AspNetCore.Http;
using UtilsAbstract;

namespace Utils
{
    public class FileReader 
        : IFileReader
    {
        public IEnumerable<EmployeeProjectInputModel> ReadEmployeeProjects(IFormFile file)
        {
            List<EmployeeProjectInputModel> projects = [];
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    projects.Add(new EmployeeProjectInputModel
                    {
                        EmpID = int.Parse(parts[0]),
                        ProjectID = int.Parse(parts[1]),
                        DateFrom = DateTime.Parse(parts[2], CultureInfo.InvariantCulture),
                        DateTo = parts[3].Trim() == "NULL"
                            ? DateTime.Today
                            : DateTime.Parse(parts[3], CultureInfo.InvariantCulture)
                    });
                }
            }

            return projects;
        }
    }
}
