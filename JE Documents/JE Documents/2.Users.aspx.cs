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
        static string userDataFile;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DropDownList lstLocations;

                Label mpTitle = (Label)Page.Master.FindControl("lblTitle");
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpTitle.Text = "User page";
                mpMessage.Text = "...";
                userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);
                string userroles = System.Configuration.ConfigurationManager.AppSettings["Userroles"];
                string[] splittedroles = userroles.Split(',');
                foreach (string role in splittedroles)
                {
                    chkRoles.Items.Add(new ListItem(role, role));
                }
                updateXML();
            }
        }

        protected void updateXML()
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                XmlDocument doc = new XmlDocument();
                int userCount = 0;

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(userDataFile);
                if (xDoc != null)
                {
                    
                    ltUsers.Text += "<tr class='w3-row listTitle'><td>User id</td><td>Username</td><td>Department</td><td>email</td><td>roles</td></tr>";
                    IEnumerable<XElement> rows = xDoc.Root.Descendants("user");
                    foreach (XElement xuser in xDoc.Root.Descendants("user"))
                    {
                        string xuserid = xuser.Element("id").Value;
                        string xusername = xuser.Element("name").Value;
                        string xuserdepartment = xuser.Element("department").Value;
                        string xuseremail = xuser.Element("email").Value;
                        string xuserroles= string.Join("<br />", xuser.Element("roles").Descendants().Distinct());
//                        List<string> xuserRoles = new List<string>();
//                        foreach (XElement xrole in xuser.Descendants("role"))
//                        {
//                            xuserRoles.Add(xrole.Value);
//                        }
                        ltUsers.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", xuserid, xusername, xuserdepartment, xuseremail, xuserroles);
                        userCount += 1;
                    }
                    lblAllUsersXML.Text = string.Format("User count {0} pcs", userCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text += "<br />" + ex.Message;
            }
        }

        protected void btnGetUsers_Click(object sender, EventArgs e)
        {

            updateXML();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            txtUserID.Visible = true;
            titleUser.InnerText = "New user";
            NewUser.Visible = true;
            UserList.Visible = false;
            FillControls();
        }

        protected void btnCustomerCancel_Click(object sended, EventArgs e)
        {
            NewUser.Visible = false;
            UserList.Visible = true;
            txtUserID.Visible = false;
            ddlUser.Visible = false;
        }

        protected void btnCloseUser_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {

            JEuser user = new JEuser(ddlUser.SelectedValue, userDataFile);
            //CRUDia varten
            titleUser.InnerText = user.username + ": " + user.fullname;
            txtUserID.Text = user.username;
            txtUsername.Text = user.fullname;
            txtDepartment.Text = user.department;
            txtEmail.Text = user.email;
            //roles
            foreach (ListItem chk in chkRoles.Items)
            {
                chk.Selected = false;
                foreach (string role in user.roles)
                {
                    if (chk.Value.Equals(role))
                    {
                        chk.Selected = true;
                    }
                }
            }

            btnUserDelete.Visible = true;

        }


        protected void btnCustomerSave_Click(object sender, EventArgs e)
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
                user.Add(new XElement("name", txtUsername.Text));
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

            //updating page
            Response.Redirect(Request.RawUrl);
        }

//EDIT
        protected void btnModify_Click(object sender, EventArgs e)
        {
            txtUserID.Visible = false;
            
            ddlUser.Visible = true;
            NewUser.Visible = true;
            UserList.Visible = false;
            titleUser.InnerText = "Select user to edit";
            FillControls();
        }


        #region METHODS

        protected void FillControls2()
        {
         
            DataSet ds = new DataSet();
            // you can bind your data with xml file or you can fetch data from database to BLL to your              //dataset ds.
            ds.ReadXml(userDataFile);
            ddlUser.DataSource = ds.Tables[0];
            ddlUser.DataTextField = "name";
            ddlUser.DataValueField = "id";
            ddlUser.DataBind();

        

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

            //user list
            XDocument xdoc = XDocument.Load(userDataFile);
            if (xdoc != null)
            {
                //updating selection list
                ddlUser.Items.Clear();
//                foreach (XElement xuser in xdoc.Root.Descendants("user"))
//                {
//                    string xuserid = xuser.Element("id").Value;
//                    string xusername = xuser.Element("name").Value;
//                    ddlUser.Items.Add(new ListItem(xuserid + ": " + xusername, xuserid));
//                }

                DataSet ds = new DataSet();
                // you can bind your data with xml file or you can fetch data from database to BLL to your              //dataset ds.
                ds.ReadXml(userDataFile);
                ddlUser.DataSource = ds.Tables[0];
                ddlUser.DataTextField = "id";
                ddlUser.DataValueField = "id";
                ddlUser.DataBind();
                //empty element at selection list
                ddlUser.Items.Insert(0, string.Empty);
                
                //for CRUD
                ddlUser.SelectedIndex = 0;
                txtUserID.Text = string.Empty;
                txtUsername.Text = string.Empty;
                txtDepartment.Text = string.Empty;
                txtEmail.Text = string.Empty;
                //resetting roles to not  selected
                foreach (ListItem rooli in chkRoles.Items)
                {
                    rooli.Selected = false;
                }

            }

            #endregion
        }

//DELETION
        protected void btnUserDelete_Click(object sender, EventArgs e)
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
                xdoc.Save(userDataFile);

                btnUserDelete.Visible = false;

                //update page
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}