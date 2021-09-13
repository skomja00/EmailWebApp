using System;
using System.Web;
using System.Data;
using Utilities;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using EmailLibrary.Model;


namespace EmailWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        DBConnect objDB = new DBConnect();
        HttpCookie objCookie;
        Account account = new Account();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadModalContent();

            //Response.Cookies.Remove("CIS3342_Email");
            if (!IsPostBack)
            {
                objCookie = Request.Cookies["CIS3342_Email"];
                if (objCookie != null)
                {
                    Byte[] accountPassword = Convert.FromBase64String(objCookie["AccountPassword"]);
                    LogIn(objCookie["CreatedEmailAddress"], accountPassword);
                    switch (Session["AccountRoleType"].ToString())
                    {
                        case "User":
                            Response.Redirect("AccountClient.aspx");
                            break;
                        case "Administrator":
                            Response.Redirect("AdminClient.aspx");
                            break;
                    }
                }
            }
        }
        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAcct.aspx");
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != null && txtPass.Text != null)
            {
                Byte[] accountPassword = Account.Encrypt(txtPass.Text);
                LogIn(txtEmail.Text, accountPassword);
            }
        }
        /// <summary>
        /// Call Account.LogIn() to verify the login credentials.
        /// If the account is banned/inactive alert the user. 
        /// Otherwise store the encrypted credentials in a cookie when remember me is checked.
        /// Redirect to either the Account or Admin pages based on the AccountRoleType
        /// </summary>
        private void LogIn(string theCreatedEmailAddress, Byte[] theAccountPassword)
        {
           account.LogIn(theCreatedEmailAddress, theAccountPassword);
           if (account.AccountId > 0)
            {
                if (account.Active == "no")
                {
                    Response.Write("<script>alert('Your account is inactive. Please contact the administrator.')</script>");
                }
                else
                {
                    UpdateSession();
                    if (chkRemember.Checked)
                    {
                        CreateCookie(account.CreatedEmailAddress, theAccountPassword);
                    }
                    switch (account.AccountRoleType)
                    {
                        case "User":
                            Response.Redirect("AccountClient.aspx");
                            break;
                        case "Administrator":
                            Response.Redirect("AdminClient.aspx");
                            break;
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Login failed. Please check your credentials.')</script>");
            }
        }
        /// <summary>
        /// Store the logged in user Account object in the Session to be available for any other page
        /// </summary>
        private void UpdateSession ()
        {
            Session["AccountId"] = account.AccountId;
            Session["UserName"] = account.UserName;
            Session["UserAddress"] = account.UserAddress;
            Session["PhoneNumber"] = account.PhoneNumber;
            Session["CreatedEmailAddress"] = account.CreatedEmailAddress;
            Session["ContactEmailAddress"] = account.ContactEmailAddress;
            Session["Avatar"] = account.Avatar;
            Session["Active"] = account.Active;
            Session["DateTimeStamp"] = account.DateTimeStamp;
            Session["AccountRoleType"] = account.AccountRoleType;
        }
        /// <summary>
        /// Write the encrypted password to a cookie if remember me is checked. 
        /// The user will either login manually or the cookie will be used to login.
        /// </summary>
        /// <param name="theCreatedEmailAddress"></param>
        /// <param name="theAccountPassword"></param>
        private void CreateCookie(string theCreatedEmailAddress, Byte[] theAccountPassword)
        {
            HttpCookie myCookie = new HttpCookie("CIS3342_Email");
            myCookie.Values["Name"] = "CIS3342_Email";
            myCookie.Values["CreatedEmailAddress"] = theCreatedEmailAddress;
            myCookie.Values["AccountPassword"] = Convert.ToBase64String(theAccountPassword);
            DateTime expires = new DateTime();
            expires = DateTime.Now.AddMonths(1);
            myCookie.Expires = expires;
            Response.Cookies.Add(myCookie);
        }
        protected void LoadModalContent()
        {
            hmLoginModal.ButtonText = "About";
            hmLoginModal.Content =
                "<h1 class=\"text-primary\">About Email WebApp...</h1> " +
                "<b>Email</b> is one of the most widely used features of the Internet. The <b>Email WebApp</b> allows sending and receiving messages with anyone registered with the App. The App provides a system administrator the ability to ban users whose email has been flagged. " +
                "Take a moment to create an account, login and explore the options. Be sure to click \"Help/About\" to see more .NET features on the other pages.</p>" +
                "<p>The Email WebApp implements many .NET technologies. " +
                "For instance, the content you are reading is a Modal <b>Custom Control</b> used throughout the web app. The modal uses <b>UpdatePanels</b> with <b>AJAX</b> Javascript provided by a ScriptManager. The Partial Page updates prevent a Full Page Postback closing the modal due to the default CSS Styling \"display: none\".</p>" +
                "<p>CSS styling via the Content Delivery Network (CDN) is built on the <i class=\"bi bi-bootstrap-fill\" style=\"color: cornflowerblue;\"></i> <b>Bootstrap</b> Framework with some <b>FlexBox</b></p>" +
                "<h3>EmailWebApp is a 4 part solution</h3>" +
                "<ol>" +
                "<li>The <b>EmailCoreWebAPI</b> .NET Core project provides <b>RESTful</b> services for Email, EmailReceipt, Tag, etc datatypes</li>" +
                "<li>The <b>EmailLibrary</b> .NET Framework project is the collection of the datatype properties and methods used by the WebApp</li>" +
                "<li>The <b>EmailSoapWebService</b> .NET Framework project serves all necessary <b>WebMethods</b> for the Account datatype</li>" +
                "<li>The <b>EmailWebApp</b> .NET Framework dynamic web pages allow an email user to interact with the web app</li>" +
                "</ol>" +
                "<h3>LogIn Help</h3> " +
                "<p><img src=Images/RememberMe.png />Check this to store your encrypted password in a <b>cookie</b> to automatically log into future sessions. Otherwise you have to type your credentials. Note: Logout <b>deletes the cookie</b> by setting the Expires property to a past date then adding the expired cookie to the Response.Cookies collection.<p>" +
                "<p><img src=Images/ForgotPassword.png />Takes you to a page to answer your security questions. If all responses are correct you can enter a new password.</p>" +
                "<h3>LogIn uses a SOAP WebService</h3> " +
                "<p>The Account.asmx <b>WebService</b> facilitates datalayer access for all Account datatype related activity.</p> " +
                "<p>The WebService provides 6 <b>WebMethods</b>:</p> " +
                "<ul> " +
                "    <li>The LogIn() method calls the \"Account_Login_SP\" <b>stored procedure</b> and returns a instance of Account if the given credentials are correct.</li>" +
                "    <li>The SecurityQuestions() method executes the \"Account_Security_Questions_SP\" <b>stored procedure</b> and returns an Integer of the number of matching responses.</li> " +
                "    <li>The UpdatePassword() method calls the \"Account_Update_Password_SP\" <b>stored procedure</b> which returns an Integer indicating the number of successfully updated rows.</li> " +
                "    <li>The CreateAccount() method calls the \"Account_Insert_SP\" <b>stored procedure</b> which returns an Integer indicating the number of rows affected by the insert.</li> " +
                "    <li>The BanUnBan() method calls the \"Account_Active_Update_SP\" <b>stored procedure</b> which returns an Integer indicating the number of rows affected by the update.</li> " +
                "    <li>The GetAccountsWithFlaggedEmail() method calls the \"Get_Accounts_With_Flagged_Email_SP\" <b>stored procedure</b> which returns a DataSet of Accounts with flagged email.</li> " +
                "</ul> " +
                "<p><a target=\"_blank\" href=\"Images/EmailWebApp-ERD.png\" type=\"application/pdf\">Here</a> is a basic database ERD and <a target=\"_blank\" href=\"PDF/email-db-install.pdf\" type=\"application/pdf\"> here</a> are the database scripts including tables, constraints, stored procedures, sample data, etc.";
        }
        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }

        protected void btnCreateAccount_Click1(object sender, EventArgs e)
        {
            Response.Redirect("CreateAccount.aspx");
        }
    }
}