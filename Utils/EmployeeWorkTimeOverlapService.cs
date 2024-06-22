using UtilsAbstract;

namespace Utils
{
    public class EmployeeWorkTimeOverlapService 
        : IEmployeeWorkTimeOverlapService
    {
        public IEnumerable<PairOfEmployeesModel> CalculateOverlappingDays(IEnumerable<EmployeeProjectInputModel> projects)
        {
            var overlapDict = new Dictionary<(int, int), int>();

            foreach (var projectGroup in projects.GroupBy(p => p.ProjectID))
            {
                var employees = projectGroup.ToList();
                for (int i = 0; i < employees.Count; i++)
                {
                    for (int j = i + 1; j < employees.Count; j++)
                    {
                        int days = CalculateOverlap(employees[i], employees[j]);
                        if (days > 0)
                        {
                            var pair = (Math.Min(employees[i].EmpID, employees[j].EmpID), Math.Max(employees[i].EmpID, employees[j].EmpID));
                            if (!overlapDict.ContainsKey(pair))
                            {
                                overlapDict[pair] = 0;
                            }
                            overlapDict[pair] += days;
                        }
                    }
                }
            }

            return overlapDict.Select(kv => new PairOfEmployeesModel
            {
                EmpID1 = kv.Key.Item1,
                EmpID2 = kv.Key.Item2,
                DaysWorkedTogether = kv.Value
            });
        }

        private int CalculateOverlap(EmployeeProjectInputModel emp1, EmployeeProjectInputModel emp2)
        {
            DateTime maxStart = emp1.DateFrom > emp2.DateFrom ? emp1.DateFrom : emp2.DateFrom;
            DateTime minEnd = emp1.DateTo < emp2.DateTo ? emp1.DateTo : emp2.DateTo;
            return (minEnd - maxStart).Days + 1 > 0 ? (minEnd - maxStart).Days + 1 : 0;
        }
    }
}

