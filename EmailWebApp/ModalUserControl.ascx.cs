using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TermProject
{
    public partial class ModalUserControl : System.Web.UI.UserControl
    {
        private string buttonText;
        private string content;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnModalUserControl.Text = buttonText;
            contentDiv.InnerHtml = content;
        }
        /// <summary>
        /// Toggle the Modal display
        /// </summary>
        protected void btnToggleVisibility(object sender, EventArgs e)
        {
            if (ModalHelp.Attributes["class"] == "modal-none")
                ModalHelp.Attributes["class"] = "modal-block";
            else
                ModalHelp.Attributes["class"] = "modal-none";
        }
        public string ButtonText
        {
            set { buttonText = value; }
        }
        public string Content
        {
            set { content = value; }
        }
    }
}