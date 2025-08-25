using PersonalRegister.Models;

namespace PersonalRegister.Services
{
    public interface IEmployeeService
    {
        void AddNewEmployee(Employee employee);
        bool EmployeeExists(string email);
        List<Employee> GetAll();
        void DeleteEmployee(string email);
    }
}
