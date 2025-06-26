using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOProjectBasedLeaning
{
    public interface Company : Model
    {
        Company AddTimeTracker(TimeTracker timeTracker);
        Employee FindEmployeeById(int id);
        Company AddEmployee(Employee employee);
        Company RemoveEmployee(Employee employee);
        void ClockIn(Employee employee);
        void ClockOut(Employee employee);
        bool IsAtWork(Employee employee);
    }

    public class CompanyModel : ModelEntity, Company
    {
        private TimeTracker timeTracker = NullTimeTracker.Instance;
        private Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

        public CompanyModel() : this(string.Empty)
        {
        }

        public CompanyModel(string name)
        {
            AcquireEmployees().ForEach(employee =>
            {
                employee.AddCompany(this);
            });
        }

        public Company AddTimeTracker(TimeTracker timeTracker)
        {
            this.timeTracker = timeTracker;
            return this;
        }

        public Employee FindEmployeeById(int id)
        {
            return employees.GetValueOrDefault(id, NullEmployee.Instance);
        }

        public Company AddEmployee(Employee employee)
        {
            employees[employee.Id] = employee;  // 修正：Add → インデクサーで追加・上書き
            return this;
        }

        public Company RemoveEmployee(Employee employee)
        {
            if (employees.ContainsKey(employee.Id))
            {
                employees.Remove(employee.Id);
            }
            return this;
        }

        private static List<Employee> staticEmployeeList = new List<Employee>()
        {
            new Manager(1, "Manager1"),
            new Manager(2, "Manager2"),
            new EmployeeModel(1000, "Employee1000"),
            new EmployeeModel(2000, "Employee2000"),
            new EmployeeModel(3000, "Employee3000")
        };

        private List<Employee> AcquireEmployees()
        {
            return staticEmployeeList;
        }

        public void ClockIn(Employee employee)
        {
            timeTracker.PunchIn(FindEmployeeById(employee.Id).Id);
        }

        public void ClockOut(Employee employee)
        {
            timeTracker.PunchOut(FindEmployeeById(employee.Id).Id);
        }

        public bool IsAtWork(Employee employee)
        {
            return timeTracker.IsAtWork(FindEmployeeById(employee.Id).Id);
        }
    }

    public class NullCompany : ModelEntity, Company, NullObject
    {
        private static Company instance = new NullCompany();
        private NullCompany()
        {
        }

        public static Company Instance { get { return instance; } }

        public override string Name
        {
            get { return string.Empty; }
            set { }
        }

        public Company AddTimeTracker(TimeTracker timeTracker)
        {
            return this;
        }

        public Employee FindEmployeeById(int id)
        {
            return NullEmployee.Instance;
        }

        public Company AddEmployee(Employee employee)
        {
            return this;
        }

        public Company RemoveEmployee(Employee employee)
        {
            return this;
        }

        public void ClockIn(Employee employee)
        {
        }

        public void ClockOut(Employee employee)
        {
        }

        public bool IsAtWork(Employee employee)
        {
            return false;
        }
    }
}
