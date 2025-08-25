using Microsoft.AspNetCore.Mvc;
using PersonalRegister.Models;
using PersonalRegister.Services;

namespace PersonalRegister.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }
        [HttpGet]
        public ActionResult<ResponseModel<List<Employee>>> GetAll()
        {
            var allEmployees = _employeeService.GetAll();

            return Ok(new ResponseModel<List<Employee>>
            {
                Success = true,
                Message = "All employees!",
                Data = allEmployees
            });
        }
        [HttpPost]
        public ActionResult<ResponseModel<Employee>> Post(Employee employee)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (!_employeeService.EmployeeExists(employee.Email))
                    {
                        _employeeService.AddNewEmployee(employee);
                        return Ok(new ResponseModel<Employee>
                        {
                            Success = true,
                            Message = "Employee created!",
                            Data = employee
                        });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<Employee>
                        {
                            Success = false,
                            Message = "Employee with this Email already exists",
                            Data = employee
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);

                    return StatusCode(500, new ResponseModel<Employee>
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again.",
                        Data = employee
                    });
                }
            }
            else
            {
                return BadRequest(new ResponseModel<Employee>
                {
                    Success = false,
                    Message = "Model state is invalid",
                    Data = employee
                });
            }
        }
        [HttpDelete]
        public ActionResult<ResponseModel<bool>> Delete(string email)
        {
            if (_employeeService.EmployeeExists(email))
            {
                _employeeService.DeleteEmployee(email);
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Employee deleted"
                });
            }
            else
            {
                return NotFound(new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Employee with email does not exist"
                });

            }
        }
    }
}
