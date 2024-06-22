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
                .GroupBy(p => p.ProjectID)
                .ToList();

            var result = groups
                .Join(groups,
                      b => new { b.Key },
                      a => new { a.Key },
                      (b, a) => b)
                .GroupBy(b => b.Key)
                .Select(g => new
                {
                    Empl1 = g.ToArray().FirstOrDefault().FirstOrDefault().EmpID,
                    Empl2 = g.ToArray().LastOrDefault().LastOrDefault().EmpID,
                    ProjectID = g.Key,
                    DateFrom = g.Min(b => b?.FirstOrDefault()?.DateFrom),
                    DateTo = g.Max(b => b?.FirstOrDefault()?.DateTo),
                    Val = (g.Max(b => b?.FirstOrDefault()?.DateTo) - g.Min(b => b?.FirstOrDefault()?.DateFrom))?.Days + 1 > 0
                     ? (g.Max(b => b?.FirstOrDefault()?.DateTo) - g.Min(b => b?.FirstOrDefault()?.DateFrom))?.Days + 1
                     : 0
                })
                .ToArray()
                .OrderBy(c => c.Val)
                .Select(c => new PairOfEmployeesModel
                {
                    EmpID1 = c.Empl1,
                    EmpID2 = c.Empl2,
                    ProjectID = c.ProjectID,
                    DaysWorkedTogether = (int)((c.Val == null) ? 0 : c.Val),
                });

            return result;
        }
    }
}
