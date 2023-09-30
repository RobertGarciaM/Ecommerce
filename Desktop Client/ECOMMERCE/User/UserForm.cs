using ECOMMERCE.Customer;
using ECOMMERCE.Helper;
using Model.Dto;
using Services;
using Services.CustomerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ECOMMERCE.User
{
    public partial class UserForm : Form
    {
        private ErrorProvider errorProvider;
        private TableLayoutPanel tableLayoutPanel;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private ComboBox cmbRole;
        private string apiUrl;
        public UserForm()
        {
            InitializeComponent();
            ConfigureControls();
            errorProvider = new ErrorProvider(this);
            this.Text = "User";
        }

        private void ConfigureControls()
        {
            List<string> options = new List<string> { "Admin", "Standard" };
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Padding = new Padding(50);
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            this.Controls.Add(tableLayoutPanel);

            Label titleLabel = Util.CreateTitleLabel(tableLayoutPanel, "User");


            Util.CreateLabelAndTextBox("Email:", out Label lblName, out txtEmail, 1, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Password:", out Label lblLastName, out txtPassword, 2, tableLayoutPanel, ValidateFieldLength, true);
            Util.CreateLabelAndTextBox("Confirm Password:", out Label lblEmail, out txtConfirmPassword, 3, tableLayoutPanel, ValidateFieldLength, true);
            Util.CreateLabelAndComboBox("Select a Role:", out Label lblComboBox, out cmbRole, 4, tableLayoutPanel, options);

            Button btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);
            tableLayoutPanel.Controls.Add(btnSave, 1, 6);
        }

        private void ValidateFieldLength(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int maxLength = 50;

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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var userRepository = new UserRepository(apiUrl, TokenManager.GetAccessToken());

            var user = new CreateUserDto
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                ConfirmPassword = txtConfirmPassword.Text,
                Role = cmbRole.Text
            };

            try
            {
                var createdCustomer = await userRepository.CreateUserAsync(user);
                MessageBox.Show("User created.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var customerForm = new UserListForm();
                customerForm.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error happen creating the client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
