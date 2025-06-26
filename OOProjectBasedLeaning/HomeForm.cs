using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    public partial class HomeForm : Form
    {
        private FlowLayoutPanel employeeListPanel;

        public HomeForm()
        {
            InitializeComponent();

            employeeListPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            Controls.Add(employeeListPanel);
        }

        public void AddEmployee(Employee employee)
        {
            var panel = new Panel
            {
                Size = new Size(380, 60),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = $"�o�^��: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true
            };

            var departButton = new Button
            {
                Text = "�o��",
                Tag = employee,
                Location = new Point(200, 10),
                Size = new Size(75, 25)
            };

            var homeButton = new Button
            {
                Text = "�A��",
                Tag = employee,
                Location = new Point(285, 10),
                Size = new Size(75, 25)
            };

            departButton.Click += (sender, e) =>
            {
                employee.Depart(); // �o���L�^

                var companyForm = Application.OpenForms
                    .OfType<CompanyForm>()
                    .FirstOrDefault();

                if (companyForm != null)
                {
                    companyForm.AddEmployeePanel(employee);
                }
            };

            homeButton.Click += (sender, e) =>
            {
                try
                {
                    if (employee.IsAtWork())
                    {
                        employee.ClockOut();
                    }
                    employee.ArriveHome();
                    MessageBox.Show($"{employee.Name} �͋A��܂���", "�A���");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�A����Ɏ��s���܂���: {ex.Message}", "�G���[");
                }
            };


            panel.Controls.Add(label);
            panel.Controls.Add(departButton);
            panel.Controls.Add(homeButton);
            employeeListPanel.Controls.Add(panel);
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }
    }
}