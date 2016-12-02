using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JE_Documents.Data;

namespace JE_Documents
{

    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
                Label mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpPageTitle.Text = "Main page";
                mpMessage.Text = "...";

                
            }
        }

    }
}