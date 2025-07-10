using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    /*
 * 従業員の出退勤打刻を管理するインターフェース
 * Company 経由で各従業員に紐づけて使用される
 */
    public interface TimeTracker
    {
        void PunchIn(int employeeId);   // 出勤打刻
        void PunchOut(int employeeId);  // 退勤打刻
        bool IsAtWork(int employeeId);  // 勤務中かどうか
    }
    /*
 * 出退勤の打刻時刻を日付ごとに記録するモデルクラス
 */

    public class TimeTrackerModel : TimeTracker
    {
        private Company company = NullCompany.Instance;

        // 出勤時刻：日付 → 従業員ID → 出勤時刻
        private Dictionary<DateTime, Dictionary<int, DateTime>> timestamp4PunchIn =
            new Dictionary<DateTime, Dictionary<int, DateTime>>();

        // 退勤時刻：日付 → 従業員ID → 退勤時刻
        private Dictionary<DateTime, Dictionary<int, DateTime>> timestamp4PunchOut =
            new Dictionary<DateTime, Dictionary<int, DateTime>>();
        // 打刻モード（今後の拡張用）
        private Mode mode = Mode.PunchIn;

        private enum Mode
        {
            PunchIn,
            PunchOut
        };
        // コンストラクタ：Company に自身を登録する
        public TimeTrackerModel(Company company)
        {
            this.company = company.AddTimeTracker(this);
        }
        // 出勤打刻の処理
        public void PunchIn(int employeeId)
        {
            if (IsAtWork(employeeId))
            {
                throw new InvalidOperationException("Employee is already at work.");
            }

            var today = DateTime.Today;
            // 今日の打刻情報がなければ初期化
            if (!timestamp4PunchIn.ContainsKey(today))
            {
                timestamp4PunchIn[today] = new Dictionary<int, DateTime>();
            }

            timestamp4PunchIn[today][employeeId] = DateTime.Now;
        }
        // 退勤打刻の処理
        public void PunchOut(int employeeId)
        {
            var today = DateTime.Today;

            if (!timestamp4PunchOut.ContainsKey(today))
            {
                timestamp4PunchOut[today] = new Dictionary<int, DateTime>();
            }

            timestamp4PunchOut[today][employeeId] = DateTime.Now;
        }
        // 現在勤務中かどうかを判定するロジック
        public bool IsAtWork(int employeeId)
        {
            var today = DateTime.Today;

            var hasPunchIn = timestamp4PunchIn.ContainsKey(today)
                && timestamp4PunchIn[today].ContainsKey(employeeId);

            var hasPunchOut = timestamp4PunchOut.ContainsKey(today)
                && timestamp4PunchOut[today].ContainsKey(employeeId);

            return hasPunchIn && !hasPunchOut;
        }
    }

    /*
     * Null Object パターン：打刻が不要な場合のダミー実装
     */

    public class NullTimeTracker : TimeTracker, NullObject
    {
        private static NullTimeTracker instance = new NullTimeTracker();

        private NullTimeTracker() { }

        public static TimeTracker Instance => instance;

        public void PunchIn(int employeeId)
        {
            // 何もしない1
        }

        public void PunchOut(int employeeId)
        {
            // 何もしない
        }

        public bool IsAtWork(int employeeId)
        {
            return false;
        }
    }
}
