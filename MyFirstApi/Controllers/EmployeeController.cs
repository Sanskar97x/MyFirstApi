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

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody]EmployeeDto employeeDto)
        {
            try
            {
                var result = await employeeService.CreateEmployee(employeeDto);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            try 
            {
                var result = await employeeService.UpdateEmployee(employeeDto);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
