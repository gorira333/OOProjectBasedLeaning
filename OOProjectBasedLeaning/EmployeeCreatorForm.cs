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

            Controls.Add(new EmployeePanel(CreateEmployee())
            {
                Location = new Point(10, 10 + Controls.Count * 30),
                Width = 300,
            });

        }

        private Employee CreateEmployee()
        {

            employeeId++;

            return new EmployeeModel(employeeId, "Employee" + employeeId);

        }

    }

}
