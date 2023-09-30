using ECOMMERCE.Helper;
using ECOMMERCE.User;
using Model.Dto;
using Services;
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
    public partial class ProductListForm : Form
    {
        private string apiUrl;
        private DataGridView dataGridView;
        private ProductRepository productRepository;
        private Button btnOpenNewWindow;

        public ProductListForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeButton();
            Load += CustomerListForm_Load;
            productRepository = new ProductRepository(apiUrl, TokenManager.GetAccessToken());
        }

        private void InitializeButton()
        {
            btnOpenNewWindow = new Button();
            btnOpenNewWindow.Text = "Create Product";
            btnOpenNewWindow.Dock = DockStyle.Top;
            btnOpenNewWindow.Click += btnOpenNewWindow_Click;
            Controls.Add(btnOpenNewWindow);
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.ReadOnly = false;
            dataGridView.AllowUserToAddRows = false;
            Controls.Add(dataGridView);
        }

        private async void CustomerListForm_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView.DataSource = await productRepository.GetAllProductsAsync();

                var updateColumn = new DataGridViewButtonColumn
                {
                    Text = "Update",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Update Product",
                    Name = "Update"
                };

                var deleteColumn = new DataGridViewButtonColumn
                {
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Delete Product",
                    Name = "Delete"
                };

                dataGridView.Columns.Add(updateColumn);
                dataGridView.Columns.Add(deleteColumn);
                dataGridView.CellContentClick += DataGridViewCustomers_CellContentClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnOpenNewWindow_Click(object sender, EventArgs e)
        {
            var form = new ProductForm();
            form.Show();
            Close();
        }

        private async void DataGridViewCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dataGridView.Columns["Update"].Index)
                {
                    try
                    {
                        var editedProduct = (ProductDto)dataGridView.Rows[e.RowIndex].DataBoundItem;
                        bool updated = await productRepository.UpdateProductAsync(editedProduct.IdProduct, editedProduct);

                        if (updated)
                        {
                            MessageBox.Show("Changes saved successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error saving changes to the API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving changes: {ex.Message}");
                    }
                }
                else if (e.ColumnIndex == dataGridView.Columns["Delete"].Index)
                {
                    try
                    {
                        var productToDelete = (ProductDto)dataGridView.Rows[e.RowIndex].DataBoundItem;
                        bool deleted = await productRepository.DeleteProductAsync(productToDelete.IdProduct);

                        if (deleted)
                        {
                            dataGridView.DataSource = await productRepository.GetAllProductsAsync();
                            MessageBox.Show("Product deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error deleting Product in the API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting Product: {ex.Message}");
                    }
                }
            }
        }
    }
}
