using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    public class EmployeePanel : Panel
    {
        public int EmployeeId => employee.Id;
        private Employee employee;

        public EmployeePanel(Employee employee)
        {
            this.employee = employee;

            this.Size = new Size(320, 140); // 高さを十分に確保
            this.BorderStyle = BorderStyle.FixedSingle;

            InitializeComponent();
            SetupStatusButton(); // ← 状態確認ボタンをここで追加
        }

        private void InitializeComponent()
        {
            Label employeeNameLabel = new Label
            {
                Text = employee.Name,
                AutoSize = true,
                Location = new Point(20, 10)
            };

            TextBox guestNameTextBox = new TextBox
            {
                Text = employee.Name,
                Location = new Point(140, 6),
                Width = 160
            };

            Controls.Add(employeeNameLabel);
            Controls.Add(guestNameTextBox);
        }

        public void SetupStatusButton()
        {
            var statusButton = new Button
            {
                Text = "状態確認",
                Location = new Point(20, 50),
                Size = new Size(80, 25)
            };

            statusButton.Click += (sender, e) =>
            {
                string message = employee.IsAtWork()
                    ? $"{employee.Name} は出勤中です"
                    : $"{employee.Name} は退勤済みまたは未出勤です";

                MessageBox.Show(message, "勤務状態");
            };

            Controls.Add(statusButton);
        }
    }
}
