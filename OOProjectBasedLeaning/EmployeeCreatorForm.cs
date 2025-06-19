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

            var panel = new EmployeePanel(employee)
            {
                Location = new Point(10, 10 + Controls.Count * 150), // 縦方向の高さを調整
            };

            Controls.Add(panel);

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



    }

}
