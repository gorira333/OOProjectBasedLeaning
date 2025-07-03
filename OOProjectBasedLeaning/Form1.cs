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

            // ���̃t�H�[���͏]���ǂ���J��
            new EmployeeCreatorForm().Show();
            new HomeForm().Show();
            new CompanyForm().Show();

            // ����UI������
            InitializeSearchUI();
        }

        private void InitializeSearchUI()
        {
            this.Text = "���C���t�H�[���i�����@�\�t���j";
            this.Size = new Size(450, 400);
            this.Font = new Font("Segoe UI", 10);

            idInputBox = new TextBox
            {
                Location = new Point(20, 20),
                Width = 200,
                PlaceholderText = "�Ј�ID�����"
            };

            searchButton = new Button
            {
                Text = "����",
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
            resultLabel.Text = ""; // �O�̌��ʂ��N���A

            if (!int.TryParse(idInputBox.Text, out int employeeId))
            {
                resultLabel.Text = "�L���ȎЈ�ID����͂��Ă�������";
                return;
            }

            var companyForm = Application.OpenForms
                .OfType<CompanyForm>()
                .FirstOrDefault();

            if (companyForm == null)
            {
                resultLabel.Text = "CompanyForm ���J����Ă��܂���";
                return;
            }

            var company = companyForm.CompanyInstance;
            var employee = company.FindEmployeeById(employeeId);

            if (employee is NullEmployee)
            {
                resultLabel.Text = "�Ј���������܂���";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"�Ј�ID�F{employee.Id}");
            sb.AppendLine($"�Ј����F{employee.Name}");
            sb.AppendLine();
            sb.AppendLine("���Ζ�������");

            var sessions = employee.GetWorkSessions();
            if (!sessions.Any()) // �� using System.Linq; ���K�v
            {
                sb.AppendLine("�Ζ������͂���܂���");
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
