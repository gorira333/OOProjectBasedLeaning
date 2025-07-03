using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OOProjectBasedLeaning;


namespace OOProjectBasedLeaning
{
    /*
   会社で従業員の出退勤を管理する画面
    */
    public partial class CompanyForm : Form
    {
        private Company company = NullCompany.Instance;
        private FlowLayoutPanel workPanelArea;
        public Company CompanyInstance => company;


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
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Controls.Add(workPanelArea);
            this.Text = "社員打刻管理";
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.WhiteSmoke;
            this.Size = new Size(400, 700);
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
                Size = new Size(360, 110),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
            };

            var nameLabel = new Label
            {
                Text = $"社員: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30)
            };

            var clockInLabel = new Label
            {
                Location = new Point(10, 50),
                AutoSize = true,
                Text = $"{employee.Name} 出勤: 未登録",
                ForeColor = Color.DarkGreen,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };

            var clockOutLabel = new Label
            {
                Location = new Point(10, 75),
                AutoSize = true,
                Text = $"{employee.Name} 退勤: 未登録",
                ForeColor = Color.DarkRed,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };

            var clockInButton = new Button
            {
                Text = "出勤",
                Location = new Point(220, 15),
                Size = new Size(60, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            clockInButton.FlatAppearance.BorderSize = 0;

            var clockOutButton = new Button
            {
                Text = "退勤",
                Location = new Point(290, 15),
                Size = new Size(60, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            clockOutButton.FlatAppearance.BorderSize = 0;

            clockInButton.Click += (sender, e) =>
            {
                try
                {
                    employee.ClockIn();
                    var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    clockInLabel.Text = $"{employee.Name} 出勤: {now}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"出勤エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            clockOutButton.Click += (sender, e) =>
            {
                employee.ClockOut();
                var now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                clockOutLabel.Text = $"{employee.Name} 退勤: {now}";
            };

            panel.Controls.Add(nameLabel);
            panel.Controls.Add(clockInLabel);
            panel.Controls.Add(clockOutLabel);
            panel.Controls.Add(clockInButton);
            panel.Controls.Add(clockOutButton);

            workPanelArea.Controls.Add(panel);
        }

        private void CompanyForm_Load(object sender, EventArgs e)
        {
            // 初期ロード処理があればここに
        }
    }
}
