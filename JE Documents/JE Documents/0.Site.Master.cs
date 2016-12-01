using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using JE_Documents.Data;

namespace JE_Documents
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        //public static JE_Documents.Data.user gUser;
        JEuser muser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = System.Configuration.ConfigurationManager.AppSettings["Username"];
                string userDatafile = System.Configuration.ConfigurationManager.AppSettings["UserDataFile"];
                string logDatafile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
                muser = new JEuser(username, Server.MapPath(userDatafile), "username");
                lblUserName.Text = muser.username;
                lblUserDepartment.Text = muser.department;
                lblUserRoles.Text = "[" + string.Join("], [", muser.roles) + "]";
                SetButtons(muser);
            }
        }


        protected void SetButtons(JEuser rUser)
        {
            //user admin
            liSettings.Visible = muser.isUserRoleOn("admin");
            hlUsers.Visible = muser.isUserRoleOn("admin");
            hlCompanies.Visible = muser.isUserRoleOn("admin");
        }

    }



}

