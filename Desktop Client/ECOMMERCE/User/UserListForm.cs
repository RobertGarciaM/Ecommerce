using ECOMMERCE.Helper;
using Model.Dto;
using Model.Interfaces;
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

namespace ECOMMERCE.User
{
    public partial class UserListForm : Form
    {
        private string apiUrl;
        private DataGridView dataGridView;
        private UserRepository userRepository;
        private Button btnOpenNewWindow;

        public UserListForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeButton();
            Load += CustomerListForm_Load;
            userRepository = new UserRepository(apiUrl, TokenManager.GetAccessToken());
        }

        private void InitializeButton()
        {
            btnOpenNewWindow = new Button();
            btnOpenNewWindow.Text = "Create User";
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
                dataGridView.DataSource = await userRepository.GetAllUsersAsync();

                var updateColumn = new DataGridViewButtonColumn
                {
                    Text = "Update",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Update User",
                    Name = "Update"
                };

                var deleteColumn = new DataGridViewButtonColumn
                {
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    CellTemplate = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    HeaderText = "Delete User",
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
            var form = new UserForm();
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
                        var editedUser = (UserDto)dataGridView.Rows[e.RowIndex].DataBoundItem;
                        bool updated = await userRepository.UpdateUserAsync(editedUser.Id, editedUser);

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
                        var userToDelete = (UserDto)dataGridView.Rows[e.RowIndex].DataBoundItem;
                        bool deleted = await userRepository.DeleteUserAsync(userToDelete.Id);

                        if (deleted)
                        {
                            dataGridView.DataSource = await userRepository.GetAllUsersAsync();
                            MessageBox.Show("User deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error deleting user in the API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}");
                    }
                }
            }
        }
    }
}
