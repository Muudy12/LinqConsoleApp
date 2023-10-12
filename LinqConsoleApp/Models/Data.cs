using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqConsoleApp.Models
{
    public static class Data
    {
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            employees.Add(new Employee()
            {
                Id = 1,
                FirstName = "Bob",
                LastName = "Rain",
                AnnualSalary = 60000.3m,
                IsManager = true,
                DepartmentId = 1
            });
            employees.Add(new Employee()
            {
                Id = 2,
                FirstName = "Mell",
                LastName = "Handson",
                AnnualSalary = 80000.3m,
                IsManager = true,
                DepartmentId = 2
            });
            employees.Add(new Employee()
            {
                Id = 3,
                FirstName = "Natta",
                LastName = "Stue",
                AnnualSalary = 40000.3m,
                IsManager = false,
                DepartmentId = 2
            });
            employees.Add(new Employee()
            {
                Id = 4,
                FirstName = "Jane",
                LastName = "Marry",
                AnnualSalary = 30000.3m,
                IsManager = false,
                DepartmentId = 3
            });

            return employees;
        }

        public static List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            departments.Add(new Department()
            {
                Id = 1,
                ShortName = "HR",
                LongName = "Human Resources"
            });
            departments.Add(new Department()
            {
                Id = 2,
                ShortName = "FN",
                LongName = "Finance"
            });
            departments.Add(new Department()
            {
                Id = 3,
                ShortName = "TE",
                LongName = "Technology"
            });

            return departments;
        }
    }
}
