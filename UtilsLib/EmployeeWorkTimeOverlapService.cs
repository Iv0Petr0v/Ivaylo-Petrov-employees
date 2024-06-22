using DTO;
using UtilsLibAbstract;

namespace UtilsLib
{
    public class EmployeeWorkTimeOverlapService
        : IEmployeeWorkTimeOverlapService
    {
        public IEnumerable<PairOfEmployeesModel> CalculateOverlappingDays(
            IEnumerable<EmployeeProjectInputModel> projects)
        {
            var overlapDict = new Dictionary<(int, int, int), int>();
            var groups = projects.GroupBy(p => p.ProjectID);

            foreach (var projectGroup in groups)
            {
                var employees = projectGroup
                    .ToArray()
                    .AsSpan();

                for (int i = 0; i < employees.Length; i++)
                {
                    if (employees.Length == 1)
                    {
                        int days = CalculateOverlap(employees[i], employees[i]);

                        OverlapDays(overlapDict, employees, i, i, days);
                    }
                    else
                    {
                        for (int j = i + 1; j < employees.Length; j++)
                        {
                            int days = CalculateOverlap(employees[i], employees[j]);

                            OverlapDays(overlapDict, employees, i, j, days);
                        }
                    }
                }
            }

            return overlapDict
                .Select(kv => new PairOfEmployeesModel
                {
                    EmpID1 = kv.Key.Item1,
                    EmpID2 = kv.Key.Item2,
                    ProjectID = kv.Key.Item3,
                    DaysWorkedTogether = kv.Value
                });
        }

        private void OverlapDays(Dictionary<(int, int, int), int> overlapDict, Span<EmployeeProjectInputModel> employees, int i, int j, int days)
        {
            if (days > 0)
            {
                var pair = GetPair(employees, i, j);

                if (!overlapDict.ContainsKey(pair))
                {
                    overlapDict[pair] = 0;
                }

                overlapDict[pair] += days;
            }
        }

        private (int, int, int) GetPair(
            Span<EmployeeProjectInputModel> employees,
            int i,
            int j)
        {
            var minNumber = Math.Min(employees[i].EmpID, employees[j].EmpID);
            var maxNumber = Math.Max(employees[i].EmpID, employees[j].EmpID);
            var projectID = employees[i].ProjectID;

            return (minNumber, maxNumber, projectID);
        }

        private int CalculateOverlap(
            EmployeeProjectInputModel emp1,
            EmployeeProjectInputModel emp2)
        {
            DateTime maxStart = emp1.DateFrom > emp2.DateFrom
                ? emp1.DateFrom
                : emp2.DateFrom;

            DateTime minEnd = emp1.DateTo < emp2.DateTo
                ? emp1.DateTo
                : emp2.DateTo;

            return (minEnd - maxStart).Days + 1 > 0
                ? (minEnd - maxStart).Days + 1
                : 0;
        }
    }
}

