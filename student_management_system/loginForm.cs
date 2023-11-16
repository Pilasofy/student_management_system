using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace student_management_system
{
    public partial class loginForm : KryptonForm
    {
        public loginForm()
        {
            InitializeComponent();
        }

        static string connectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=student_ms;Data Source=Mohamed-Kiyas\SQLEXPRESS";
        static SqlConnection connectedConnection = new SqlConnection(connectionString);

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectedConnection.State == ConnectionState.Closed)
                {
                    connectedConnection.Open();
                }

                string query = "SELECT user_full_name, user_password FROM users WHERE user_full_name = @UserName";
                SqlCommand cmd = new SqlCommand(query, connectedConnection);

                cmd.Parameters.AddWithValue("@UserName", userName.Text);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader["user_password"].ToString() == userPassword.Text)
                    {
                        MessageBox.Show($"{userName.Text} you are Login Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Username is Incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                connectedConnection.Close();
            }
            catch (Exception ex)
            {
                connectedConnection.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkPassword_Click(object sender, EventArgs e)
        {
            if (checkPassword.Checked)
            {
                userPassword.PasswordChar = '\0';
            }
            else
            {
                userPassword.PasswordChar = '*';
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            forgotPasswordForm newForgotPasswordForm = new forgotPasswordForm();
            this.Hide();
            newForgotPasswordForm.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            registerForm newRegisterForm = new registerForm();
            this.Hide();
            newRegisterForm.Show();
        }
    }
}
