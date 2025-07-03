using System;
using System.Drawing;
using System.Linq;
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

            // 元の機能はそのまま
            new EmployeeCreatorForm().Show();
            new HomeForm().Show();
            new CompanyForm().Show();

            // UI初期化を追加
            InitializeSearchUI();
        }

        private void InitializeSearchUI()
        {
            this.Text = "メインフォーム（検索機能付き）";
            this.Size = new Size(400, 200);
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
            if (int.TryParse(idInputBox.Text, out int employeeId))
            {
                var companyForm = Application.OpenForms
                    .OfType<CompanyForm>()
                    .FirstOrDefault();

                if (companyForm != null)
                {
                    var company = companyForm.CompanyInstance;
                    var employee = company.FindEmployeeById(employeeId);

                    if (employee is NullEmployee)
                        resultLabel.Text = "社員が見つかりません";
                    else
                        resultLabel.Text = $"社員名：{employee.Name}";
                }
                else
                {
                    resultLabel.Text = "CompanyForm が開かれていません";
                }
            }
            else
            {
                resultLabel.Text = "有効な社員IDを入力してください";
            }
        }
    }
}
