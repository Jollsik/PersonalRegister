using Moq;
using PersonalRegister.Models;
using PersonalRegister.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRegisterTests.Services
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void GetAllEmployeesReturnsAllEmployees()
        {
            var cacheService = new Mock<ICacheService>();
            var employeeService = new EmployeeService(cacheService.Object);

            cacheService.Setup(c => c.GetFromCache("allIds")).Returns(new List<string> { "joel@gmail.com", "felldin@outlook.se" });
            cacheService.Setup(c => c.GetFromCache("joel@gmail.com")).Returns(new Employee { Email = "joel@gmail.com", FirstName = "Joel", LastName = "Felldin" });
            cacheService.Setup(c => c.GetFromCache("felldin@outlook.se")).Returns(new Employee { Email = "felldin@outlook.se", FirstName = "Joel", LastName = "Felldin" });


            var result = employeeService.GetAll();

            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void AddEmployee_AddedSuccessfully()
        {
            var cacheService = new Mock<ICacheService>();
            var employeeService = new EmployeeService(cacheService.Object);

            employeeService.AddNewEmployee(new Employee { Email = "felldin@outlook.se", FirstName = "Joel", LastName = "Felldin" });

            cacheService.Verify(c => c.AddToCache("felldin@outlook.se", It.IsAny<Employee>()));
            cacheService.Verify(c => c.AddToCache("allIds", It.IsAny<List<string>>()));
        }
        [Fact]
        public void DeleteEmployee_DeletedSuccessfully()
        {
            var cacheService = new Mock<ICacheService>();
            var employeeService = new EmployeeService(cacheService.Object);

            List<string> allEmployees = new List<string> { "joel@gmail.com", "felldin@outlook.se" };

            cacheService.Setup(c => c.GetFromCache("allIds")).Returns(allEmployees);

            employeeService.DeleteEmployee("felldin@outlook.se");

            cacheService.Verify(c => c.RemoveFromCache("felldin@outlook.se"));
            cacheService.Verify(c => c.AddToCache("allIds", new List<string> { "joel@gmail.com" }));
        }
    }
}