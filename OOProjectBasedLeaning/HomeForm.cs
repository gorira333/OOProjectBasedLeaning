using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    /*
 * 自宅側の画面。
 * 従業員の「出発」や「帰宅」など、自宅からの行動を管理する役割を持つ。
 * 従業員一覧もこの画面に表示される。
 */
    public partial class HomeForm : Form
    {
        // すべての従業員パネルを並べるエリア
        private FlowLayoutPanel employeeListPanel;

        public HomeForm()
        {
            InitializeComponent();
            SetupFormStyle();
            InitializeLayout();
        }

        // フォームの外観設定
        private void SetupFormStyle()
        {
            this.Text = "Home - 従業員リスト";
            this.Size = new Size(450, 600);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);
        }

        // 従業員パネル用レイアウト初期化
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

        // 従業員情報をこの画面にパネル形式で追加する
        public void AddEmployee(Employee employee)
        {
            var employeePanel = new EmployeePanel(employee);

           

            var panel = new Panel
            {
                Size = new Size(400, 70),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var label = new Label
            {
                Text = $"登録済: {employee.Name}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            


            var departButton = CreateStyledButton("出発", new Point(200, 10));
            var homeButton = CreateStyledButton("帰宅", new Point(285, 10));

            // 出発ボタンクリック時処理
            departButton.Click += (sender, e) =>
            {
                employee.Depart();  // 出発時間を記録

                // 会社画面が開いている場合、その画面にこの従業員を追加

                var companyForm = Application.OpenForms
                    .OfType<CompanyForm>()
                    .FirstOrDefault();

                if (companyForm != null)
                {
                    companyForm.AddEmployeePanel(employee);
                }
            };

            // 帰宅ボタンを押したときの処理
            homeButton.Click += (sender, e) =>
            {
                try
                {
                    // 出勤中であれば先に退勤処理
                    if (employee.IsAtWork())
                    {
                        employee.ClockOut();
                    }

                    // 帰宅記録
                    employee.ArriveHome();

                    // 成功メッセージを表示
                    MessageBox.Show($"{employee.Name} は帰宅しました。", "帰宅完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // 例外が出たときにメッセージ表示（例：未出勤で退勤しようとした）
                    MessageBox.Show($"帰宅エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            panel.Controls.Add(label);
            panel.Controls.Add(departButton);
            panel.Controls.Add(homeButton);
            employeeListPanel.Controls.Add(panel);
        }

        // ボタンの共通スタイル設定
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
            // 必要に応じて初期処理をここへ
        }
    }
}
