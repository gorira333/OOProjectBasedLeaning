using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    public partial class Form1 : Form
    {
        private TextBox idInputBox;
        private Button searchButton;
        private Label resultLabel;

        public Form1()
        {
            InitializeComponent();

            // 他のフォームは従来どおり開く
            new EmployeeCreatorForm().Show();
            new HomeForm().Show();
            new CompanyForm().Show();

            // 検索UI初期化
            InitializeSearchUI();
        }

        private void InitializeSearchUI()
        {
            this.Text = "メインフォーム（検索機能付き）";
            this.Size = new Size(450, 400);
            this.Font = new Font("Segoe UI", 10);

            idInputBox = new TextBox
            {
                Location = new Point(20, 20),
                Width = 200,
                PlaceholderText = "社員IDを入力"
            };

            searchButton = new Button
            {
                Text = "検索",
                Location = new Point(230, 18),
                Size = new Size(80, 30),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White
            };
            searchButton.Click += SearchButton_Click;

            resultLabel = new Label
            {
                Location = new Point(20, 60),
                AutoSize = true
            };

            Controls.Add(idInputBox);
            Controls.Add(searchButton);
            Controls.Add(resultLabel);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            resultLabel.Text = ""; // 前の結果をクリア

            if (!int.TryParse(idInputBox.Text, out int employeeId))
            {
                resultLabel.Text = "有効な社員IDを入力してください";
                return;
            }

            var companyForm = Application.OpenForms
                .OfType<CompanyForm>()
                .FirstOrDefault();

            if (companyForm == null)
            {
                resultLabel.Text = "CompanyForm が開かれていません";
                return;
            }

            var company = companyForm.CompanyInstance;
            var employee = company.FindEmployeeById(employeeId);

            if (employee is NullEmployee)
            {
                resultLabel.Text = "社員が見つかりません";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"社員ID：{employee.Id}");
            sb.AppendLine($"社員名：{employee.Name}");
            sb.AppendLine();
            sb.AppendLine("▼勤務履歴▼");

            var sessions = employee.GetWorkSessions();
            if (!sessions.Any()) // ← using System.Linq; が必要
            {
                sb.AppendLine("勤務履歴はありません");
            }
            else
            {
                foreach (var session in sessions)
                {
                    sb.AppendLine(session.ToString());
                }
            }

            resultLabel.Text = sb.ToString();
        }
    }
}
