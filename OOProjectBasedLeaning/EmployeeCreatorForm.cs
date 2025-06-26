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
        private Random rand = new Random(); // ★ 名前生成用の乱数

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

        // ★ 名前付き従業員オブジェクトの作成
        private Employee CreateEmployee()
        {
            employeeId++;

            // 日本風の名前を自動生成
            string[] firstNames = { "太郎", "花子", "一郎", "さくら", "健太", "美咲", "陽翔", "結衣" };
            string[] lastNames = { "田中", "佐藤", "鈴木", "高橋", "伊藤", "山本", "中村", "渡辺" };

            string fullName = $"{lastNames[rand.Next(lastNames.Length)]} {firstNames[rand.Next(firstNames.Length)]}";

            return new EmployeeModel(employeeId, fullName);
        }

        // HomeFormに従業員情報を送信
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
