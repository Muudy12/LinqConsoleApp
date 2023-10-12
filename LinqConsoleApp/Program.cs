
using LinqConsoleApp.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace LinqConsoleApp
{
    public class Program : IProgram
    {
        private readonly ILogger _log;

        public Program(ILogger<Program> log)
        {
            _log = log;
        }
        public void Run()
        {
            //JoinTwoList();

            //All and Any returns bool; used for comparing
            //Contains also used for comparing objects which uses comparable class implementing IEqualityComparer<Type of object or class>
            //AllAnyContain();

            //Returns the first element of a colletion that meets the condition while Default will not throw an exception when conditions don't meet and using if statement to print a message or do something else isntead
            //in a list of int, Default is 0
            //FirstLastOperators();

            //Single returns the one element only when there exists only a single element that meets the condition and Default will not throw an exception similar to FirstOrDefault
            //in a list of objects, Default is null
            //SingleOperator();

            //Distinct returns values without duplicates existed
            //DistinctOperator();

            //Except returns values that exists in the first list but don't exist in the second list
            //Intercept returns values that only exist in both lists
            //ExceptIntersect();

            //Returns the property type of the selected from a collection
            //SelectOperator();

            //ObtainDataSource();

            ConditionalOperator();
        }
        public void JoinTwoList()
        {
            List<Employee> employeeList = Data.GetEmployees();
            List<Department> departmentList = Data.GetDepartments();

            //Joining two list into one result List using join clause
            ///Using inner join to join data from employee table and data from department table which uses DepartmentId to join them together;
            //////"SELECT e.FirstName, e.LastName, e.AnnualSalary, e.IsManager, d.LongName 
            ///         FROM Employee e 
            ///         INNER JOIN Department d on e.DepartmentId = d.Id
            //resultList becomes a new IEnumerable list with the data from the SELECT query
            var resultList = from emp in employeeList
                             join dept in departmentList
                             on emp.DepartmentId equals dept.Id
                             select new
                             {
                                 FirstName = emp.FirstName,
                                 LastName = emp.LastName,
                                 AnnualSalary = emp.AnnualSalary,
                                 Manager = emp.IsManager,
                                 Department = dept.LongName
                             };

            //foreach (var employee in resultList)
            //{
            //    _log.LogInformation($"First Name: {employee.FirstName}");
            //    _log.LogInformation($"Last Name: {employee.LastName}");
            //    _log.LogInformation($"Annual Salary: {employee.AnnualSalary}");
            //    _log.LogInformation($"Manager: {employee.Manager}");
            //    _log.LogInformation($"Department: {employee.Department}");
            //    _log.LogInformation("");
            //}

            var groupResult = from emp in employeeList
                              orderby emp.DepartmentId
                              group emp by emp.DepartmentId;

            foreach (var empGroup in groupResult)
            {
                _log.LogInformation($"Department Id: {empGroup.Key}");
                foreach (Employee emp in empGroup)
                {
                    _log.LogInformation($" {emp.FirstName} {emp.LastName}");
                }
                _log.LogInformation("");

            }
        }

        public void AllAnyContain()
        {
            List<Employee> employeeList = Data.GetEmployees();
            //List<Department> departmentList = Data.GetDepartments();

            var annualSalaryCompare = 40000;

            ////ALL Operator
            bool isTrueAll = employeeList.All(x => x.AnnualSalary > annualSalaryCompare);
            if (isTrueAll)
            {
                _log.LogInformation($"ALL employee annual salaries are above {annualSalaryCompare}");
            }
            else
            {
                _log.LogInformation($"Not All employee annual salaries are above {annualSalaryCompare}");
            }

            ////ANY Operator
            var isTrueAny = employeeList.Any(x => x.AnnualSalary > annualSalaryCompare);
            if (isTrueAny)
            {
                _log.LogInformation($"At Least one employee's annual salary is above {annualSalaryCompare}");
            }
            else
            {
                _log.LogInformation($"NO employee annual salaries are above {annualSalaryCompare}");
            }


            ////CONTAINS Operator
            var searchEmployee = new Employee()
            {
                Id = 3,
                FirstName = "Natta",
                LastName = "Stue",
                AnnualSalary = 40000.3m,
                IsManager = false,

            };

            bool containsEmployee = employeeList.Contains(searchEmployee, new EmployeeComparer()); ///After adding the comparer Class; 
                                                                                                   ///Now we've told the compiler how we want to compare the serachEmployee object 
                                                                                                   ///with the objects stored within the EmployeeList collection
            if (containsEmployee)
            {
                _log.LogInformation($"An employee record for {searchEmployee.FirstName} {searchEmployee.LastName} was found.");
            }
            else
            {
                _log.LogInformation($"An employee record for {searchEmployee.FirstName} {searchEmployee.LastName} was NOT found.");
            }
        }

        //Need a comparer class in order to compare the objects to list of objects using the class below implementing IEqualityComparer
        public class EmployeeComparer : IEqualityComparer<Employee> //comparing Employee class
        {
            public bool Equals(Employee? x, Employee? y) //used to establish how we want the objects to be compared; are of equal values
            {
                //If both passed in Employee objecs have the same id, Names cast in lower case, then return true, else return false as default
                if (x.Id == y.Id && x.FirstName.ToLower() == y.FirstName.ToLower() && x.LastName.ToLower() == y.LastName.ToLower())
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode([DisallowNull] Employee obj) //used for the purpose of uniquely identifying an object
            {
                //implement the GetHashCode method by returning the hashcode of the id property of the relevant employee object
                return obj.Id.GetHashCode();
            }
        }

        public void FirstLastOperators()
        {
            List<int> integerList = new() { 3, 14, 17, 2, 9, 5, 25};

            int result = integerList.First(); //returns 3, the first element in the collection
            _log.LogInformation($"First(): {result}");

            int result2 = integerList.First(x => x % 2 == 0); //The added condition returns the first EVEN number of the collection
            _log.LogInformation($"First(with even number condition): {result2}"); //returns number 14 which is the first even number of the collection

            List<int> integerList2 = new() { 3, 13, 17, 5, 9, 7, 25 };

            int result3 = integerList2.First(x => x % 2 == 0); //if no even number exist in the collection, Invalid operation exception has been thrown
            _log.LogInformation($"First(with even number condition) but no even number exists: {result3}"); 
            //but if we don't want to put in try catch exceptions, can use FirstOrDefault operator which will return the default value 0

            int result4 = integerList2.FirstOrDefault(x => x % 2 == 0);
            //default should return 0 but if it doesn't;
            if (result4 != 0)
            {
                _log.LogInformation($"First(with even number condition) but no even number exists: {result4}");
            }
            else
            {
                _log.LogInformation("There are no even numbers in the collection."); //should have returned this message
            }

            //Last() and LastOrDefault() does the same as the First() and FirstOrDefault() operator just the difference in return first or last of the collection
        }

        public void SingleOperator()
        {
            //Single operator returns the only element in the collection or one element that satisfies the specified condition from a collection
            //Should only return one single element; if more than one element in the collection satisfies the condition, Invalid Operator exception will be thrown

            List<Employee> employeeList = Data.GetEmployees();

            //var employee = employeeList.Single();
            //_log.LogInformation($"Only one or Single Element/Employee existing in a list: {employee.FirstName} {employee.LastName}"); //if one and only one exist in a list, returns the Single(no condition passed in);

            //var specifiedEmployee = employeeList.Single(x => x.Id == 0);
            //_log.LogInformation($"Only one employee has an Id of 2: {specifiedEmployee.FirstName} {specifiedEmployee.LastName}"); //only one element of the colletion satisfies the passed in condition therefore only returns one item

            //using the SingleOrDefault;
            //when more than one element satisfies the condition, Default will be null therefore use if statements to print out message
            var specifiedEmployee = employeeList.SingleOrDefault(x => x.Id == 0);
            if (specifiedEmployee != null)
            {
                _log.LogInformation($"Only one employee has an Id of 0: {specifiedEmployee.FirstName} {specifiedEmployee.LastName}"); //one and only one return
            }
            else
            {
                _log.LogInformation("This employee does not exist."); //if 0 or many satisfies the condition
            }

        }

        public void DistinctOperator()
        {
            List<int> numberList = new() {1,1,2,5,3,8,5 };

            var distinctNumbers = numberList.Distinct();
            foreach (var number in distinctNumbers)
            {
                _log.LogInformation($"{number}");
            }
        }

        public void ExceptIntersect()
        {
            List<int> list1 = new() {1,2,3,4,5,6 };
            List<int> list2 = new() { 1, 3, 6, 4, 8, 9 };

            var intersectingNumbers = list1.Intersect(list2).ToList(); //without .ToList() at the end, it the var will become IEnumerable<int> instead of List<int>
            foreach (var number in intersectingNumbers)
            {
                _log.LogInformation($"list1 numbers that exist in list2: {number}");
            }

            var exceptNumbers = list1.Except(list2).ToList();
            foreach (var number in exceptNumbers)
            {
                _log.LogInformation($"list1 numbers that don't exist in list2: {number}");
            }
        }

        public void SelectOperator()
        {
            List<Employee> employeeList = Data.GetEmployees();
            List<Department> departmentList = Data.GetDepartments();

            var unusedDepartment = departmentList.Select(x => x.Id).Except(employeeList.Select(y => y.DepartmentId)).ToList();
            foreach (var dep in unusedDepartment)
            {
                _log.LogInformation($"Department {dep} is unused.");
            }

        }
        
        public void ObtainDataSource()
        {
            List<Employee> employeeList = Data.GetEmployees();

            //var queryAllEmployees = from emp in employeeList
            //                        select emp;

            //foreach (var employee in queryAllEmployees)
            //{
            //    _log.LogInformation($"Employee: {employee.FirstName} {employee.LastName} Makes {employee.AnnualSalary} per year");
            //}

            //var salaryOver40k = employeeList.Where(x => x.AnnualSalary > 40000).ToList(); //same result as the salaryOver40kk
            //foreach (var employee in salaryOver40k)
            //{
            //    _log.LogInformation($"Employee Salary: {employee.FirstName} {employee.LastName} Makes {employee.AnnualSalary} per year");
            //}

            //var salaryOver40kk = from emp in employeeList
            //                     where emp.AnnualSalary > 40000
            //                     select emp;
            //foreach (var employee in salaryOver40kk) //same result as the salaryOver40k
            //{
            //    _log.LogInformation($"Employee Salary Query: {employee.FirstName} {employee.LastName} Makes {employee.AnnualSalary} per year");
            //}

            //var over40kEmployees = employeeList.Where(x => x.AnnualSalary > 40000).Select(emp => emp.FirstName + " " + emp.LastName).ToList();
            //foreach (string emp in over40kEmployees)
            //{
            //    _log.LogInformation($"Emp: {emp}"); //same result as over40kEmployeesQuery but less lines
            //}

            //var over40kEmployeesQuery = from emp in employeeList
            //                            where emp.AnnualSalary > 40000
            //                            select emp.FirstName + " " + emp.LastName;
            //foreach (string emp in over40kEmployeesQuery)
            //{
            //    _log.LogInformation($"Emp Query: {emp}");
            //}

            //var over40kManagers = employeeList.Where(x => x.AnnualSalary > 40000 && x.IsManager).Select(emp => emp.FirstName + " " + emp.LastName).ToList();
            //foreach (string emp in over40kManagers)
            //{
            //    _log.LogInformation($"Manager: {emp}");
            //}

            //var over40kManagerQuery = from emp in employeeList
            //                          where emp.AnnualSalary > 40000 && emp.IsManager == true
            //                          select emp.FirstName + " " + emp.LastName;
            //foreach (string emp in over40kManagerQuery)
            //{
            //    _log.LogInformation($"Manager Query: {emp}");
            //}

            var employeeBob = employeeList.Where(x => x.FirstName == "Bob"); //this way is shorter to write/code than the query
            foreach (var emp in employeeBob)
            {
                _log.LogInformation($"Employee Bob: {emp.FirstName} {emp.LastName}");
            }

            var employeeBobQuery = from emp in employeeList
                                   where emp.FirstName == "Bob"
                                   select emp;
            foreach (var emp in employeeBobQuery)
            {
                _log.LogInformation($"Employee Bob: {emp.FirstName} {emp.LastName}");
                _log.LogInformation(String.Format("Employee Bob in String.Format: {0} {1}", emp.FirstName, emp.LastName));
            }
        }

        public void ConditionalOperator()
        {
            List<Employee> employeeList = Data.GetEmployees();

            //string GetWeatherDisplay(double tempInCelsius) => tempInCelsius < 20.0 ? "Cold" : "Perfect!";
            //_log.LogInformation(String.Format("GetWeatherDisplay: {0}", GetWeatherDisplay(15)));

            //string GetEmployee(string firstName) => employeeList.Any(x => x.FirstName.Contains(firstName)) ? "Employee Found" : "Employee Not Found";
            //_log.LogInformation(String.Format("This {0}", GetEmployee("David")));

            ////
            ///Create NewList with conditions injected, => such as
            ///if any of the DepartmentId in employeeList collection equals the condition input, returns true
            ///when return is true do the following by populating the NewList with the elements in the collection that meets the condition
            ///otherwise return is false, make the NewList Null
            
            List<Employee>? NewList(int departmentId) => 
                employeeList.Any(x => x.DepartmentId == departmentId)
                ? employeeList.Where(x => x.DepartmentId == departmentId).ToList() 
                : null;

            if (NewList(2) != null)
            {
                foreach (var emp in NewList(2))
                {
                    _log.LogInformation(String.Format("Employee {0} {1} is in this NewList.\n", emp.FirstName, emp.LastName));

                }
            }
            else
            {
                _log.LogInformation("Department Id not found.");
            }
            
        }
    }
}

