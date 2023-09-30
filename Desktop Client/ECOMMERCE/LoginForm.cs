using ECOMMERCE.Helper;
using Model.Dto;
using Services.CustomerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ECOMMERCE
{
    public partial class LoginForm : Form
    {
        private ErrorProvider errorProvider;
        private TableLayoutPanel tableLayoutPanel;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private string apiUrl;

        public LoginForm()
        {
            InitializeComponent();
            ConfigureControls();
            errorProvider = new ErrorProvider(this);
            Text = "Login";
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

            Label titleLabel = Util.CreateTitleLabel(tableLayoutPanel, "Login");

            Util.CreateLabelAndTextBox("User:", out Label lblName, out txtUsername, 1, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Password:", out Label lblLastName, out txtPassword, 2, tableLayoutPanel, ValidateFieldLength, true);

            Button btnSave = new Button();
            btnSave.Text = "Login";
            btnSave.Click += new EventHandler(btnLogin_Click);
            tableLayoutPanel.Controls.Add(btnSave, 1, 6);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var loginModel = new LoginModelDto
            {
                UserName = txtUsername.Text,
                Password = txtPassword.Text
            };
          

            var authenticationRepository = new AuthenticationRepository(apiUrl);
            var token = await authenticationRepository.AuthenticateAsync(loginModel);

            if (token != null)
            {
                TokenManager.SetAccessToken(token);
                MenuForm customerForm = new MenuForm();
                ShowForm(customerForm);
            }
            else
            {
                MessageBox.Show("Incorrect User or Password.");
            }
        }

        private void ShowForm(Form form)
        {
            tableLayoutPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(form, 1, 6);
            form.Show();
        }

        private void ValidateFieldLength(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int maxLength = 0;

            if (textBox == txtUsername)
                maxLength = 50;
            else if (textBox == txtPassword)
                maxLength = 50;

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
