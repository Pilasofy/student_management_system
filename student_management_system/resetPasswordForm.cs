using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace student_management_system
{
    public partial class resetPasswordForm : KryptonForm
    {
        string userEmail = forgotPasswordForm.to;

        public resetPasswordForm()
        {
            InitializeComponent();
        }

        static string connectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=student_ms;Data Source=Mohamed-Kiyas\SQLEXPRESS";
        static SqlConnection connectedConnection = new SqlConnection(connectionString);

        private void exitIcon_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backIcon_Click(object sender, EventArgs e)
        {
            forgotPasswordForm backToForgotPasswordForm = new forgotPasswordForm();
            this.Hide();
            backToForgotPasswordForm.Show();
        }

        private void checkPassword_Click(object sender, EventArgs e)
        {
            if (checkPassword.Checked)
            {
                setNewPassword.PasswordChar = '\0';
                confirmPassword.PasswordChar = '\0';
            }
            else
            {
                setNewPassword.PasswordChar = '*';
                confirmPassword.PasswordChar = '*';
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectedConnection.State == ConnectionState.Closed)
                {
                    connectedConnection.Open();
                }

                if (string.IsNullOrEmpty(setNewPassword.Text) || string.IsNullOrEmpty(confirmPassword.Text))
                {
                    MessageBox.Show("Both of these fields should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (setNewPassword.Text == confirmPassword.Text)
                    {
                        // Use parameterized query to avoid SQL injection
                        string query = "UPDATE [dbo].[users] SET user_password = @newPassword WHERE user_email = @userEmail";

                        using (SqlCommand cmd = new SqlCommand(query, connectedConnection))
                        {
                            // Use parameters to avoid SQL injection
                            cmd.Parameters.AddWithValue("@newPassword", confirmPassword.Text);
                            cmd.Parameters.AddWithValue("@userEmail", userEmail);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            connectedConnection.Close();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                loginForm backToLoginForm = new loginForm();
                                this.Hide();
                                backToLoginForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("New password and Confirm password do not match. Both these fields must contain a same password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                connectedConnection.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
