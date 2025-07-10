using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace OOProjectBasedLeaning
{
    /*
    * EmployeeCreatorForm
    * -------------------
    * 新しい従業員を作成し、UIに表示する画面。
    * 入力された名前をもとに EmployeeModel を生成し、
    * その情報を EmployeePanel として表示する。
    * また、HomeForm にも従業員データを通知する。
    */
    public partial class EmployeeCreatorForm : Form
    {
        private int employeeId = 10000;
        // 従業員パネルを格納するコンテナ
        private FlowLayoutPanel employeeContainer;
        // 作成ボタンと名前入力欄
        private Button createButton;
        private TextBox nameInput; 

        public EmployeeCreatorForm()
        {
            InitializeComponent();
            SetupFormStyle();// フォームの外観設定
            InitializeLayout();// レイアウト構築
        }
        // フォームの基本的なスタイル設定

        private void SetupFormStyle()
        {
            this.Text = "Employee Creator";
            this.Size = new Size(400, 600);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
        }
        // UIコンポーネントのレイアウトを構築
        private void InitializeLayout()
        {
            // 名前入力欄
            nameInput = new TextBox
            {
                PlaceholderText = "名前を入力してください",
                Width = 300,
                Margin = new Padding(10)
            };

            // 「Create Employee」ボタンの設定
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

            // 従業員パネルを縦方向に並べるコンテナ
            employeeContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // 入力エリアの配置
            var inputPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            inputPanel.Controls.Add(nameInput);
            inputPanel.Controls.Add(createButton);
            // 入力と一覧を分けて配置するテーブルレイアウト
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
        // ボタンクリックで従業員作成処理を実行

        private void CreateGuestEvent(object sender, EventArgs e)
        {
            string inputName = nameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(inputName))
            {
                MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isDuplicate = employeeContainer.Controls
               .OfType<EmployeePanel>()
               .Any(panel => panel.Employee.Name.Equals(inputName, StringComparison.OrdinalIgnoreCase));

            if (isDuplicate) {
                MessageBox.Show("同じ名前の従業員は作成できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 従業員を作成し、表示用のパネルを生成
            var employee = CreateEmployee(inputName);

            var panel = new EmployeePanel(employee)
            {
                Width = 340,
                Height = 100,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };
            // UIに追加
            employeeContainer.Controls.Add(panel);

            // HomeForm にも通知して一元管理させる
            NotifyHomeForm(employee);

            // 入力欄をリセット
            nameInput.Clear();
        }

        // 一意の社員IDで従業員インスタンスを作成
        private Employee CreateEmployee(string name)
        {
            employeeId++;
            return new EmployeeModel(employeeId, name);
        }
        // HomeForm に従業員を通知する（データ共有）
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
