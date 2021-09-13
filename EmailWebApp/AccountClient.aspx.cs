using EmailLibrary.Model;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Utilities;

namespace EmailWebApp
{
    /// <summary>
    /// 
    /// Overall the EmailWebApp provides the 2 types of accounts;
    /// 1. An Account type provides access to email functions; send/recv email, organize email, etc...
    /// 2. An Administrator type makes sure content is safe from scams, suspected illegality, etc...
    /// 
    /// The AccountClient page facilitates access to email. 
    ///
    /// Page layout includes the following;
    ///
    /// - NavBar at the top
    /// - a SideBar containing LinkButtons with OnClick events to select email organized by Tags
    /// - MainContent switches visibility of 4 different types of content 
    ///     - EmailList content including a row of controls to interact with emails
    ///     - CreateEmail to create and and send an email
    ///     - SelectEmail to view the detail of a selected email
    ///     - SearchEamil lists results of a text search
    /// 
    /// The state of several data bound controls is maintained by .NET including...
    /// GridView gvEmail presents email content and PostBack events to interact with email items
    /// DropDownList ddlMoveTo updates the EmailReceipt.TagId in the SelectedIndexChanged event
    /// DropDownList ddlCustomTags is a side-bar feature to view email of a given user defined TagName
    /// 
    /// The state of dynamic content is maintained in the server-side code behind such as...
    /// A DropDownList for email sent to multiple accounts. This DDL must be re-created every roundtrip
    /// 
    /// </summary>
    public partial class AccountClient : System.Web.UI.Page
    {
        DBConnect objDB = new DBConnect();
        SqlCommand objSqlCmd = new SqlCommand();
        DataSet objDS = new DataSet();
        Email email = new Email();
        String[] gvEmailDataKeyNames = new string[] { "AccountId", "EmailId", "AvatarSend", "RecvEmailList", "RecvEmailCount"};
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadModalContent();
            if (!IsPostBack)
            {
                MakeVisible("emailList");
                txtSearch.Text = "";
                txtAddTag.Text = "";

                lblNavUserName.Text = Session["UserName"].ToString();
                ArrayList arrayListAvatar = (ArrayList) Application["arrayListAvatar"];

                //ChangeUrlHrefValue (string theAttribute, string theStringToSearch, string theNewStringValue)
                string newUrlHref = ChangeUrlHrefValue("href", 
                                                        svgAvatar.InnerText, 
                                                        "Images/" + arrayListAvatar[Convert.ToInt32(Session["Avatar"])] + "#Capa_1");
                svgAvatar.InnerHtml = newUrlHref;

                HighlightTag("Inbox");
                objDS = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Inbox");
                gvEmail.DataSource = objDS;
                gvEmail.DataKeyNames = gvEmailDataKeyNames;
                gvEmail.DataBind();
                ddlMoveTo.DataSource = Tags.GetTagsDataSet(Session["CreatedEmailAddress"].ToString(), "All");
                ddlMoveTo.DataTextField = "TagName";
                ddlMoveTo.DataValueField = "TagName";
                ddlMoveTo.DataBind();
                ddlMoveTo.Items.Insert(0, new ListItem("Move To", "Move"));
                ddlCustomTags.DataSource = Tags.GetTagsDataSet(Session["CreatedEmailAddress"].ToString(), "Custom");
                ddlCustomTags.DataTextField = "TagName";
                ddlCustomTags.DataValueField = "TagName";
                ddlCustomTags.DataBind();
                ddlCustomTags.Items.Insert(0, new ListItem("Other folders", "Custom"));
            }
        }
        private string ChangeUrlHrefValue(string theAttribute, string theStringToSearch, string theNewStringValue)
        {
            // Define a regular expression matching everything between double-quotes
            return Regex.Replace(theStringToSearch,
                                theAttribute + "=" + "\"([^ \"]*)\"",
                                theAttribute + "=\"" + theNewStringValue + "\"",
                                RegexOptions.Compiled | RegexOptions.Multiline);
        }   
        private void HighlightTag(string theTagName = null)
        {
            MakeVisible("emailList");

            btnInbox.CssClass = btnInbox.CssClass.Replace("font-weight-bold", "font-weight-normal");
            btnInbox.Text = "Inbox";

            btnSent.CssClass = btnSent.CssClass.Replace("font-weight-bold", "font-weight-normal");
            btnSent.Text = "Sent";

            btnFlag.CssClass = btnFlag.CssClass.Replace("font-weight-bold", "font-weight-normal");
            btnFlag.Text = "Flag";

            btnJunk.CssClass = btnJunk.CssClass.Replace("font-weight-bold", "font-weight-normal");
            btnJunk.Text = "Junk";

            btnTrash.CssClass = btnTrash.CssClass.Replace("font-weight-bold", "font-weight-normal");
            btnTrash.Text = "Trash";

            switch (theTagName)
            {
                case"Inbox":
                    btnInbox.CssClass = btnInbox.CssClass.Replace("font-weight-normal", "font-weight-bold");
                    // right/left angle quotes 
                    btnInbox.Text = "&raquo;&nbsp;Inbox&nbsp;&laquo;";
                    break;
                case "Sent":
                    btnSent.CssClass = btnSent.CssClass.Replace("font-weight-normal", "font-weight-bold");
                    btnSent.Text = "&raquo;&nbsp;Sent&nbsp;&laquo;";
                    break;
                case "Flag":
                    btnFlag.CssClass = btnFlag.CssClass.Replace("font-weight-normal", "font-weight-bold");
                    btnFlag.Text = "&raquo;&nbsp;Flag&nbsp;&laquo;";
                    break;
                case "Junk":
                    btnJunk.CssClass = btnFlag.CssClass.Replace("font-weight-normal", "font-weight-bold");
                    btnJunk.Text = "&raquo;&nbsp;Junk&nbsp;&laquo;";
                    break;
                case "Trash":
                    btnTrash.CssClass = btnTrash.CssClass.Replace("font-weight-normal", "font-weight-bold");
                    btnTrash.Text = "&raquo;&nbsp;Trash&nbsp;&laquo;";
                    break;
                default:
                    break;
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            txtSendEmailAddr.Text = Session["CreatedEmailAddress"].ToString();
            txtRecvEmailAddr.Focus();

            MakeVisible("createEmail");
        }

        protected void btnInbox_Click(object sender, EventArgs e)
        {
            HighlightTag("Inbox");
            ddlCustomTags.SelectedIndex = 0;
            ddlMoveTo.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Inbox");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void btnSent_Click(object sender, EventArgs e)
        {
            HighlightTag("Sent");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetSentEmail(Session["CreatedEmailAddress"].ToString());
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void btnFlag_Click(object sender, EventArgs e)
        {
            HighlightTag("Flag");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Flag");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void btnJunk_Click(object sender, EventArgs e)
        {
            HighlightTag("Junk");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Junk");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void btnTrash_Click(object sender, EventArgs e)
        {
            HighlightTag("Trash");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Trash");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void ddlCustomTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            HighlightTag(ddlCustomTags.SelectedItem.Text);
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), ddlCustomTags.SelectedItem.Text);
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                // The constructor Email(string email, string tagname) creates a local DataSet instance
                // If you pass a wildcard "%" TagName it matches all TagsNames (ie Inbox, Flag, Sent, etc.)  
                email.GetEmail(Session["CreatedEmailAddress"].ToString(), "%");

                string searchTerm = txtSearch.Text.ToLower();

                String searchString = "UserName LIKE '%" + searchTerm + "%' " +
                           "or CreatedEmailAddress LIKE '%" + searchTerm + "%' " +
                           "or UserNameSend LIKE '%" + searchTerm + "%' " +
                           "or CreatedEmailAddressSend LIKE '%" + searchTerm + "%' " +
                           "or EmailSubject LIKE '%" + searchTerm + "%' " +
                           "or EmailBody LIKE '%" + searchTerm + "%' ";

                // Email.SearchEmail returns DataTable matching the search string
                // filter criteria (DataView RowFilter Syntax required).
                gvSearch.DataSource = email.SearchEmail(searchString);
                gvSearch.DataBind();

                txtSearch.Text = "";

                MakeVisible("searchEmail");
            }
            else
            {
                Response.Write("<script>alert('No search text provided. Please check your input.')</script>");
            }
        }
        protected void gvEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList arrayListAvatar = (ArrayList)Application["arrayListAvatar"];

            MakeVisible("selectEmail");

            txtSelectFrom.Text = gvEmail.SelectedRow.Cells[2].Text;
            txtSelectRecv.Text = gvEmail.DataKeys[gvEmail.SelectedIndex].Values["RecvEmailList"].ToString(); 
            int avatarIdx = Convert.ToInt32(gvEmail.DataKeys[gvEmail.SelectedIndex].Values["AvatarSend"]);
            imgSelectEmailAvatar.ImageUrl = "Images/" + arrayListAvatar[avatarIdx];            
            txtSelectSubject.Text = gvEmail.SelectedRow.Cells[4].Text;
            txtSelectEmail.Text = gvEmail.SelectedRow.Cells[5].Text;
            txtSelectDateTimeStamp.Text = gvEmail.SelectedRow.Cells[6].Text;
        }
        /// <summary>
        /// Add multiple email receipients to a DropDown list
        /// Otherwise add a label for single email receipts
        /// </summary>
        protected void gvEmail_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            MaintainRecvEmailRow(e.Row, e.Row.RowIndex);
        }
        protected void chkSelectEmail_CheckedChanged(object sender, EventArgs e)
        {
            MaintainRecvEmailDDL();
        }
        /// <summary>
        /// Maintain the dynamic content DropDown for multiple email receipients
        /// </summary>
        protected void MaintainRecvEmailDDL()
        {
            for (int i = 0; i < gvEmail.Rows.Count; i++)
            {
                MaintainRecvEmailRow(gvEmail.Rows[i], i);
            }
        }
        protected void MaintainRecvEmailRow (GridViewRow row, int rowNum)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                int recvEmailCount = Convert.ToInt32(gvEmail.DataKeys[rowNum].Values["RecvEmailCount"]);
                PlaceHolder ph = row.FindControl("RecvEmail") as PlaceHolder;
                if (recvEmailCount > 1)
                {
                    string recvEamilList = gvEmail.DataKeys[row.RowIndex].Values["RecvEmailList"].ToString();
                    DropDownList ddl = new DropDownList();
                    ddl.Style.Add("border", "none");
                    string[] addrs = recvEamilList.Split(';');
                    for (int j = 0; j < addrs.Count(); j++)
                    {
                        ListItem li = new ListItem();
                        li.Value = rowNum.ToString();
                        li.Text = addrs[j];
                        ddl.Items.Add(li);
                    }
                    ph.Controls.Add(ddl);
                }
                else
                {
                    string recvEamilList = gvEmail.DataKeys[row.RowIndex].Values["RecvEmailList"].ToString();
                    Label lbl = new Label();
                    lbl.Text = recvEamilList;
                    ph.Controls.Add(lbl);
                }
            }
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            Email email = new Email();
            email.SendAccount = txtSendEmailAddr.Text;
            email.RecvAccountList = txtRecvEmailAddr.Text;
            email.EmailSubject = txtEmailSubject.Text;
            email.EmailBody = txtEmailBody.Text;

            Boolean sendSuccessful = email.SendEmail();

            if (sendSuccessful)
            {
                MakeVisible("emailList");
            }
            else
            {
                Response.Write("<script>alert('Send Unsuccessful. Please check your input.')</script>");
            }
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
        protected void btnFlagEmail_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvEmail.Rows.Count; i++)
            {
                CheckBox emailIs = (CheckBox)gvEmail.Rows[i].Cells[0].FindControl("chkSelectEmail");
                if (emailIs.Checked)
                {
                    EmailReceipt.TagUpdate(Convert.ToInt32(gvEmail.DataKeys[i].Values["AccountId"]),
                                            Convert.ToInt32(gvEmail.DataKeys[i].Values["EmailId"]),
                                            "Flag");
                }
            }
            HighlightTag("Flag");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Flag");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }
        protected void btnUnFlagEmail_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvEmail.Rows.Count; i++)
            {
                CheckBox emailIs = (CheckBox)gvEmail.Rows[i].Cells[0].FindControl("chkSelectEmail");
                if (emailIs.Checked)
                {
                    EmailReceipt.TagUpdate(Convert.ToInt32(gvEmail.DataKeys[i].Values["AccountId"]),
                                            Convert.ToInt32(gvEmail.DataKeys[i].Values["EmailId"]),
                                            "Inbox");
                }
            }
            HighlightTag("Inbox");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Inbox");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }

        protected void btnAddTag_Click(object sender, EventArgs e)
        {
            if (txtAddTag.Text.Length > 0)
            {
                Tags tag = new Tags();
                tag.TagName = txtAddTag.Text;

                Boolean successful = tag.Add(Session["CreatedEmailAddress"].ToString());

                if (successful)
                {
                    ddlMoveTo.DataSource = Tags.GetTagsDataSet(Session["CreatedEmailAddress"].ToString(), "All");
                    ddlMoveTo.DataTextField = "TagName";
                    ddlMoveTo.DataValueField = "TagId";
                    ddlMoveTo.DataBind();
                    ddlMoveTo.Items.Insert(0, new ListItem("Move To", "Move"));
                    ddlMoveTo.SelectedIndex = 0;

                    txtAddTag.Text = "";
                    ddlCustomTags.DataSource = Tags.GetTagsDataSet(Session["CreatedEmailAddress"].ToString(), "Custom");
                    ddlCustomTags.DataTextField = "TagName";
                    ddlCustomTags.DataValueField = "TagId";
                    ddlCustomTags.DataBind();
                    ddlCustomTags.Items.Insert(0, new ListItem("Other folders", "Custom"));
                    ddlCustomTags.SelectedIndex = 0;
                    Response.Write("<script>alert('Folder created. Please click to continue.')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Could not create the tag. Please check your input.')</script>");
                }
            } 
            else
            {
                Response.Write("<script>alert('No folder name entered. Please check your input.')</script>");
            }
        }

        /// <summary>
        /// Update all Checked email with Tag selected in the MoveTo dropdown
        /// </summary>
        protected void ddlMoveTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gvEmail.Rows.Count; i++)
            {
                CheckBox emailIs = (CheckBox) gvEmail.Rows[i].Cells[0].FindControl("chkSelectEmail");
                if (emailIs.Checked)
                {
                    EmailReceipt.TagUpdate(Convert.ToInt32(gvEmail.DataKeys[i].Values["AccountId"]),
                                            Convert.ToInt32(gvEmail.DataKeys[i].Values["EmailId"]),
                                            ddlMoveTo.SelectedItem.ToString());
                }
            }
            // After moving to a folder, redirect the client to the Inbox.
            // If the client chooses to do so they can manually 
            // navigate to the folder to which they moved the email
            HighlightTag("Inbox");
            ddlMoveTo.SelectedIndex = 0;
            ddlCustomTags.SelectedIndex = 0;
            gvEmail.DataSource = email.GetEmail(Session["CreatedEmailAddress"].ToString(), "Inbox");
            gvEmail.DataKeyNames = gvEmailDataKeyNames;
            gvEmail.DataBind();
        }

        protected void btnSearchEmailBack_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            MakeVisible("emailList");
        }
        private void MakeVisible (string theContent)
        {
            emailList.Visible = false;
            createEmail.Visible = false;
            selectEmail.Visible = false;
            searchEmail.Visible = false;

            switch (theContent)
            {
                case "emailList":
                    emailList.Visible = true;
                    break;
                case "createEmail":
                    createEmail.Visible = true;
                    break;
                case "selectEmail":
                    selectEmail.Visible = true;
                    break;
                case "searchEmail":
                    searchEmail.Visible = true;
                    break;
            }
        }
        protected void LoadModalContent()
        {
            hmAccountClientModal.ButtonText = "Help";
            hmAccountClientModal.Content =
                "<h1 class=\"text-primary\">Email WebApp Help...</h1> " +
                "<p><b>Flag</b> email if you receive inappropriate content, and a system administrator will consider banning that user whose email is flagged.</p>" +
                "<p><b>Tags</b> help organize your emails, like Inbox or Junk. You won't see all emails at once. Add a custom tag by typing in the box, and clicking \"Add Folder\" to add the tag. Custom tags can be found in the \"Other\" dropdown list.</p>" +
                "<p><b>Search</b> your email by entering filter criteria. Search includes UserName, EmailAddress, EmailSubject, and EmailBody.</p>" +
                "<p><b>Create</b> email for one or more recipients separated by the semi-colon symbol.</p>" +
                "<h3><b>State</b></h3>" +
                "<p>The <b>ViewState</b> of data bound controls is maintained by .NET such as the <b>DropDownLists</b> \"Move To\" and \"Other folders\". ASP.NET does not maintain the state of dynamic content. It must be handled by server-side code. For example the " +
                "\"Recv\" column for email sent to multiple accounts must be created for every roundtrip. A <b>PlaceHolder</b> in the ASPX markup holds the place for a DropDownList when there is more than one receipient of an email.</p> " +
                "<h3><b>RESTful WebAPIs</b></h3> " +
                "<p>.NET Core WebAPIs provide datalayer access for all Email related activity.</p>" +
                "<p>Two <b>WebAPI Controllers</b> are available:</p> " +
                "<ul> " +
                "    <li>Tags Controller: POST and GET actions for Tags datatype.</li> " +
                "    <li>Email Controller: POST, GET and PUT actions for Email and EmailRecept datatypes.</li>" +
                "</ul> " +
                "<p>PascalCase naming for Json objects is configured in Startup.cs using the Json options DefaultContractResolver.</p>" +
                "<p>Json and .NET objects are serialized and deserialized using <b><i class=\"fa fa-rocket\"></i> Json.NET - Newtonsoft</b></p>";
        }
    }
}