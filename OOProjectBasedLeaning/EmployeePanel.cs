using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    // 従業員情報を表示するパネル
    public class EmployeePanel : Panel
    {
        // 従業員IDを外部から取得するためのプロパティ
        public int EmployeeId => employee.Id;

        // 従業員オブジェクトを外部に公開（必要な情報アクセス用）
        public Employee Employee => employee;

        // 従業員データ
        private Employee employee;


        // コンストラクタ：パネルを初期化し、UIを構築する

        public EmployeePanel(Employee employee)
        {
            this.employee = employee;

            this.Size = new Size(320, 140);
            this.BorderStyle = BorderStyle.FixedSingle;
            // パネルのサイズと境界線を設定
            InitializeComponent();      // ラベルとテキストボックスのUI構築
            SetupStatusButton();        // 状態確認ボタンのUI構築
        }
        // 名前表示ラベルと名前編集用テキストボックスを初期化

        private void InitializeComponent()
        {
            Label employeeNameLabel = new Label
            {
                Text = employee.Name,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            // 名前を編集するテキストボックス
            TextBox guestNameTextBox = new TextBox
            {
                Text = employee.Name,
                Location = new Point(140, 6),
                Width = 160
            };
            // テキスト変更時にラベルとEmployeeの名前を更新
            guestNameTextBox.TextChanged += (sender, e) =>
            {
                employeeNameLabel.Text = guestNameTextBox.Text;
                employee.Name = guestNameTextBox.Text;
            };


            // コントロールをパネルに追加
            Controls.Add(employeeNameLabel);
            Controls.Add(guestNameTextBox);


        }
        // 状態確認ボタンを追加し、勤務状態のダイアログ表示機能を実装
        public void SetupStatusButton()
        {
            var statusButton = new Button
            {
                Text = "状態確認",
                Location = new Point(20, 50),
                Size = new Size(110, 40)
            };
            // ボタンクリック時に、社員情報と勤務状態を表示
            statusButton.Click += (sender, e) =>
            {
                string message = $"社員ID：{employee.Id}\n" +
                 $"名前：{employee.Name}\n" +
                 $"勤務状態：{employee.GetStatusMessage()}";
                MessageBox.Show(message, "勤務状態の確認");
            };
            
            Controls.Add(statusButton);
        }
        



    }
}