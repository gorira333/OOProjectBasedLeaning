using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    internal class employee2
    {
        public class Employee
        {
            private string name;
            public string Name
            {
                get => name;
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnNameChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public event EventHandler OnNameChanged;

            public string GetStatusMessage()
            {
                return "勤務中"; // 仮実装
            }
        }
    }
}
