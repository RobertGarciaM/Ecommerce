using System.ComponentModel;
using static ECOMMERCE.CustomerForm;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace ECOMMERCE.Helper
{
    public static class Util
    {
        public static Label CreateTitleLabel(TableLayoutPanel tableLayoutPanel, string labelText)
        {
            Label lblTitle = new Label();
            lblTitle.Text = labelText;
            lblTitle.Font = new Font(lblTitle.Font, FontStyle.Bold);
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            tableLayoutPanel.SetColumnSpan(lblTitle, 2);
            tableLayoutPanel.Controls.Add(lblTitle, 0, 0);
            return lblTitle;
        }
        public static void CreateLabelAndTextBox(string labelText, out Label label, out TextBox textBox, int row, 
            TableLayoutPanel tableLayoutPanel, ValidationDelegate validationMethod, bool isPassword = false, KeyPressEventHandler keyPressHandler = null)
        {
            label = new Label();
            label.Text = labelText;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Anchor = AnchorStyles.Right;
            tableLayoutPanel.Controls.Add(label, 0, row);

            tableLayoutPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 150);

            textBox = new TextBox();
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.Controls.Add(textBox, 1, row);
            textBox.Validating += new CancelEventHandler(validationMethod);

            if (isPassword)
            {
                textBox.PasswordChar = '*'; 
            }

            if (keyPressHandler != null)
            {
                textBox.KeyPress += keyPressHandler;
            }
        }

        public static void CreateLabelAndComboBox(string labelText, out Label label, out ComboBox comboBox, int row, 
            TableLayoutPanel tableLayoutPanel, List<string> options, EventHandler selectedIndexChangedHandler = null)
        {
            label = new Label();
            label.Text = labelText;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Anchor = AnchorStyles.Right;
            tableLayoutPanel.Controls.Add(label, 0, row);

            tableLayoutPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 150);

            comboBox = new ComboBox();
            comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.Controls.Add(comboBox, 1, row);

            if (selectedIndexChangedHandler != null)
            {
                comboBox.SelectedIndexChanged += selectedIndexChangedHandler;
            }

            comboBox.Items.AddRange(options.ToArray());
        }


    }
}
