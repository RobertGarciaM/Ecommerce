using ECOMMERCE.Helper;
using Model.Dto;
using Model.Interfaces;
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

namespace ECOMMERCE.Customer
{
    public partial class CustomerListForm : Form
    {
        private string apiUrl;
        private DataGridView dataGridViewCustomers;
        private CustomerRepository customerRepository;
        private Button btnOpenNewWindow;

        public CustomerListForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeButton();
            Load += CustomerListForm_Load;
            customerRepository = new CustomerRepository(apiUrl, TokenManager.GetAccessToken());
        }

        private void InitializeButton()
        {
            btnOpenNewWindow = new Button();
            btnOpenNewWindow.Text = "Create Customer";
            btnOpenNewWindow.Dock = DockStyle.Top;
            btnOpenNewWindow.Click += btnOpenNewWindow_Click;
            Controls.Add(btnOpenNewWindow);
        }

        private void InitializeDataGridView()
        {
            dataGridViewCustomers = new DataGridView();
            dataGridViewCustomers.Dock = DockStyle.Fill;
            dataGridViewCustomers.AutoGenerateColumns = true;
            dataGridViewCustomers.ReadOnly = false;
            dataGridViewCustomers.AllowUserToAddRows = false;
            Controls.Add(dataGridViewCustomers);
        }

        private async void CustomerListForm_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridViewCustomers.DataSource = await customerRepository.GetAllCustomersAsync();

                var updateColumn = new DataGridViewButtonColumn
                {
                    Text = "Update",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Update Customer",
                    Name = "Update"
                };

                var deleteColumn = new DataGridViewButtonColumn
                {
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Delete Customer",
                    Name = "Delete"
                };

                dataGridViewCustomers.Columns.Add(updateColumn);
                dataGridViewCustomers.Columns.Add(deleteColumn);
                dataGridViewCustomers.CellContentClick += DataGridViewCustomers_CellContentClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnOpenNewWindow_Click(object sender, EventArgs e)
        {
            var customerForm = new CustomerForm();
            customerForm.Show();
            Close();
        }

        private async void DataGridViewCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dataGridViewCustomers.Columns["Update"].Index)
                {
                    try
                    {
                        var editedCustomer = (CustomerDto)dataGridViewCustomers.Rows[e.RowIndex].DataBoundItem;
                        bool updated = await customerRepository.UpdateCustomerAsync(editedCustomer.IdCustomer, editedCustomer);

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
                else if (e.ColumnIndex == dataGridViewCustomers.Columns["Delete"].Index)
                {
                    try
                    {
                        var customerToDelete = (CustomerDto)dataGridViewCustomers.Rows[e.RowIndex].DataBoundItem;
                        bool deleted = await customerRepository.DeleteCustomerAsync(customerToDelete.IdCustomer);

                        if (deleted)
                        {
                            dataGridViewCustomers.DataSource = await customerRepository.GetAllCustomersAsync();
                            MessageBox.Show("Customer deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error deleting customer in the API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting customer: {ex.Message}");
                    }
                }
            }
        }
    }

}
