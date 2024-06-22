using DTO;

namespace UtilsLibAbstract
{
    public interface IEmployeeWorkTimeOverlapService
    {
        IEnumerable<PairOfEmployeesModel> CalculateOverlappingDays(
            IEnumerable<EmployeeProjectInputModel> projects);
    }
}