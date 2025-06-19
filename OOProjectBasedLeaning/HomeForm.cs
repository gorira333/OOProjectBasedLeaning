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

            Controls.Add(employeeListPanel); // ÅŒã‚É’Ç‰Á‚µ‚Ä‰B‚ê‚È‚¢‚æ‚¤‚É
        }

        public void AddEmployee(Employee employee)
        {
            var panel = new Panel
            {
                Size = new Size(300, 60),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = $"“o˜^Ï: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true
            };

            var departButton = new Button
            {
                Text = "o”­",
                Tag = employee,
                Location = new Point(200, 10),
                Size = new Size(75, 25)
            };

            departButton.Click += (sender, e) =>
            {
                var companyForm = Application.OpenForms
                    .OfType<CompanyForm>()
                    .FirstOrDefault();

                if (companyForm != null)
                {
                    companyForm.AddEmployeePanel(employee);
                }
            };

            panel.Controls.Add(label);
            panel.Controls.Add(departButton);
            employeeListPanel.Controls.Add(panel);
        }
    }
}
