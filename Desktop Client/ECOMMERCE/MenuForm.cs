using ECOMMERCE.Customer;
using ECOMMERCE.Product;
using ECOMMERCE.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECOMMERCE
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            InitializeButtons();


        }

        private void InitializeButtons()
        {
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.AutoSize = false;

            Button btnUsers = new Button();
            Button btnCustomer = new Button();
            Button btnProducts = new Button();

            btnUsers.Text = "Users";
            btnCustomer.Text = "Customers";
            btnProducts.Text = "Products";

            btnUsers.Click += BtnUsers_Click;
            btnCustomer.Click += BtnClients_Click;
            btnProducts.Click += BtnProductos_Click;

            buttonPanel.Controls.Add(btnUsers);
            buttonPanel.Controls.Add(btnCustomer);
            buttonPanel.Controls.Add(btnProducts);

            Controls.Add(buttonPanel);
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            UserListForm ListForm = new UserListForm();
            ListForm.ShowDialog();
        }

        private void BtnClients_Click(object sender, EventArgs e)
        {
            CustomerListForm ListForm = new CustomerListForm();
            ListForm.ShowDialog();
        }

        private void BtnProductos_Click(object sender, EventArgs e)
        {
            ProductListForm ListForm = new ProductListForm();
            ListForm.ShowDialog();
        }
    }
}
