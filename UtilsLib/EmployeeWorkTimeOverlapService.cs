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

            var groups = projects
                .GroupBy(p => p.ProjectID);

            CalculateOverlapDays(overlapDict, groups);

            return overlapDict
                .Select(kv => new PairOfEmployeesModel
                {
                    EmpID1 = kv.Key.Item1,
                    EmpID2 = kv.Key.Item2,
                    ProjectID = kv.Key.Item3,
                    DaysWorkedTogether = kv.Value
                });
        }

        private static void CalculateOverlapDays(Dictionary<(int, int, int), int> overlapDict, IEnumerable<IGrouping<int, EmployeeProjectInputModel>> groups)
        {
            foreach (var employees in from projectGroup in groups
                                      let employees = projectGroup.ToArray()
                                      select employees)
            {
                if (employees.Length != 1)
                {
                    AddOverlapDays(overlapDict, employees);
                }
                else
                {
                    continue;
                }
            }
        }

        private static void AddOverlapDays(Dictionary<(int, int, int), int> overlapDict, EmployeeProjectInputModel[] employees)
        {
            var from = employees.Min(c => c.DateFrom);
            var to = employees.Max(c => c.DateTo);

            var edays = (to - from).Days + 1 > 0
                ? (to - from).Days + 1
                : 0;

            var firstEmplpyee = employees.FirstOrDefault();
            var lastEmplpyee = employees.LastOrDefault();

            (int, int, int) key = (
                firstEmplpyee.EmpID,
                lastEmplpyee.EmpID,
                firstEmplpyee.ProjectID);

            overlapDict.Add(key, edays);
        }
    }
}

