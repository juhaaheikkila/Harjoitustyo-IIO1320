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

namespace JE_Documents
{
    public partial class _02_Company : System.Web.UI.Page
    {
        Label mpPageTitle;
        Label mpMessage;
        static string companyDataFile;
        static string userDataFile;
        static List<string> selectedApprovers;
        static List<string> selectedDepartments;
        static string strQueryKey = "CompanyID";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
                mpPageTitle.Text = "Companies page";
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpMessage.Text = "...";

                companyDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CompanyDataFile"]);
                userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);

                FillControls();
                hideEditForm();
                
                if (Request.QueryString[strQueryKey] != null)
                {
                    string strCompanyCode = Request.QueryString[strQueryKey];
                    if (!"".Equals(strCompanyCode))
                    {
                        mpPageTitle.Text = "Companies page / company: " + strCompanyCode;
                        JECompany company = new JECompany(strCompanyCode, companyDataFile);
                        getCompanyData(company);
                    }
                }
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
            XDocument xdoc = XDocument.Load(companyDataFile);

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
            xdoc.Save(companyDataFile);

            hideEditForm();

            //updating page
            //Response.Redirect(Request.RawUrl);
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(companyDataFile);
            if (xdoc != null)
            {
                //search for user
                var xcompany = xdoc.Root.Descendants("company").Where(x => x.Element("id").Value == txtCompanyID.Text).SingleOrDefault();
                if (xcompany != null)
                {
                    xcompany.Remove();
                    xdoc.Save(companyDataFile);
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

        #region METHODS

        protected void getCompanyData(JECompany rJECompany)
        {
            if (rJECompany != null)
            {

                displayEditForm(rJECompany.id + ": " + rJECompany.code + ", " + rJECompany.name, true, true, true, true, true);

                selectedApprovers = new List<string>();
                selectedDepartments = new List<string>();
                selectedApprovers.Add("");
                selectedDepartments.Add("");
                txtCompanyID.Text = rJECompany.id;
                txtCompanyCode.Text = rJECompany.code;
                txtCompanyName.Text = rJECompany.name;
                txtAddress.Text = rJECompany.address;
                //approvers
                foreach (string approver in rJECompany.approvers)
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
                foreach (string department in rJECompany.departments)
                {
                    if (!"".Equals(department))
                    {
                        selectedDepartments.Add(department);
                        ListItem liDepartment = new ListItem(department, department, true);
                        liDepartment.Selected = true;
                        chkDepartments.Items.Add(liDepartment);
                    }
                }
                txtHomeCurrency.Text = rJECompany.homecurrency;

            }

        }

        protected void hideEditForm()
        {
            NewCompany.Visible = false;
            CompanyList.Visible = true;
            updateXML();
        }

        protected void displayEditForm(string strTitle, bool blnShowSelectCompany, bool blnShowCompanyData, bool blnShowSave, bool blnShowCancel, bool blnShowDelete)
        {
            titleCompany.InnerText = strTitle;
            NewCompany.Visible = true;
            CompanyList.Visible = false;
            btnSave.Visible = blnShowSave;
            btnCancel.Visible = blnShowCancel;
            btnDelete.Visible = blnShowDelete;
        }

        protected void updateXML()
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                int companyCount = 0;

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(companyDataFile);
                
                ltTableHead.Text = "<tr><th>Id</th><th>Code</th><th>Name</th><th>Address</th><th>Departments</th><th>approvers</th><th>home Currency</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                if (xDoc != null)
                {
                    // xDoc.Descendants("Team").OrderByDescending(p => DateTime.Parse(p.Element("LastAccessed").Value));
                   
                    
                    foreach (XElement xcompany in xDoc.Root.Descendants("company"))
                    {
                        string xcompanyid = xcompany.Element("id").Value;
                        string xcompanycode = xcompany.Element("code").Value;
                        string xcompanyname = xcompany.Element("name").Value;
                        string xcompanyaddress = xcompany.Element("address").Value;
                        string xdepartments = string.Join("<br />", xcompany.Element("departments").Descendants());
                        string xcompanyapprovers = string.Join("<br />", xcompany.Element("approvers").Descendants().Distinct());
                        string xhomecurrency = xcompany.Element("homecurrency").Value;

                        ltTableData.Text += string.Format("<tr class='w3-row listRow'><td><a href='{7}?{8}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", xcompanyid, xcompanycode, xcompanyname, xcompanyaddress, xdepartments, xcompanyapprovers, xhomecurrency, strEditUrl,strQueryKey);
                        companyCount += 1;
                    }
                    //                    ltCompanies.Text += "</table>";
                    lblAllCompaniesXML.Text = string.Format("Company count {0} pcs", companyCount);
                    int childrenCount = xDoc.Root.Elements().Count();
                    lblAllCompaniesXML.Text += string.Format("Company count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text += "<br />" + ex.Message;
            }
        }

        protected void FillControls()
        {

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