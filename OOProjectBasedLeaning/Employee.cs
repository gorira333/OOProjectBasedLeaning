using System;
using System.Collections.Generic;
using System.Linq;

namespace OOProjectBasedLeaning
{
    /*
    * Employee インターフェース
    * 従業員の基本的な属性と行動（出勤・退勤・出発・帰宅など）を定義。
    */
    public interface Employee : Model
    {
        const int NEW = 0; // 新規従業員の初期ID

        int Id { get; } // 従業員ID（読み取り専用）

        // 所属会社の登録・解除・参照
        Employee AddCompany(Company company);
        Employee RemoveCompany();
        Company In();
        // 出退勤・出発・帰宅操作
        void Depart();
        void ClockIn();
        void ClockOut();
        void ArriveHome();

        // 勤務状態の確認
        bool IsAtWork();               // 勤務中かどうか
        string GetStatusMessage();     // 状態メッセージ（出勤中・退勤済など）

        // 勤務履歴の取得
        IReadOnlyList<WorkSession> GetWorkSessions();
    }
    /*
 * 勤務セッションを表すクラス
 * 出勤時刻と退勤時刻を記録。
 */

    public class WorkSession
    {
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }

        public override string ToString()
        {
            string outTime = ClockOutTime.HasValue
                ? ClockOutTime.Value.ToString("yyyy/MM/dd HH:mm:ss")
                : "退勤なし";

            return $"出勤: {ClockInTime:yyyy/MM/dd HH:mm:ss} / 退勤: {outTime}";
        }
    }
    /*
 * 実際の従業員の実装クラス
 * ID・名前・会社・勤務履歴などの管理を行う。
 */

    public class EmployeeModel : ModelEntity, Employee
    {
        private int id;
        private Company company = NullCompany.Instance;// 初期は NullCompany（無所属）

        private DateTime? departedAt = null;
        private DateTime? arrivedHomeAt = null;

        private List<WorkSession> workSessions = new List<WorkSession>();
        private WorkSession? currentSession = null;
        // コンストラクタのオーバーロード（IDと名前の初期化）
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

        // 所属会社を登録（双方向登録）
        public Employee AddCompany(Company company)
        {
            this.company = company.AddEmployee(this);
            return this;
        }
        // 所属会社から外れる

        public Employee RemoveCompany()
        {
            company.RemoveEmployee(this);
            company = NullCompany.Instance;
            return this;
        }

        public Company In() => company;
        // 出発処理：出発時刻を記録、セッションを初期化

        public void Depart()
        {
            departedAt = DateTime.Now;
            currentSession = null;
            arrivedHomeAt = null;

            // ✅ 勤務履歴を消してしまわないように注意
            // workSessions.Clear(); ← 消さない！
        }

        // 出勤処理：勤務中でないことを確認して新セッションを開始

        public void ClockIn()
        {
            if (IsAtWork())
            {
                throw new InvalidOperationException("すでに出勤中です");
            }

            currentSession = new WorkSession
            {
                ClockInTime = DateTime.Now
            };
            workSessions.Add(currentSession);

            company.ClockIn(this);
        }
        // 退勤処理：勤務中であることを確認してセッションを終了
        public void ClockOut()
        {
            if (!IsAtWork())
            {
                throw new InvalidOperationException("出勤していません");
            }

            currentSession!.ClockOutTime = DateTime.Now;

            company.ClockOut(this);// 会社に通知

            currentSession = null; // セッション終了
        }
        // 帰宅処理：帰宅時刻を記録

        public void ArriveHome()
        {
            arrivedHomeAt = DateTime.Now;
        }
        // 勤務中であるかの判定（セッションが存在し、退勤していない）
        public bool IsAtWork()
        {
            return currentSession != null && currentSession.ClockOutTime == null;
        }
        // 現在の勤務状態を人が読めるテキストで返す
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
        // 勤務履歴の取得（読み取り専用リスト）
        public IReadOnlyList<WorkSession> GetWorkSessions() => workSessions.AsReadOnly();
    }
    /*
 * Manager クラス（管理者の派生クラス）
 * EmployeeModel を継承し、役職を表現するためのクラス。
 * 特別な処理はなく、名前とIDの初期化のみ行う。
 */
    public class Manager : EmployeeModel
    {
        public Manager() : base(Employee.NEW) { }
        public Manager(int id) : base(id, string.Empty) { }
        public Manager(string name) : base(Employee.NEW, name) { }
        public Manager(int id, string name) : base(id, name) { }
    }
    /*
 * NullEmployee クラス（無効な従業員）
 * Null Object パターンの実装で、存在しない従業員の代替として使用。
 */
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

        public IReadOnlyList<WorkSession> GetWorkSessions() =>
            new List<WorkSession>().AsReadOnly();
    }

}
