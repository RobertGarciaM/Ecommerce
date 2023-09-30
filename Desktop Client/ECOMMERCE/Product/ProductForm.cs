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

namespace ECOMMERCE.Product
{
    public partial class ProductForm : Form
    {
        private ErrorProvider errorProvider;
        private TextBox txtxDescription;
        private TextBox txtPrice;
        private TableLayoutPanel tableLayoutPanel;
        public delegate void ValidationDelegate(object sender, CancelEventArgs e);
        private string apiUrl;

        public ProductForm()
        {
            InitializeComponent();
            ConfigureControls();
            errorProvider = new ErrorProvider(this);
            Text = "Product";
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

            Label titleLabel = Util.CreateTitleLabel(tableLayoutPanel, "Product");


            Util.CreateLabelAndTextBox("Description:", out Label lblName, out txtxDescription, 1, tableLayoutPanel, ValidateFieldLength);
            Util.CreateLabelAndTextBox("Price:", out Label lblLastName, out txtPrice, 2, tableLayoutPanel, ValidateFieldLength, false, textBox_KeyPress);

            Button btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);
            tableLayoutPanel.Controls.Add(btnSave, 1, 6);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }



        private async void btnSave_Click(object sender, EventArgs e)
        {
            var productService = new ProductRepository(apiUrl, TokenManager.GetAccessToken());

            var product = new ProductDto
            {
                Description = txtxDescription.Text,
                Price = decimal.Parse(txtPrice.Text)
            };

            try
            {
                var createdCustomer = await productService.AddProductAsync(product);
                MessageBox.Show("Product created.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var productForm = new ProductListForm();
                productForm.Show();
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

            if (textBox == txtxDescription)
            {
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
}
