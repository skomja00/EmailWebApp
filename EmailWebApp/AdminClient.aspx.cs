using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EmailLibrary.Model;
using System.Data;
using System.Data.SqlClient;
using Utilities;
using System.Text.RegularExpressions;

namespace EmailWebApp
{
    public partial class AdminClient : System.Web.UI.Page
    {
        Email email = new Email();
        Account acct = new Account();
        DBConnect objDB = new DBConnect();
        DataRow row;
        String[] gvAccountsDataKeyNames = new string[] { "AccountId" };
        String[] gvFlaggedEmailsDataKeyNames = new string[] { "EmailId", "EmailReceiptId" };

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadModalContent();
            if (!IsPostBack)
            {
                lblNavUserName.Text = Session["UserName"].ToString();
                ArrayList arrayListAvatar = (ArrayList)Application["arrayListAvatar"];

                //ChangeUrlHrefValue (string theAttribute, string theStringToSearch, string theNewStringValue)
                string newUrlHref = ChangeUrlHrefValue("href", svgAvatar.InnerText, "Images/" + arrayListAvatar[Convert.ToInt32(Session["Avatar"])] + "#Capa_1");
                svgAvatar.InnerHtml = newUrlHref;

                DataSet accountDS = acct.GetAccountsWithFlaggedEmail();
                gvAccounts.DataSource = accountDS;
                gvAccounts.DataKeyNames = gvAccountsDataKeyNames;
                gvAccounts.DataBind();
                for (int i = 0; i < gvAccounts.Rows.Count; i++)
                {
                    Image img = (Image)gvAccounts.Rows[i].Cells[1].FindControl("gvAccountsAvatar");
                    row = accountDS.Tables[0].Rows[i];
                    img.ImageUrl = "Images/" + arrayListAvatar[(int)row.ItemArray[2]];
                }
                gvFlaggedEmails.DataSource = email.GetEmailWithTag("Flag");
                gvFlaggedEmails.DataKeyNames = gvFlaggedEmailsDataKeyNames;
                gvFlaggedEmails.DataBind();
            }
        }
        private string ChangeUrlHrefValue(string theAttribute, string theStringToSearch, string theNewStringValue)
        {
            //Console.WriteLine(Regex.Replace())
            // Define a regular expression for repeated words.
            // ie. href="([^"]*)" matches everything between double-quotes
            return Regex.Replace(theStringToSearch,
                                theAttribute + "=" + "\"([^ \"]*)\"",
                                theAttribute + "=\"" + theNewStringValue + "\"",
                                RegexOptions.Compiled | RegexOptions.Multiline);
        }
        protected void btnNavLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            HttpCookie delCookie = Request.Cookies["CIS3342_Email"];
            //If RememberMe hasn't been selected the cookie will not exist.
            if (delCookie != null)
            {
                delCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(delCookie);
            }
            Response.Redirect("Login.aspx");
        }
        protected void btnBan_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvAccounts.Rows.Count; i++)
            {
                CheckBox account = (CheckBox)gvAccounts.Rows[i].Cells[0].FindControl("chkSelectEmail");
                if (account.Checked)
                {
                    acct.AccountId = Convert.ToInt32(gvAccounts.DataKeys[i].Values["AccountId"]);
                    acct.Active = "no";
                    acct.BanUnban();
                }
            }
            DataSet accountDS = acct.GetAccountsWithFlaggedEmail();
            gvAccounts.DataSource = accountDS;
            gvAccounts.DataKeyNames = gvAccountsDataKeyNames;
            gvAccounts.DataBind();
            ArrayList arrayListAvatar = (ArrayList)Application["arrayListAvatar"];
            for (int i = 0; i < gvAccounts.Rows.Count; i++)
            {
                Image img = (Image)gvAccounts.Rows[i].Cells[1].FindControl("gvAccountsAvatar");
                row = accountDS.Tables[0].Rows[i];
                img.ImageUrl = "Images/" + arrayListAvatar[(int)row.ItemArray[2]];
            }
        }

        protected void btnUnBan_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvAccounts.Rows.Count; i++)
            {
                CheckBox account = (CheckBox)gvAccounts.Rows[i].Cells[0].FindControl("chkSelectEmail");
                if (account.Checked)
                {
                    acct.AccountId = Convert.ToInt32(gvAccounts.DataKeys[i].Values["AccountId"]);
                    acct.Active = "yes";
                    acct.BanUnban();
                }
            }
            DataSet accountDS = acct.GetAccountsWithFlaggedEmail();
            gvAccounts.DataSource = accountDS;
            gvAccounts.DataKeyNames = gvAccountsDataKeyNames;
            gvAccounts.DataBind();
            ArrayList arrayListAvatar = (ArrayList)Application["arrayListAvatar"];
            for (int i = 0; i < gvAccounts.Rows.Count; i++)
            {
                Image img = (Image)gvAccounts.Rows[i].Cells[1].FindControl("gvAccountsAvatar");
                row = accountDS.Tables[0].Rows[i];
                img.ImageUrl = "Images/" + arrayListAvatar[(int)row.ItemArray[2]];
            }
        }
        protected void chkSelectEmail_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void LoadModalContent()
        {
            hmAccountClientModal.ButtonText = "Help";
            hmAccountClientModal.Content =
                "<h1 class=\"text-primary\">Administrator Help...</h1> " +
                "<p>An administrator can select one or more accounts, and <b>Ban</b> or <b>UnBan</b> to prohibit users with flagged content. All email with banned content are listed in the \"Flagged Email List\".</p>" +
                "<p>The Administrator page uses a combination of SOAP and WebAPI.</p>" +
                "<h3>Account List</h3>" +
                "<p>The SOAP WebService Account class has a GetAccountsWithFlaggedEmail WebMethod that provides the list all accounts with flagged email.</p>" +
                "<h3>Flagged Email List</h3>" +
                "<p>The list of flagged email are selected with an HTTP GET for the WebAPI Email controller using the URL and QueryString 'api/Email/GetEmailWithTag?tagName='Flag'. The Get_Email_With_Tag_SP <b>stored procedure</b> returns a DataSet.</p>";
        }
    }
}