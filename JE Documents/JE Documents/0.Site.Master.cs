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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //setting variables
                if (CommonCodes.gLog == null)
                {
                    CommonCodes.initialize();
                }
                if (CommonCodes.gUser.username != null)
                {
                    lblUserName.Text = CommonCodes.gUser.username;
                    lblUserDepartment.Text = CommonCodes.gUser.department;
                    lblUserRoles.Text = "[" + string.Join("], [", CommonCodes.gUser.roles) + "]";
                    SetButtons(CommonCodes.gUser);
                }
                else
                {
                    lblMessage.Text = "Username " + CommonCodes.gUsername + " missing!";
                   
                }

            }
        }

        protected void SetButtons(JEuser rUser)
        {
            //visible for admin user
            liSettings.Visible = CommonCodes.gUser.isUserRoleOn("admin");
            hlUsers.Visible = CommonCodes.gUser.isUserRoleOn("admin");
            hlCompanies.Visible = CommonCodes.gUser.isUserRoleOn("admin");
            hlVATCodes.Visible = CommonCodes.gUser.isUserRoleOn("admin");
            hlLogs.Visible = CommonCodes.gUser.isUserRoleOn("admin");
        }

    }



}

