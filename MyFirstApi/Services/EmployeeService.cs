using MyFirstApi.Data;
using MyFirstApi.IService;
using MyFirstApi.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
                    Position = x.Position
                }).ToListAsync());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, string>> CreateEmployee([FromBody]EmployeeDto employee)
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
                Position = employee.Position
            });
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1, "Employee created successfully");
        }
    }
}
