using System;
using System.Collections.Generic;
using System.Linq;

namespace OOProjectBasedLeaning
{
    /*
    1人の従業員の状態を管理するインターフェース
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

        IReadOnlyList<WorkSession> GetWorkSessions();
        string GetClockInTimeString();
        string GetClockOutTimeString();
    }

    /*
    出勤・退勤時間の1回分の勤務セッションを表すクラス
    */
    public class WorkSession
    {
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }

        public override string ToString()
        {
            var outTime = ClockOutTime.HasValue ? ClockOutTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "退勤なし";
            return $"出勤: {ClockInTime:yyyy/MM/dd HH:mm:ss} / 退勤: {outTime}";
        }
    }

    /*
    Employeeインターフェースの実装クラス
    */
    public class EmployeeModel : ModelEntity, Employee
    {
        private int id;
        private Company company = NullCompany.Instance;

        private DateTime? departedAt = null;
        private DateTime? arrivedHomeAt = null;

        private List<WorkSession> workSessions = new List<WorkSession>();
        private WorkSession? currentSession = null;

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
            currentSession = null;
            arrivedHomeAt = null;
            workSessions.Clear();
        }

        public void ClockIn()
        {
            if (IsAtWork())
            {
                throw new InvalidOperationException("既に出勤中です");
            }

            currentSession = new WorkSession
            {
                ClockInTime = DateTime.Now
            };
            workSessions.Add(currentSession);

            company.ClockIn(this);
        }

        public void ClockOut()
        {
            if (!IsAtWork())
            {
                throw new InvalidOperationException("出勤していません");
            }

            currentSession!.ClockOutTime = DateTime.Now;

            company.ClockOut(this);

            currentSession = null;  // 退勤したらセッション終了（nullクリア）
        }

        public void ArriveHome()
        {
            arrivedHomeAt = DateTime.Now;
        }

        public bool IsAtWork()
        {
            return currentSession != null && currentSession.ClockOutTime == null;
        }

        public string GetStatusMessage()
        {
            if (arrivedHomeAt.HasValue)
                return "帰宅済み、未出発";
            if (currentSession == null && workSessions.Count > 0 && workSessions[^1].ClockOutTime.HasValue)
                return "退勤済み、未帰宅";
            if (IsAtWork())
                return "出勤済み、未退勤";
            if (departedAt.HasValue)
                return "出発済み、未出勤";

            return "未出発";
        }

        public IReadOnlyList<WorkSession> GetWorkSessions() => workSessions.AsReadOnly();

        public string GetClockInTimeString()
        {
            var lastSession = workSessions.LastOrDefault();
            if (lastSession != null)
            {
                return lastSession.ClockInTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
            return "未出勤";
        }

        public string GetClockOutTimeString()
        {
            var lastSession = workSessions.LastOrDefault();
            if (lastSession != null && lastSession.ClockOutTime.HasValue)
            {
                return lastSession.ClockOutTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
            }
            return "未退勤";
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

        public IReadOnlyList<WorkSession> GetWorkSessions() => new List<WorkSession>().AsReadOnly();
        public string GetClockInTimeString() => "未出勤";
        public string GetClockOutTimeString() => "未退勤";
    }
}
