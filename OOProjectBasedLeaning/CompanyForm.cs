using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OOProjectBasedLeaning;

namespace OOProjectBasedLeaning
{
    public partial class CompanyForm : Form
    {
        private Company company = NullCompany.Instance;
        private FlowLayoutPanel workPanelArea;

        public CompanyForm()
        {
            InitializeComponent();

            // 会社とタイムトラッカーの初期化
            company = new CompanyModel("MyCompany");
            new TimeTrackerModel(company);

            // 打刻パネルエリアの設定
            workPanelArea = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10)
            };

            Controls.Add(workPanelArea);
        }

        /// <summary>
        /// 従業員の打刻パネルを追加する
        /// </summary>
        /// <param name="employee">対象従業員</param>
        public void AddEmployeePanel(Employee employee)
        {
            // 会社に所属させる（重複防止）
            if (!company.Equals(employee.In()))
            {
                employee.AddCompany(company);
            }

            var panel = new Panel
            {
                Size = new Size(360, 90),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            var nameLabel = new Label
            {
                Text = $"社員: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true
            };

            var logLabel = new Label
            {
                Location = new Point(10, 50),
                AutoSize = true
            };

            var clockInButton = new Button
            {
                Text = "出勤",
                Location = new Point(150, 10),
                Size = new Size(60, 25)
            };

            var clockOutButton = new Button
            {
                Text = "退勤",
                Location = new Point(220, 10),
                Size = new Size(60, 25)
            };

            clockInButton.Click += (sender, e) =>
            {
                try
                {
                    employee.ClockIn();
                    logLabel.Text = $"{employee.Name} 出勤: {DateTime.Now:HH:mm:ss}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"出勤エラー: {ex.Message}");
                }
            };

            clockOutButton.Click += (sender, e) =>
            {
                try
                {
                    employee.ClockOut();
                    logLabel.Text = $"{employee.Name} 退勤: {DateTime.Now:HH:mm:ss}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"退勤エラー: {ex.Message}");
                }
            };

            panel.Controls.Add(nameLabel);
            panel.Controls.Add(clockInButton);
            panel.Controls.Add(clockOutButton);
            panel.Controls.Add(logLabel);

            workPanelArea.Controls.Add(panel);
        }
    }
}
