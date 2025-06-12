using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOProjectBasedLeaning
{

    public interface Employee : Model
    {

        const int NEW = 0;

        int Id { get; }

        Employee AddCompany(Company company);

        Employee RemoveCompany();

        Company In();

        void ClockIn();

        void ClockOut();

        bool IsAtWork();

    }

    public class EmployeeModel : ModelEntity, Employee
    {

        private int id;

        private Company company = NullCompany.Instance;

        public EmployeeModel() : this(Employee.NEW)
        {

        }

        public EmployeeModel(int id) : this(id, string.Empty)
        {

        }

        public EmployeeModel(string name) : this(Employee.NEW, name)
        {

        }

        public EmployeeModel(int id, string name)
        {

            this.id = id;
            Name = name;

        }

        public override int GetHashCode()
        {

            return Id;

        }

        public override bool Equals(object? obj)
        {

            if (obj is Employee)
            {

                return Id == (obj as Employee)?.Id;

            }

            return false;

        }

        public int Id { get { return id; } }

        public Employee AddCompany(Company company)
        {

            this.company = company.AddEmployee(this);

            return this;

        }

        public Employee RemoveCompany()
        {

            company.RemoveEmployee(this);

            company = NullCompany.Instance;

            return this;

        }

        public Company In()
        {

            return company;

        }

        public void ClockIn()
        {

            company.ClockIn(this);

        }

        public void ClockOut()
        {

            company.ClockOut(this);

        }

        public bool IsAtWork()
        {

            return company.IsAtWork(this);

        }

    }

    public class Manager : EmployeeModel
    {

        public Manager() : base(Employee.NEW)
        {

        }

        public Manager(int id) : base(id, string.Empty)
        {

        }

        public Manager(string name) : base(Employee.NEW, name)
        {

        }

        public Manager(int id, string name) : base(id, name)
        {

        }

        // TODO: Implement manager specific methods if needed

    }

    public class NullEmployee : ModelEntity, Employee, NullObject
    {

        private static Employee instance = new NullEmployee();

        private NullEmployee()
        {

        }

        public static Employee Instance { get { return instance; } }

        public int Id { get { return Employee.NEW; } }

        public override string Name
        {

            get { return string.Empty; }

            set { /* do nothing */ }

        }

        public Employee AddCompany(Company company)
        {

            return this;

        }

        public Employee RemoveCompany()
        {

            return this;

        }

        public Company In()
        {

            return NullCompany.Instance;

        }

        public void ClockIn()
        {

        }

        public void ClockOut()
        {

        }

        public bool IsAtWork()
        {

            return false;

        }

    }

}
