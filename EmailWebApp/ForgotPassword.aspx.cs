using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EmailLibrary.Model;

namespace EmailWebApp
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        //AccountService.Account pxyAccount = new AccountService.Account();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadForgotModalContent();
            if (!IsPostBack)
            {
                MakeVisible("securityCard");
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        /// <summary>
        /// Check whether all responses have been provided and call the SecurityQuestions WebMethod.
        /// If provided the correct number of responses show the "New Password" card on the page. 
        /// Otherwise alert the user check incorrect responses.
        /// </summary>
        protected void btnCheckQuestions_Click(object sender, EventArgs e)
        {
            if (txtResponseCity.Text.Length > 0 &&
                txtResponsePhone.Text.Length > 0 &&
                txtResponseSchool.Text.Length > 0 &&
                txtEmailAddress.Text.Length > 0)
            {
                Account acct = new Account();
                acct.SecurityQuestionCity = txtResponseCity.Text;
                acct.SecurityQuestionPhone = txtResponsePhone.Text;
                acct.SecurityQuestionSchool = txtResponseSchool.Text;
                acct.CreatedEmailAddress = txtEmailAddress.Text;
                int numOfCorrectResponses = acct.SecurityQuestions();
                if (numOfCorrectResponses == 3)
                {
                    MakeVisible("newPasswordCard");
                }
                else
                {
                    Response.Write("<script>alert('Incorrect. Please check your Security Questions and try again.')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('You must answer ALL security questions.')</script>");
            }
        }
        /// <summary>
        /// Check whether the new password/password confirm match and call the UpdatePassword WebMethod.
        /// If the update was successful redirect to the Login page. 
        /// Otherwise alert the user to contact the "HelpDesk".
        /// </summary>
        protected void btnSaveNewPassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Length > 0 &&
                txtPasswordConfirm.Text.Length > 0 &&
                txtNewPassword.Text == txtPasswordConfirm.Text)
            {
                Account acct = new Account();
                acct.AccountPassword = Account.Encrypt(txtNewPassword.Text);
                acct.CreatedEmailAddress = txtEmailAddress.Text;
                int returnValue = acct.UpdatePassword();
                if (returnValue > 0)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Problem updating password in the database. Please contact the HelpDesk.')</script>");
                }
            }
        }
        private void MakeVisible(string theContent)
        {
            newPasswordCard.Visible = false;
            securityCard.Visible = false;
 
            switch (theContent)
            {
                case "newPasswordCard":
                    newPasswordCard.Visible = true;
                    break;
                case "securityCard":
                    securityCard.Visible = true;
                    break;
            }
        }
        protected void LoadForgotModalContent()
        {
            hmForgotPassModal.ButtonText = "Help";
            hmForgotPassModal.Content =
                "<h1>Forgot Password Help</h1> " +
                "<h3>Forgot Password</h3> " +
                "<p>Forgot Password calls the SecurityQuestions <b>SOAP WebMethod</b> to execute an SqlCommand for the \"Account_Security_Questions_SP\" stored procedure. The procedure returns an integer of the number of matching responses." +
                "<p>If you provide all correct responses you will be redirected to a page to enter a new password." +
                "<h3>New Password</h3> " +
                "<p>When the new and confirm passwords match, the Account.Encrypt() encrypts the new password, and passes it to the Account <b>Web Service Proxy</b> UpdatePassword() method to store it in the database.</p>";
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAccount.aspx");
        }
    }
}