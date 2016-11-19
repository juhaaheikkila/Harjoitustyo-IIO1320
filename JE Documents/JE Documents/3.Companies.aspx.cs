using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace JE_Documents
{
    public partial class _02_Company : System.Web.UI.Page
    {
        Label mpMessage;
        static string companyDataFile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DropDownList lstLocations;

                Label mpTitle = (Label)Page.Master.FindControl("lblTitle");
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpTitle.Text = "User page";
                mpMessage.Text = "...";
                companyDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CompanyDataFile"]);

                updateXML();
            }
        }

        protected void updateXML()
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                int userCount = 0;

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(companyDataFile);
                if (xDoc != null)
                {

                    ltCompanies.Text += "<tr class='w3-row listTitle'><td>Company id</td><td>Name</td><td>Address</td><td>Departments</td><td>approvers</td><td>home Currency</td></tr>";
                    foreach (XElement xcompany in xDoc.Root.Descendants("company"))
                    {
                        string xcompanyid = xcompany.Element("id").Value;
                        string xcompanyname = xcompany.Element("name").Value;
                        string xcompanyaddress = xcompany.Element("address").Value;
                        string xdepartments = string.Join("<br />", xcompany.Element("departments").Descendants());
                        string xcompanyapprovers = string.Join("<br />", xcompany.Element("approvers").Descendants().Distinct());
                        string xhomecurrency = xcompany.Element("homecurrency").Value;

                        ltCompanies.Text += string.Format("<tr class='w3-row listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", xcompanyid, xcompanyname, xcompanyaddress, xdepartments, xcompanyapprovers, xhomecurrency);
                        userCount += 1;
                    }
                    //                    ltCompanies.Text += "</table>";
                    lblAllCompaniesXML.Text = string.Format("User count {0} pcs", userCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text += "<br />" + ex.Message;
            }
        }

        protected void btnGetCompanies_Click(object sender, EventArgs e)
        {
            updateXML();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            txtCompanyID.Visible = true;
            titleCompany.InnerText = "New company";
            NewCompany.Visible = true;
            CompanyList.Visible = false;
            FillControls();
        }

        protected void btnUserDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            NewCompany.Visible = false;
            CompanyList.Visible = true;
            //            txtUserID.Visible = false;
            //            ddlUser.Visible = false;
        }


        protected void FillControls()
        {
            //populating user id dropdownlist
            //user roles
            string userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);

            //user list
            XDocument xdoc = XDocument.Load(userDataFile);
            if (xdoc != null)
            {
                //updating selection list
                ddlUser.Items.Clear();
                foreach (XElement xuser in xdoc.Root.Descendants("user"))
                {
                    string xuserid = xuser.Element("id").Value;
                    string xusername = xuser.Element("username").Value;
                    ddlUser.Items.Add(new ListItem(xuserid + ": " + xusername, xuserid));
                }
                //empty element at selection list
                ddlUser.Items.Insert(0, string.Empty);

                //for CRUD
                ddlUser.SelectedIndex = 0;
                //  txtUserID.Text = string.Empty;
                //  txtUsername.Text = string.Empty;
                //  txtDepartment.Text = string.Empty;
                //  txtEmail.Text = string.Empty;
                //resetting roles to not  selected
                //  foreach (ListItem rooli in chkRoles.Items)
                //  {
                //     rooli.Selected = false;
                // }

            }
        }

        protected void addApprover_Click(object sender, EventArgs e)
        {
            txtApprovers.Text += ddlUser.SelectedValue;
            ddlUser.SelectedIndex = 0;

        }
    }
}