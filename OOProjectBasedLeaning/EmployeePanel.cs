using System;
using System.Drawing;
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

            this.Size = new Size(320, 140);
            this.BorderStyle = BorderStyle.FixedSingle;

            InitializeComponent();
            SetupStatusButton();
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
                Size = new Size(110, 40)
            };

            statusButton.Click += (sender, e) =>
            {
                string message = employee.GetStatusMessage();
                MessageBox.Show($"{employee.Name} の状態：{message}", "勤務状態");
            };

            Controls.Add(statusButton);
        }
    }
}