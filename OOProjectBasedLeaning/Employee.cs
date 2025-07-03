using System;

namespace OOProjectBasedLeaning
{
    /*
    1人の従業員の状態を管理するクラス
    */
    public interface Employee : Model
    {
        const int NEW = 0;

        int Id { get; }
        Employee AddCompany(Company company);
        Employee RemoveCompany();
        Company In();

        void Depart();
        void ClockIn();
        void ClockOut();
        void ArriveHome();

        bool IsAtWork();
        string GetStatusMessage();
    }

    public class EmployeeModel : ModelEntity, Employee
    {
        private int id;
        private Company company = NullCompany.Instance;

        private DateTime? departedAt = null;
        private DateTime? clockInAt = null;
        private DateTime? clockOutAt = null;
        private DateTime? arrivedHomeAt = null;

        public EmployeeModel() : this(Employee.NEW) { }
        public EmployeeModel(int id) : this(id, string.Empty) { }
        public EmployeeModel(string name) : this(Employee.NEW, name) { }
        public EmployeeModel(int id, string name)
        {
            this.id = id;
            Name = name;
        }

        public int Id => id;

        public override int GetHashCode() => Id;
        public override bool Equals(object? obj) => obj is Employee other && Id == other.Id;

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

        public Company In() => company;

        public void Depart()
        {
            departedAt = DateTime.Now;
            clockInAt = null;
            clockOutAt = null;
            arrivedHomeAt = null;
        }

        public void ClockIn()
        {
            clockInAt = DateTime.Now;
            company.ClockIn(this);
        }

        public void ClockOut()
        {
            if (!IsAtWork())
            {
                throw new InvalidOperationException("Employee is not at work");
            }
            clockOutAt = DateTime.Now;
            company.ClockOut(this);
        }

        public void ArriveHome()
        {
            arrivedHomeAt = DateTime.Now;
        }

        public bool IsAtWork() => company.IsAtWork(this);

        public string GetStatusMessage()
        {
            if (arrivedHomeAt.HasValue)
                return "帰宅済み、未出発";
            if (clockOutAt.HasValue)
                return "退勤済み、未帰宅";
            if (clockInAt.HasValue)
                return "出勤済み、未退勤";
            if (departedAt.HasValue)
                return "出発済み、未出勤";

            return "未出発";
        }
    }

    public class Manager : EmployeeModel
    {
        public Manager() : base(Employee.NEW) { }
        public Manager(int id) : base(id, string.Empty) { }
        public Manager(string name) : base(Employee.NEW, name) { }
        public Manager(int id, string name) : base(id, name) { }
    }

    public class NullEmployee : ModelEntity, Employee, NullObject
    {
        private static Employee instance = new NullEmployee();
        public static Employee Instance => instance;

        private NullEmployee() { }

        public int Id => Employee.NEW;
        public override string Name { get => string.Empty; set { } }

        public Employee AddCompany(Company company) => this;
        public Employee RemoveCompany() => this;
        public Company In() => NullCompany.Instance;

        public void Depart() { }
        public void ClockIn() { }
        public void ClockOut() { }
        public void ArriveHome() { }
        public bool IsAtWork() => false;
        public string GetStatusMessage() => "未登録";
    }

}
