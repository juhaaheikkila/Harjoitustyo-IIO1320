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
        static string strPagetitle = "Main page";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonCodes.gLog == null)
                {
                    CommonCodes.initialize();
                    //CommonCodes.gLog = new JELogHelper(CommonCodes.gLogDatafile, CommonCodes.gUserDatafile, CommonCodes.gUsername);
                }
                if (CommonCodes.gUser.username == null)
                {
                    Response.Redirect("2.Users.aspx");
                }
                Label mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
                Label mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpPageTitle.Text = "Main page";
                mpMessage.Text = "...";
               
                CommonCodes.gLog.logEvent("Opening " + strPagetitle);

            }
        }

    }
}