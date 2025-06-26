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
            SetupFormStyle();
            InitializeLayout();
        }

        // �t�H�[���̊O�ϐݒ�
        private void SetupFormStyle()
        {
            this.Text = "Home - �]�ƈ����X�g";
            this.Size = new Size(450, 600);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
        }

        // �]�ƈ��p�l���p���C�A�E�g������
        private void InitializeLayout()
        {
            employeeListPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            Controls.Add(employeeListPanel);
        }

        // �]�ƈ����p�l���Ƃ��Ēǉ�
        public void AddEmployee(Employee employee)
        {
            var panel = new Panel
            {
                Size = new Size(400, 70),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var label = new Label
            {
                Text = $"�o�^��: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var departButton = CreateStyledButton("�o��", new Point(200, 10));
            var homeButton = CreateStyledButton("�A��", new Point(285, 10));

            // �o���{�^���N���b�N������
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

            // �A��{�^���N���b�N������
            homeButton.Click += (sender, e) =>
            {
                try
                {
                    if (employee.IsAtWork())
                    {
                        employee.ClockOut();
                    }

                    employee.ArriveHome();
                    MessageBox.Show($"{employee.Name} �͋A��܂����B", "�A���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�A��G���[: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            panel.Controls.Add(label);
            panel.Controls.Add(departButton);
            panel.Controls.Add(homeButton);
            employeeListPanel.Controls.Add(panel);
        }

        // �{�^���̋��ʃX�^�C���ݒ�
        private Button CreateStyledButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(75, 30),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            // �K�v�ɉ����ď���������������
        }
    }
}
