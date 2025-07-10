using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOProjectBasedLeaning
{
    /*
     * Company（会社）インターフェース
     * 従業員の登録・削除・検索や、打刻（出勤・退勤）の処理を規定する。
     * 実装は CompanyModel または NullCompany で行う。
     */
    public interface Company : Model
    {
        Company AddTimeTracker(TimeTracker timeTracker); // 出退勤を管理するタイムトラッカーを会社に登録
        Employee FindEmployeeById(int id);               // 従業員IDで従業員を検索
        Company AddEmployee(Employee employee);          // 従業員を追加
        Company RemoveEmployee(Employee employee);       // 従業員を削除
        void ClockIn(Employee employee);                 // 出勤打刻
        void ClockOut(Employee employee);                // 退勤打刻
        bool IsAtWork(Employee employee);                // 勤務中かどうかを判定
    }

    /*
     * CompanyModel（実装クラス）
     * 実際の従業員と出退勤の管理を行う。
     * 社員のリストを保持し、TimeTracker へ打刻処理を委譲する。
     */
    public class CompanyModel : ModelEntity, Company
    {
        // 出退勤管理用のタイムトラッカー（デフォルトはNullオブジェクト）
        private TimeTracker timeTracker = NullTimeTracker.Instance;

        // 従業員リスト（IDをキーに従業員を管理）
        private Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

        // 名前未指定のコンストラクタ（空の名前を指定）
        public CompanyModel() : this(string.Empty) { }

        // 名前指定コンストラクタ
        public CompanyModel(string name)
        {
            // 社員リストから取得し、全員をこの会社に所属させる
            AcquireEmployees().ForEach(employee =>
            {
                employee.AddCompany(this);
            });
        }

        // タイムトラッカーを登録
        public Company AddTimeTracker(TimeTracker timeTracker)
        {
            this.timeTracker = timeTracker;
            return this;
        }

        // IDから従業員を検索（見つからない場合は NullEmployee を返す）
        public Employee FindEmployeeById(int id)
        {
            return employees.GetValueOrDefault(id, NullEmployee.Instance);
        }

        // 従業員を追加（既に存在していれば上書き）
        public Company AddEmployee(Employee employee)
        {
            employees[employee.Id] = employee; // Add → インデクサーで上書き対応
            return this;
        }

        // 従業員を削除
        public Company RemoveEmployee(Employee employee)
        {
            if (employees.ContainsKey(employee.Id))
            {
                employees.Remove(employee.Id);
            }
            return this;
        }

        // 社員の初期データ（静的に定義された社員リスト）
        private static List<Employee> staticEmployeeList = new List<Employee>()
        {
            new Manager(1, "Manager1"),
            new Manager(2, "Manager2"),
            new EmployeeModel(1000, "Employee1000"),
            new EmployeeModel(2000, "Employee2000"),
            new EmployeeModel(3000, "Employee3000")
        };

        // 初期社員リストを取得（この会社に所属させるために使用）
        private List<Employee> AcquireEmployees()
        {
            return staticEmployeeList;
        }

        // 指定した従業員の出勤打刻（TimeTracker に委譲）
        public void ClockIn(Employee employee)
        {
            timeTracker.PunchIn(FindEmployeeById(employee.Id).Id);
        }

        // 指定した従業員の退勤打刻（TimeTracker に委譲）
        public void ClockOut(Employee employee)
        {
            timeTracker.PunchOut(FindEmployeeById(employee.Id).Id);
        }

        // 勤務中かどうかの判定（TimeTracker に問い合わせ）
        public bool IsAtWork(Employee employee)
        {
            return timeTracker.IsAtWork(FindEmployeeById(employee.Id).Id);
        }
    }

    /*
     * NullCompany（ダミー会社の実装）
     * 実体のない会社として動作。Null Object パターン。
     */
    public class NullCompany : ModelEntity, Company, NullObject
    {
        private static Company instance = new NullCompany();

        private NullCompany() { }

        // シングルトンインスタンスの取得
        public static Company Instance => instance;

        // 名前プロパティ（空文字固定）
        public override string Name
        {
            get { return string.Empty; }
            set { } // 無視
        }

        // すべての処理は無効な操作として無視する

        public Company AddTimeTracker(TimeTracker timeTracker) => this;
        public Employee FindEmployeeById(int id) => NullEmployee.Instance;
        public Company AddEmployee(Employee employee) => this;
        public Company RemoveEmployee(Employee employee) => this;
        public void ClockIn(Employee employee) { }
        public void ClockOut(Employee employee) { }
        public bool IsAtWork(Employee employee) => false;
    }
}
