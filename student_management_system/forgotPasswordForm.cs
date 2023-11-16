using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace student_management_system
{
    public partial class forgotPasswordForm : KryptonForm
    {
        string verificationCode;
        public static string to;

        public forgotPasswordForm()
        {
            InitializeComponent();
        }

        private void exitIcon_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backIcon_Click(object sender, EventArgs e)
        {
            loginForm backTologinForm = new loginForm();
            this.Hide();
            backTologinForm.Show();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            verificationCode = rand.Next(999999).ToString();

            MailMessage mailMessage = new MailMessage();
            to = userEmail.Text;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            if (string.IsNullOrWhiteSpace(to) || !regex.IsMatch(to))
            {
                MessageBox.Show("Please provide a valid email address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                mailMessage.To.Add(to);
                mailMessage.From = new MailAddress("darkdoor77@gmail.com");
                mailMessage.Body = $"Your verification code is: {verificationCode}";
                mailMessage.Subject = "Password Reset Verification Code";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential("darkdoor77@gmail.com", "nqwvmhvtzjfgrguk");

                try
                {
                    smtpClient.Send(mailMessage);
                    MessageBox.Show("Your verification code was successfully sent", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void verifyBtn_Click(object sender, EventArgs e)
        {
            if (verificationCode == userVerificationCode.Text)
            {
                to = userEmail.Text;
                resetPasswordForm newResetPasswordForm = new resetPasswordForm();
                this.Hide();
                newResetPasswordForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid verification code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
