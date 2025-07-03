namespace OOProjectBasedLeaning
{

    public partial class Form1 : Form
    {
        /*
        アプリケーションの起動時に呼ばれる「最初のフォーム」
        */

        public Form1()
        {

            InitializeComponent();

            // 従業員の作成
            new EmployeeCreatorForm().Show();

            // 家
            new HomeForm().Show();

            // 会社
            new CompanyForm().Show();

        }

    }

}
