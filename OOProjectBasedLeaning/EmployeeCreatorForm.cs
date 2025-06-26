using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace OOProjectBasedLeaning
{
    public partial class EmployeeCreatorForm : Form
    {
        private int employeeId = 10000;
        private FlowLayoutPanel employeeContainer;
        private Button createButton;
        private TextBox nameInput; // ← 名前入力欄

        public EmployeeCreatorForm()
        {
            InitializeComponent();
            SetupFormStyle();
            InitializeLayout();
        }

        private void SetupFormStyle()
        {
            this.Text = "Employee Creator";
            this.Size = new Size(400, 600);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
        }

        private void InitializeLayout()
        {
            // 名前入力欄
            nameInput = new TextBox
            {
                PlaceholderText = "名前を入力してください",
                Width = 300,
                Margin = new Padding(10)
            };

            // 作成ボタン
            createButton = new Button
            {
                Text = "Create Employee",
                Width = 200,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(10)
            };
            createButton.Click += CreateGuestEvent;

            // 従業員パネルコンテナ
            employeeContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // レイアウトパネル
            var inputPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            inputPanel.Controls.Add(nameInput);
            inputPanel.Controls.Add(createButton);

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainPanel.Controls.Add(inputPanel, 0, 0);
            mainPanel.Controls.Add(employeeContainer, 0, 1);

            this.Controls.Add(mainPanel);
        }

        private void CreateGuestEvent(object sender, EventArgs e)
        {
            string inputName = nameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(inputName))
            {
                MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var employee = CreateEmployee(inputName);

            var panel = new EmployeePanel(employee)
            {
                Width = 340,
                Height = 100,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            employeeContainer.Controls.Add(panel);
            NotifyHomeForm(employee);

            nameInput.Clear(); // 入力欄をクリア
        }

        // 名前付き従業員の作成
        private Employee CreateEmployee(string name)
        {
            employeeId++;
            return new EmployeeModel(employeeId, name);
        }

        private void NotifyHomeForm(Employee employee)
        {
            Application.OpenForms
                .OfType<HomeForm>()
                .FirstOrDefault()
                ?.AddEmployee(employee);
        }

        private void EmployeeCreatorForm_Load(object sender, EventArgs e)
        {
        }
    }
}
