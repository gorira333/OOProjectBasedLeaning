namespace OOProjectBasedLeaning
{

    public partial class Form1 : Form
    {
        /*
        �A�v���P�[�V�����̋N�����ɌĂ΂��u�ŏ��̃t�H�[���v
        */

        public Form1()
        {

            InitializeComponent();

            // �]�ƈ��̍쐬
            new EmployeeCreatorForm().Show();

            // ��
            new HomeForm().Show();

            // ���
            new CompanyForm().Show();

        }

    }

}
