using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace student_management_system
{
    public partial class registerForm : KryptonForm
    {
        public registerForm()
        {
            InitializeComponent();
        }

        static string connectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=student_ms;Data Source=Mohamed-Kiyas\SQLEXPRESS";
        static SqlConnection connectedConnection = new SqlConnection(connectionString);

        private void label5_Click(object sender, EventArgs e)
        {
            loginForm newloginForm = new loginForm();
            this.Hide();
            newloginForm.Show();
        }

        private void backIcon_Click(object sender, EventArgs e)
        {
            loginForm newloginForm = new loginForm();
            this.Hide();
            newloginForm.Show();
        }

        private void exitIcon_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPassword.Checked)
            {
                newPassword.PasswordChar = '\0';
                confirmPassword.PasswordChar = '\0';
            }
            else
            {
                newPassword.PasswordChar = '*';
                confirmPassword.PasswordChar = '*';
            }
        }

        bool IsValidUsername(string username)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-zA-Z ]+$");
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);

                if (mailAddress.Host.IndexOf('.') == -1)
                {
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectedConnection.State == ConnectionState.Closed)
                {
                    connectedConnection.Open();
                }

                if (string.IsNullOrEmpty(fullName.Text) || string.IsNullOrEmpty(emailAddress.Text) || string.IsNullOrEmpty(newPassword.Text) || string.IsNullOrEmpty(confirmPassword.Text) || string.IsNullOrEmpty(roleType.Text))
                {
                    MessageBox.Show("All fields should be filled out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (fullName.Text.Length < 4 || fullName.Text.Length > 20)
                {
                    MessageBox.Show("Username must be between 4 and 20 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!IsValidUsername(fullName.Text))
                {
                    MessageBox.Show("Invalid characters in the username. Please use only letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!IsValidEmail(emailAddress.Text))
                {
                    MessageBox.Show("Invalid email address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (newPassword.Text == confirmPassword.Text)
                    {
                        // Use parameterized query to avoid SQL injection
                        string query = "INSERT INTO [dbo].[users] (user_full_name, user_email, user_password, user_role, created_at, updated_at) VALUES (@fullName, @emailAddress, @confirmPassword, @roleType, @createdAt, @updatedAt)";

                        using (SqlCommand cmd = new SqlCommand(query, connectedConnection))
                        {
                            // Use parameters to avoid SQL injection
                            cmd.Parameters.AddWithValue("@fullName", fullName.Text);
                            cmd.Parameters.AddWithValue("@emailAddress", emailAddress.Text);
                            cmd.Parameters.AddWithValue("@confirmPassword", confirmPassword.Text);
                            cmd.Parameters.AddWithValue("@roleType", roleType.Text);

                            // Use DateTime.UtcNow for both createdAt and updatedAt
                            DateTime currentTime = DateTime.UtcNow;
                            cmd.Parameters.AddWithValue("@createdAt", currentTime);
                            cmd.Parameters.AddWithValue("@updatedAt", currentTime);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            connectedConnection.Close();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"{fullName.Text} your are registered successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                loginForm backToLoginForm = new loginForm();
                                this.Hide();
                                backToLoginForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Failed to register due to the following error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The entered passwords do not match. Please make sure both password fields contain the same value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
