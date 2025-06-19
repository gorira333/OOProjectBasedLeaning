using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public interface TimeTracker
    {
        void PunchIn(int employeeId);
        void PunchOut(int employeeId);
        bool IsAtWork(int employeeId);
    }

    public class TimeTrackerModel : TimeTracker
    {
        private Company company = NullCompany.Instance;

        private Dictionary<DateTime, Dictionary<int, DateTime>> timestamp4PunchIn =
            new Dictionary<DateTime, Dictionary<int, DateTime>>();

        private Dictionary<DateTime, Dictionary<int, DateTime>> timestamp4PunchOut =
            new Dictionary<DateTime, Dictionary<int, DateTime>>();

        private Mode mode = Mode.PunchIn;

        private enum Mode
        {
            PunchIn,
            PunchOut
        };

        public TimeTrackerModel(Company company)
        {
            this.company = company.AddTimeTracker(this);
        }

        public void PunchIn(int employeeId)
        {
            if (IsAtWork(employeeId))
            {
                throw new InvalidOperationException("Employee is already at work.");
            }

            var today = DateTime.Today;

            if (!timestamp4PunchIn.ContainsKey(today))
            {
                timestamp4PunchIn[today] = new Dictionary<int, DateTime>();
            }

            timestamp4PunchIn[today][employeeId] = DateTime.Now;
        }

        public void PunchOut(int employeeId)
        {
            if (!IsAtWork(employeeId))
            {
                throw new InvalidOperationException("Employee is not at work.");
            }

            var today = DateTime.Today;

            if (!timestamp4PunchOut.ContainsKey(today))
            {
                timestamp4PunchOut[today] = new Dictionary<int, DateTime>();
            }

            timestamp4PunchOut[today][employeeId] = DateTime.Now;
        }

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

    public class NullTimeTracker : TimeTracker, NullObject
    {
        private static NullTimeTracker instance = new NullTimeTracker();

        private NullTimeTracker() { }

        public static TimeTracker Instance => instance;

        public void PunchIn(int employeeId)
        {
            // 何もしない
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
