using MyFirstApi.Data;
using MyFirstApi.IService;
using MyFirstApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApi.Services
{
    public class EmployeeService(AppDbContext _context) : IEmployeeService
    {
        public async Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync()
        {
            try
            {
                return new Tuple<int, List<EmployeeDto>>(1, await _context.Employees.AsNoTracking().Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate,
                    Department = x.Department,
                    DOB = x.DOB,
                    Name = x.Name,
                    EmailAddress = x.EmailAddress,
                    Postition = x.Postition
                }).ToListAsync());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee)
        {
            var existing = await _context.Employees.AnyAsync(x => x.EmailAddress == employee.EmailAddress);

            if (existing)
            {
                return new Tuple<int, string>(0,"Employee Already exist with same Email Id");
            }

            await _context.Employees.AddAsync(new Entities.Employee
            {
                Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                LastModifiedDate = null,
                Department = employee.Department,
                DOB = employee.DOB,
                Name = employee.Name,
                EmailAddress = employee.EmailAddress,
                Postition = employee.Postition
            });
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1, "Employee created successfully");
        }
    }
}
