using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using JE_Documents.Data;

namespace JE_Documents
{
    public partial class _2_Users : System.Web.UI.Page

    {

        Label mpMessage;
        Label mpPageTitle;
        static string userDataFile;
        static string strQueryKey = "UserName";

        protected void Page_Load(object sender, EventArgs e)
        {
            mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
            mpPageTitle.Text = "Users page";
            mpMessage = (Label)Page.Master.FindControl("lblMessage");
            mpMessage.Text = "...";
            userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);

            if (!IsPostBack)
            {
                FillControls();
            }

            hideEditForm();
            

            if (Request.QueryString[strQueryKey] != null)
            {
                string strUsername = Request.QueryString[strQueryKey];
                if (!"".Equals(strUsername))
                {
                    mpPageTitle.Text = "Users page / user: " + strUsername;
                    JEuser user = new JEuser(strUsername, userDataFile, "username");
                    getUserData(user);
                }
            }
        }

        protected void btnGetUsers_Click(object sender, EventArgs e)
        {
            updateXML();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }

        //NEW
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            int intCounter;
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(userDataFile);
            if (xDoc != null)
            {
                intCounter = xDoc.Root.Elements().Count() + 1;
            }
            else
            {
                intCounter = 0;
            }
            txtUserID.Text = Convert.ToString(intCounter);
            txtUserID.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtFirstname.Text = string.Empty;
            txtLastname.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            txtEmail.Text = string.Empty;


            displayEditForm("New user", false, true, true, true, false);
        }

        //SAVE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(userDataFile);

            if (xdoc != null)
            {
                //search for user
                var xuser = xdoc.Root.Descendants("user").Where(x => x.Element("id").Value == txtUserID.Text).SingleOrDefault();
                if (xuser != null)
                {
                    xuser.Remove();
                }

                XElement user = new XElement("user");
                XElement userroles = new XElement("roles");
                user.Add(new XElement("id", txtUserID.Text));
                user.Add(new XElement("username", txtUsername.Text));
                user.Add(new XElement("firstname", txtFirstname.Text));
                user.Add(new XElement("lastname", txtLastname.Text));
                user.Add(new XElement("department", txtDepartment.Text));
                user.Add(new XElement("email", txtEmail.Text));
                //user.Add(new XElement("roles", ""));
                foreach (ListItem rooli in chkRoles.Items)
                {
                    if (rooli.Selected)
                    {
                        userroles.Add(new XElement("role", rooli.Value));
                    }
                }
                user.Add(userroles);
                xdoc.Element("users").Add(user);

            }

            xdoc.Save(userDataFile);
            updateXML();
            hideEditForm();

            //updating page
            //Response.Redirect(Request.RawUrl);
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(userDataFile);
            if (xdoc != null)
            {
                //search for user
                var xuser = xdoc.Root.Descendants("user").Where(x => x.Element("id").Value == txtUserID.Text).SingleOrDefault();
                if (xuser != null)
                {
                    xuser.Remove();
                    xdoc.Save(userDataFile);
                }
                
                updateXML();
                hideEditForm();
            }
        }

        //CANCED
        protected void btnCancel_Click(object sended, EventArgs e)
        {
            hideEditForm();
        }


        #region METHODS

        protected void getUserData(JEuser rJEUser)
        {
            if (rJEUser != null)
            {
                
                displayEditForm(rJEUser.id + ": " + rJEUser.username, true, true, true, true, true);
                
                txtUserID.Text = rJEUser.id;
                txtUsername.Text = rJEUser.username;
                txtFirstname.Text = rJEUser.firstname;
                txtLastname.Text = rJEUser.lastname;
                txtDepartment.Text = rJEUser.department;
                txtEmail.Text = rJEUser.email;
                //roles
                foreach (ListItem chk in chkRoles.Items)
                {
                    chk.Selected = false;
                    foreach (string role in rJEUser.roles)
                    {
                        if (chk.Value.Equals(role))
                        {
                            chk.Selected = true;
                        }
                    }
                }
            }
        }

        protected void hideEditForm()
        {

            NewUser.Visible = false;
            UserList.Visible = true;
            updateXML();

        }

        protected void displayEditForm(string strTitle, bool blnShowSelectUser, bool blnShowUserData, bool blnShowSave, bool blnShowCancel, bool blnShowDelete)
        {
            liUserData.Visible = blnShowUserData;
            titleUser.InnerText = strTitle;
            NewUser.Visible = true;
            UserList.Visible = false;
            FillControls();
            btnSave.Visible = blnShowSave;
            btnCancel.Visible = blnShowCancel;
            btnDelete.Visible = blnShowDelete;
        }

        protected void updateXML()
        {
            //List all users
            try
            {
                XDocument xDoc = new XDocument();
                int userCount = 0;

                xDoc = XDocument.Load(userDataFile);
                var newxDoc = new XElement("User", xDoc.Root
                    .Elements()
                    .OrderBy(x => (int)x.Element("id"))
                    );

                ltTableHead.Text = "<tr><th>Id</th><th>Username</th><th>Firstname</th><th>Lastname</th><th>Department</th><th>email</th><th>roles</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                //if (xDoc != null)
                if (newxDoc != null)
                {
                    foreach (XElement xuser in newxDoc.Descendants("user"))
                    {
                        string xuserid = xuser.Element("id").Value;
                        string xusername = xuser.Element("username").Value;
                        string xfirstname = xuser.Element("firstname").Value;
                        string xlastname = xuser.Element("lastname").Value;
                        string xuserdepartment = xuser.Element("department").Value;
                        string xuseremail = xuser.Element("email").Value;
                        string xuserroles = string.Join("<br />", xuser.Element("roles").Descendants().Distinct());
                        ltTableData.Text += string.Format("<tr class='listRow'><td><a href='{7}?{8}={1}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", xuserid, xusername, xfirstname, xlastname, xuserdepartment, xuseremail, xuserroles, strEditUrl, strQueryKey);
                        userCount += 1;
                    }
                    int childrenCount = xDoc.Root.Elements().Count();
                    mpMessage.Text = string.Format("User count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text += "<br />" + ex.Message;
            }
        }

        protected void FillControls()
        {
            //populating user id dropdownlist
            //user roles
            string userroles = System.Configuration.ConfigurationManager.AppSettings["Userroles"];
            string[] splittedroles = userroles.Split(',');
            chkRoles.Items.Clear();
            foreach (string role in splittedroles)
            {
                chkRoles.Items.Add(new ListItem(role, role));
            }

            #endregion
        }
    }
}