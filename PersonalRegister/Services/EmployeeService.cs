using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PersonalRegister.Models;

namespace PersonalRegister.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICacheService _cacheService;
        public EmployeeService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public void AddNewEmployee(Employee employee)
        {
            _cacheService.AddToCache(employee.Email, employee);
            AddToAllIds(employee.Email);
        }
        private void AddToAllIds(string key)
        {
            var allIds = (List<string>)_cacheService.GetFromCache("allIds");

            if(allIds == null)
            {
                allIds = new List<string> { key };
            }
            else
            {
                allIds.Add(key);
            }

            _cacheService.AddToCache("allIds", allIds);
        }
        public bool EmployeeExists(string email)
        {
            if(_cacheService.GetFromCache(email) != null)
            {
                return true;
            }
            return false;
        }
        public List<Employee> GetAll()
        {
            var allEmployees = new List<Employee>();
            var allIds = (List<string>)_cacheService.GetFromCache("allIds");

            if(allIds != null)
            {
                foreach(var id in allIds)
                {
                    var employee = (Employee)_cacheService.GetFromCache(id);

                    if(employee != null)
                    {
                        allEmployees.Add(employee);
                    }
                }
            }
            return allEmployees;
        }
        public void DeleteEmployee(string email)
        {
            _cacheService.RemoveFromCache(email);
            RemoveFromAllIds(email);
        }
        private void RemoveFromAllIds(string email)
        {
            var allIds = (List<string>)_cacheService.GetFromCache("allIds");

            allIds.Remove(email);

            _cacheService.AddToCache("allIds", allIds);

        }
    }
}
