using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    public partial class EmployeeCreatorForm : Form
    {
        private int employeeId = 10000;
        private FlowLayoutPanel employeeContainer;
        private Button createButton;

        public EmployeeCreatorForm()
        {
            InitializeComponent();
            SetupFormStyle();
            InitializeLayout();
        }

        // フォームの見た目・基本設定を整える
        private void SetupFormStyle()
        {
            this.Text = "Employee Creator";
            this.Size = new Size(400, 600);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
        }

        // レイアウト初期化（ボタンと従業員パネル用コンテナを作成）
        private void InitializeLayout()
        {
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

            // 従業員パネルを縦に積むコンテナ
            employeeContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // 全体レイアウト用パネル
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainPanel.Controls.Add(createButton, 0, 0);
            mainPanel.Controls.Add(employeeContainer, 0, 1);

            this.Controls.Add(mainPanel);
        }

        // 作成ボタンクリック時の処理
        private void CreateGuestEvent(object sender, EventArgs e)
        {
            var employee = CreateEmployee();

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
        }

        // Employee作成（IDインクリメントと名前付与）
        private Employee CreateEmployee()
        {
            employeeId++;
            return new EmployeeModel(employeeId, $"Employee{employeeId}");
        }

        // HomeFormに従業員情報を送信
        private void NotifyHomeForm(Employee employee)
        {
            Application.OpenForms
                .OfType<HomeForm>()
                .FirstOrDefault()
                ?.AddEmployee(employee);
        }
    }
}
