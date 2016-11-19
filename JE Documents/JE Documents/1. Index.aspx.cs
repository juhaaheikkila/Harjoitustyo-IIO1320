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
        //Label mpUsername;
        JEuser muser = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = System.Configuration.ConfigurationManager.AppSettings["Username"];
                string userDatafile = System.Configuration.ConfigurationManager.AppSettings["UserDataFile"];
                Label mpTitle = (Label)Page.Master.FindControl("lblTitle");
                Label lblUserID = (Label)Page.Master.FindControl("lblUserID");
                Label lblUsername = (Label)Page.Master.FindControl("lblUsername");
                Label lblUserDepartment = (Label)Page.Master.FindControl("lblUserDepartment");
                Label lblUserRoles = (Label)Page.Master.FindControl("lblUserRoles");
                lblUserID.Text = username;
                muser = new JEuser(lblUserID.Text, Server.MapPath(userDatafile));
                lblUserID.Text = muser.id;
                lblUsername.Text = muser.username;
                lblUserDepartment.Text = muser.department;
                lblUserRoles.Text = string.Join(", ", muser.roles);
                mpTitle.Text = "Main page";
                SetButtons(muser);
            }
        }

        protected void btnUsers_Click(object sender, EventArgs e)
        {

        }

        #region METHODS
        protected void SetButtons(JEuser rUser)
        {
            //user admin
            hlUsers.Visible = muser.isUserRoleOn("admin");
            hlCompanies.Visible = muser.isUserRoleOn("admin");
        }
        #endregion

        protected void btnUsers_Click1(object sender, EventArgs e)
        {

        }

        protected void btnJEDocuments_Click(object sender, EventArgs e)
        {

        }
    }
}