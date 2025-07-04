﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{
    public class EmployeePanel : Panel
    {
        public int EmployeeId => employee.Id;
        private Employee employee;
        
        public Employee Employee => employee;


       
       
        public EmployeePanel(Employee employee)
        {
            this.employee = employee;

            this.Size = new Size(320, 140);
            this.BorderStyle = BorderStyle.FixedSingle;

            InitializeComponent();
            SetupStatusButton();
        }

        private void InitializeComponent()
        {
            Label employeeNameLabel = new Label
            {
                Text = employee.Name,
                AutoSize = true,
                Location = new Point(20, 10)
            };

            TextBox guestNameTextBox = new TextBox
            {
                Text = employee.Name,
                Location = new Point(140, 6),
                Width = 160
            };
            // ✅ TextBoxの変更処理
            guestNameTextBox.TextChanged += (sender, e) =>
            {
                employeeNameLabel.Text = guestNameTextBox.Text;
                employee.Name = guestNameTextBox.Text;
            };


            Controls.Add(employeeNameLabel);
                Controls.Add(guestNameTextBox);

            
        }

        public void SetupStatusButton()
        {
            var statusButton = new Button
            {
                Text = "状態確認",
                Location = new Point(20, 50),
                Size = new Size(110, 40)
            };

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