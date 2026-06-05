using MyFirstApi.Dto;

namespace MyFirstApi.IService
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync();
    }
}
