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

        /// <summary>
        /// タイムトラッカーを追加する。
        /// </summary>
        /// <param name="timeTracker">タイムトラッカー</param>
        /// <returns>会社</returns>
        Company AddTimeTracker(TimeTracker timeTracker);

        /// <summary>
        /// IDで従業員を検索する。
        /// </summary>
        /// <param name="id">従業員のID</param>
        /// <returns>従業員</returns>
        Employee FindEmployeeById(int id);

        /// <summary>
        /// Adds a new employee to the company.
        /// </summary>
        /// <param name="employee">The employee to add. Cannot be null.</param>
        /// <returns>The updated company instance containing the newly added employee.</returns>
        Company AddEmployee(Employee employee);

        /// <summary>
        /// Removes the specified employee from the company.
        /// </summary>
        /// <remarks>If the specified employee is not part of the company, no changes are made.</remarks>
        /// <param name="employee">The employee to be removed. Cannot be null.</param>
        /// <returns>The updated <see cref="Company"/> instance after the employee has been removed.</returns>
        Company RemoveEmployee(Employee employee);

        /// <summary>
        /// Records the clock-in time for the specified employee.
        /// </summary>
        /// <param name="employee">The employee whose clock-in time is being recorded. Cannot be null.</param>
        void ClockIn(Employee employee);

        /// <summary>
        /// Records the end of an employee's work shift.
        /// </summary>
        /// <remarks>This method updates the employee's work status to indicate they have clocked out. 
        /// Ensure the <paramref name="employee"/> object is valid and represents an active employee  before calling
        /// this method.</remarks>
        /// <param name="employee">The employee whose shift is being ended. Cannot be null.</param>
        void ClockOut(Employee employee);
        
        /// <summary>
        /// Determines whether the specified employee is currently at work.
        /// </summary>
        /// <param name="employee">The employee to check. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the employee is at work; otherwise, <see langword="false"/>.</returns>
        bool IsAtWork(Employee employee);

    }

    public class CompanyModel : ModelEntity, Company
    {

        /// <summary>
        /// Tracks the elapsed time for operations or processes.
        /// </summary>
        /// <remarks>This field is initialized to a default instance of <see cref="NullTimeTracker"/>,
        /// which represents a no-op implementation of the <see cref="TimeTracker"/> class. Use this field to measure
        /// time intervals or replace it with a custom implementation for more detailed tracking.</remarks>
        private TimeTracker timeTracker = NullTimeTracker.Instance;
        /// <summary>
        /// <Employee.Id, Employee>
        /// </summary>
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

            // TODO: ここで従業員をIDで検索する処理を実装する
            return employees.GetValueOrDefault(id, NullEmployee.Instance);

        }

        public Company AddEmployee(Employee employee)
        {

            employees.Add(employee.Id, employee);

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

                // Manager
                new Manager(1, "Manager1"), new Manager(2, "Manager2"),
                // Employee
                new EmployeeModel(1000, "Employee1000"), new EmployeeModel(2000, "Employee2000"), new EmployeeModel(3000, "Employee3000")

            };

        private List<Employee> AcquireEmployees()
        {

            // TODO: ここで従業員のリストを取得する処理を実装する
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

            set { /* do nothing */ }

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
