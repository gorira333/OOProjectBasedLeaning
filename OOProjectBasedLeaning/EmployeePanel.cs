using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{

    public class EmployeePanel : Panel
    {
        public int EmployeeId => employee.Id;

        private Employee employee;

        public EmployeePanel(Employee employee)
        {

            this.employee = employee;

            InitializeComponent();

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
        public void SetupWorkButtons()
        {
            var clockInButton = new Button
            {
                Text = "出勤",
                Location = new Point(20, 40)
            };

            var clockOutButton = new Button
            {
                Text = "退勤",
                Location = new Point(100, 40)
            };

            var logLabel = new Label
            {
                Location = new Point(20, 70),
                AutoSize = true
            };

            clockInButton.Click += (sender, e) =>
            {
                employee.ClockIn();
                logLabel.Text = $"{employee.Name} 出勤: {DateTime.Now:HH:mm:ss}";
            };

            clockOutButton.Click += (sender, e) =>
            {
                employee.ClockOut();
                logLabel.Text = $"{employee.Name} 退勤: {DateTime.Now:HH:mm:ss}";
            };

            Controls.Add(clockInButton);
            Controls.Add(clockOutButton);
            Controls.Add(logLabel);
        }


    }

}
