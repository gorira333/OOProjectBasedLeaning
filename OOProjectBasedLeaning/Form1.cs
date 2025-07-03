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

            // ���̋@�\�͂��̂܂�
            new EmployeeCreatorForm().Show();
            new HomeForm().Show();
            new CompanyForm().Show();

            // UI��������ǉ�
            InitializeSearchUI();
        }

        private void InitializeSearchUI()
        {
            this.Text = "���C���t�H�[���i�����@�\�t���j";
            this.Size = new Size(400, 200);
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
                        resultLabel.Text = "�Ј���������܂���";
                    else
                        resultLabel.Text = $"�Ј����F{employee.Name}";
                }
                else
                {
                    resultLabel.Text = "CompanyForm ���J����Ă��܂���";
                }
            }
            else
            {
                resultLabel.Text = "�L���ȎЈ�ID����͂��Ă�������";
            }
        }
    }
}
