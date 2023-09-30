using ECOMMERCE.Customer;
using ECOMMERCE.Helper;
using ECOMMERCE.User;
using Model.Dto;
using Services.CustomerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ECOMMERCE
{
    public partial class CustomerForm : Form
    {
        private ErrorProvider errorProvider;
        private TextBox txtName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private TextBox txtCity;
        private TableLayoutPanel tableLayoutPanel;
        public delegate void ValidationDelegate(object sender, CancelEventArgs e);
        private string apiUrl;

        public CustomerForm()
        {
            InitializeComponent();
            ConfigureControls();
            errorProvider = new ErrorProvider(this);
            Text = "Customer";
        }

        private void ConfigureControls()
        {
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Padding = new Padding(50);
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            this.Controls.Add(tableLayoutPanel);

            Label titleLabel = Util.CreateTitleLabel(tableLayoutPanel, "Customer");


            Util.CreateLabelAndTextBox("Name:", out Label lblName, out txtName, 1, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Last Name:", out Label lblLastName, out txtLastName, 2, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Email:", out Label lblEmail, out txtEmail, 3, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Address:", out Label lblAddress, out txtAddress, 4, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("City:", out Label lblCity, out txtCity, 5, tableLayoutPanel, ValidateFieldLength);

            Button btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);
            tableLayoutPanel.Controls.Add(btnSave, 1, 6);
        }


        private async void btnSave_Click(object sender, EventArgs e)
        {
            var customerService = new CustomerRepository(apiUrl, TokenManager.GetAccessToken());

            var customer = new CustomerDto
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                City = txtCity.Text
            };

            try
            {
                var createdCustomer = await customerService.CreateCustomerAsync(customer);
                MessageBox.Show("Customer created.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var customerForm = new CustomerListForm();
                customerForm.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error happen creating the client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateFieldLength(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int maxLength = 100;

            if (textBox == txtName || textBox == txtLastName || textBox == txtEmail || textBox == txtCity)
            {
                maxLength = 50;
            }

            if (textBox.Text.Length > maxLength)
            {
                e.Cancel = true;
                errorProvider.SetError(textBox, $"The {textBox.Tag} field cannot exceed {maxLength} characters.");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(textBox, "");
            }
        }
    }
}
