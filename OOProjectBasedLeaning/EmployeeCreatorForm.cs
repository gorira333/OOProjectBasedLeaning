using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{

    public partial class EmployeeCreatorForm : Form
    {

        private int employeeId = 10000;

        public EmployeeCreatorForm()
        {

            InitializeComponent();

        }

        private void CreateGuestEvent(object sender, EventArgs e)
        {


            var employee = CreateEmployee();

            // パネル追加
            Controls.Add(new EmployeePanel(employee)
            {
                Location = new Point(10, 10 + Controls.Count * 30),
                Width = 300,
            });

            // HomeForm に送信
            var homeForm = Application.OpenForms
                .OfType<HomeForm>()
                .FirstOrDefault();

            if (homeForm != null)
            {
                homeForm.AddEmployee(employee);
            }

        }


        private Employee CreateEmployee()
        {

            employeeId++;

            return new EmployeeModel(employeeId, "Employee" + employeeId);

        }
        public void PrepareForWork(Employee employee)
        {
            // Controls の中から該当 EmployeePanel を探す
            foreach (Control ctrl in Controls)
            {
                if (ctrl is EmployeePanel panel && panel.EmployeeId == employee.Id)
                {
                    panel.SetupWorkButtons();
                    break;
                }
            }
        }


    }

}
