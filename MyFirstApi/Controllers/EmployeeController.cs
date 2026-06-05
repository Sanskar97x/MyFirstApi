using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Services;
using MyFirstApi.Dto;
using MyFirstApi.Generic_Collections;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService): ControllerBase
    {

        [HttpGet]         
        public async Task <IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await employeeService.GetAllEmployeeAsync();

                if(!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDto>>.Failure(null, "No Employees Found"));
                }
                return Ok(ResponseResult<List<EmployeeDto>>.Success(result.Item2, "Employee Found"));
            }
            catch 
            {
                throw;
            }
        }
    }
}
