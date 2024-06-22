using System;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using UtilsLibAbstract;
using static System.Net.Mime.MediaTypeNames;

namespace UtilsLib
{
    public class FileReader
        : IFileReader
    {
        // private static readonly string[] first = ["M-d-yyyy", "dd-MM-yyyy", "MM-dd-yyyy", "M.d.yyyy", "dd.MM.yyyy", "MM.dd.yyyy" , "yyyy.MM.dd"];

        public IEnumerable<EmployeeProjectInputModel> ReadEmployeeProjects(IFormFile file)
        {
            List<EmployeeProjectInputModel> projects = [];

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    string dateFrom;
                    string dateTo;

                    var model = new EmployeeProjectInputModel
                    {
                        EmpID = int.Parse(parts[0]),
                        ProjectID = int.Parse(parts[1])
                    };

                    if (parts.Length < 4)
                    {
                        continue;
                    }

                    if (parts.Length > 4)
                    {

                        if (parts.Length % 2 == 0)
                        {
                            dateFrom = $"{GetDateOrToday(parts[2])}, {GetDateOrToday(parts[3])}";
                            dateTo = $"{GetDateOrToday(parts[4])}, {GetDateOrToday(parts[5])}";

                            model.DateFrom = ParseDate(dateFrom);
                            model.DateTo = ParseDate(dateTo);
                        }
                        else
                        {
                            var isDateFrom = ParseDate(GetDateOrToday(parts[2]));
                          

                        }


                    }
                    else
                    {
                        var dateFromStr = GetDateOrToday(parts[2]);
                        model.DateFrom = ParseDate(dateFromStr);

                        var dateToStr = GetDateOrToday(parts[3]);
                        model.DateTo = ParseDate(dateToStr);
                    };

                    projects.Add(model);
                }
            }

            return projects;
        }

        private string GetDateOrToday(string dateString)
        {
            if (!dateString.Trim().Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return dateString;
            }
            else
            {
                return DateTime.Today.ToString();
            }
        }

        private string ClearText(string txt)
            => txt.Replace("\"", "");

        private DateTime ParseDate(string dateString)
        {
            // var ci = new CultureInfo("en-US");
            // var formats = first.Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();

            var arg = ClearText(dateString);
            var result = DateTime.Parse(arg);

            return result;
        }
    }
}
