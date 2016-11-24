using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using JE_Documents.Data;
using System.Data;
using JE_Documents.Data;

namespace JE_Documents
{
    public partial class _02_Company : System.Web.UI.Page
    {
        Label mpMessage;
        static string DataFile;
        static string userDataFile;
        static public List<string> selectedApprovers;
        static public List<string> selectedDepartments;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Label mpTitle = (Label)Page.Master.FindControl("lblTitle");
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpTitle.Text = "Company page";
                mpMessage.Text = "...";
                DataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CompanyDataFile"]);
                userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);
                updateXML();
                FillControls();
            }
        }

        //ACTION-BUTTONS

        //LIST COMPANIES
        protected void btnGetCompanies_Click(object sender, EventArgs e)
        {
            updateXML();
        }

        //NEW
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            selectedApprovers = new List<string>();
            selectedDepartments = new List<string>();
            selectedApprovers.Add("");
            selectedDepartments.Add("");
            displayEditForm("New company", false, true, true, true, false);
        }

        //SAVE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(DataFile);

            if (xdoc != null)
            {
                //search for user
                string strCompanyID = txtCompanyID.Text;

                var xcompany = xdoc.Root.Descendants("company").Where(x => x.Element("id").Value == strCompanyID).SingleOrDefault();
                if (xcompany != null)
                {
                    xcompany.Remove();
                }

                XElement company = new XElement("company");
                XElement approvers = new XElement("approvers");
                XElement departments = new XElement("departments");

                company.Add(new XElement("id", txtCompanyID.Text));
                company.Add(new XElement("code", txtCompanyCode.Text));
                company.Add(new XElement("name", txtCompanyName.Text));
                company.Add(new XElement("address", txtAddress.Text));
                company.Add(new XElement("homecurrency", txtHomeCurrency.Text));

                //approvers
                foreach (ListItem person in chkApprovers.Items)
                {
                    if (person.Selected)
                    {
                        if (!"".Equals(person.Value)) {
                            approvers.Add(new XElement("approver", person.Value));
                        }
                    }
                }
                company.Add(approvers);

                //departments
                foreach (ListItem department in chkDepartments.Items)
                {
                    if (department.Selected)
                    {
                        if (!"".Equals(department.Value))
                        {
                            departments.Add(new XElement("department", department.Value));
                        }
                    }
                }
                company.Add(departments);

                xdoc.Element("companies").Add(company);
                
            }
            xdoc.Save(DataFile);

            hideEditForm();

            //updating page
            //Response.Redirect(Request.RawUrl);
        }

        //EDIT
        protected void btnModify_Click(object sender, EventArgs e)
        {
            displayEditForm("Select company to edit", true, false, false, false, false);
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(DataFile);

            if (xdoc != null)
            {
                //search for user
                var xcompany = xdoc.Root.Descendants("company").Where(x => x.Element("id").Value == txtCompanyID.Text).SingleOrDefault();
                if (xcompany != null)
                {
                    xcompany.Remove();
                    xdoc.Save(DataFile);
                }
                hideEditForm();
                
            }
        }

        //CANCED
        protected void btnCancel_Click(object sended, EventArgs e)
        {
            hideEditForm();
        }

        //ADD APPROVER
        protected void addApprover_Click(object sender, EventArgs e)
        {
            if (!selectedApprovers.Contains(ddlUser.SelectedValue) && !"".Equals(ddlUser.SelectedValue))
            {
                selectedApprovers.Add(ddlUser.SelectedValue);
                ListItem approveri = new ListItem(ddlUser.SelectedValue, ddlUser.SelectedValue, true);
                approveri.Selected = true;
                chkApprovers.Items.Add(approveri);
                ddlUser.SelectedIndex = 0;
            }
        }

        //ADD DEPARTMENT
        protected void addDepartment_Click(object sender, EventArgs e)
        {
            if (!selectedDepartments.Contains(txtDepartment.Text) && !"".Equals(txtDepartment.Text))
            {
                selectedDepartments.Add(txtDepartment.Text);
                ListItem liDepartment = new ListItem(txtDepartment.Text, txtDepartment.Text, true);
                liDepartment.Selected = true;
                chkDepartments.Items.Add(liDepartment);
                txtDepartment.Text = "";
            }
        }

        //SELECT COMPANY
        protected void ddlCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            selectedApprovers = new List<string>();
            selectedDepartments = new List<string>();
            selectedApprovers.Add("");
            selectedDepartments.Add("");

            JECompany company = new JECompany(ddlCompanies.SelectedValue, DataFile);

            displayEditForm(company.id + ": " + company.name, true, true, true, true, true);

            txtCompanyID.Text = company.id;
            txtCompanyCode.Text = company.code;
            txtCompanyName.Text = company.name;
            txtAddress.Text = company.address;
            //approvers
            foreach (string approver in company.approvers)
            {
                if (!"".Equals(approver))
                {
                    selectedApprovers.Add(approver);
                    ListItem liApprover = new ListItem(approver, approver, true);
                    liApprover.Selected = true;
                    chkApprovers.Items.Add(liApprover);
                }
            }
            //departments
            foreach (string department in company.departments)
            {
                if (!"".Equals(department))
                {
                    selectedDepartments.Add(department);
                    ListItem liDepartment = new ListItem(department, department, true);
                    liDepartment.Selected = true;
                    chkDepartments.Items.Add(liDepartment);
                }
            }
            txtHomeCurrency.Text = company.homecurrency;
        }

        #region METHODS

        protected void hideEditForm()
        {
            NewCompany.Visible = false;
            CompanyList.Visible = true;
            divNavigation.Visible = true;
            updateXML();
            FillControls();
        }

        protected void displayEditForm(string strTitle, bool blnShowSelectCompany, bool blnShowCompanyData, bool blnShowSave, bool blnShowCancel, bool blnShowDelete)
        {
            divNavigation.Visible = false;
            liCompanyData.Visible = blnShowCompanyData;
            titleCompany.InnerText = strTitle;
            NewCompany.Visible = true;
            CompanyList.Visible = false;
            FillControls();
            btnSave.Visible = blnShowSave;
            btnCancel.Visible = blnShowCancel;
            btnDelete.Visible = blnShowDelete;
        }

        protected void updateXML()
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                int userCount = 0;

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(DataFile);
                
                ltCompanies.Text = "";

                if (xDoc != null)
                {
                    // xDoc.Descendants("Team").OrderByDescending(p => DateTime.Parse(p.Element("LastAccessed").Value));
                   
                    ltCompanies.Text += "<tr class='w3-row listTitle'><td>Id</td><td>Code</td><td>Name</td><td>Address</td><td>Departments</td><td>approvers</td><td>home Currency</td></tr>";
                    foreach (XElement xcompany in xDoc.Root.Descendants("company"))
                    {
                        string xcompanyid = xcompany.Element("id").Value;
                        string xcompanycode = xcompany.Element("code").Value;
                        string xcompanyname = xcompany.Element("name").Value;
                        string xcompanyaddress = xcompany.Element("address").Value;
                        string xdepartments = string.Join("<br />", xcompany.Element("departments").Descendants());
                        string xcompanyapprovers = string.Join("<br />", xcompany.Element("approvers").Descendants().Distinct());
                        string xhomecurrency = xcompany.Element("homecurrency").Value;

                        ltCompanies.Text += string.Format("<tr class='w3-row listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", xcompanyid, xcompanycode, xcompanyname, xcompanyaddress, xdepartments, xcompanyapprovers, xhomecurrency);
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

        protected void FillControls()
        {
            //company dropdownlist
            ddlCompanies.Items.Clear();
            DataSet dsCompanies = new DataSet();
            dsCompanies.ReadXml(DataFile);
            ddlCompanies.DataSource = dsCompanies.Tables[0];
            ddlCompanies.DataTextField = "name";
            ddlCompanies.DataValueField = "id";
            ddlCompanies.DataBind();
            //empty element at selection list
            ddlCompanies.Items.Insert(0, "Edit company");
            ddlCompanies.SelectedIndex = 0;

            //updating user selection list
            ddlUser.Items.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(userDataFile);
            ddlUser.DataSource = ds.Tables[0];
            ddlUser.DataTextField = "username";
            ddlUser.DataValueField = "username";
            ddlUser.DataBind();
            //empty element at selection list
            ddlUser.Items.Insert(0, string.Empty);
            ddlUser.SelectedIndex = 0;


            chkApprovers.Items.Clear();
            chkDepartments.Items.Clear();
        }


        #endregion

    }
}