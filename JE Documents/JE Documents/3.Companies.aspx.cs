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
        static List<string> selectedApprovers;
        static List<string> selectedDepartments;
        static string strQueryKey = "CompanyID";
        static string strPagetitle = "Companies page";
        static string strRedirectTo = "3.Companies.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
                mpPageTitle.Text = strPagetitle;
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpMessage.Text = "...";

                FillControls();
                hideEditForm();

                if (Request.QueryString[strQueryKey] != null)
                {
                    string strCompanyCode = Request.QueryString[strQueryKey];
                    if (!"".Equals(strCompanyCode))
                    {
                        JECompany company = new JECompany(strCompanyCode, CommonCodes.gCompanyDatafile, "id");
                        mpPageTitle.Text = strPagetitle + " / company: " + strCompanyCode + " : " + company.name;
                        getCompanyData(company);
                    }
                }

                CommonCodes.gLog.logEvent("Opening " + strPagetitle);
            }


        }

        //ACTION-BUTTONS

        //LIST COMPANIES
        protected void btnGetCompanies_Click(object sender, EventArgs e)
        {
            updateXML("id");
        }

        //NEW
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            int intCounter;
            intCounter = CommonCodes.getCount(CommonCodes.gCompanyDatafile);
            txtCompanyID.Text = Convert.ToString(intCounter);
            selectedApprovers = new List<string>();
            selectedDepartments = new List<string>();
            selectedApprovers.Add("");
            selectedDepartments.Add("");
            txtCompanyCode.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            //txtHomeCurrency.Text = string.Empty;
            ddlCurrency.Text = string.Empty;
            //chkApprovers.ClearSelection();
            chkApprovers.Items.Clear();
            //chkDepartments.ClearSelection();
            chkDepartments.Items.Clear();
            displayEditForm("New company", false, true, true, true, false);
        }

        //SAVE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(CommonCodes.gCompanyDatafile);

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
                company.Add(new XElement("status", txtStatus.Text));
                company.Add(new XElement("code", txtCompanyCode.Text));
                company.Add(new XElement("name", txtCompanyName.Text));
                company.Add(new XElement("address", txtAddress.Text));
                company.Add(new XElement("homecurrency", ddlCurrency.Text));
                company.Add(new XElement("dataok", isDataOk()));
                //approvers
                foreach (ListItem person in chkApprovers.Items)
                {
                    if (person.Selected)
                    {
                        if (!"".Equals(person.Value))
                        {
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
            xdoc.Save(CommonCodes.gCompanyDatafile);
            updateXML("id");
            hideEditForm();

            Response.Redirect(strRedirectTo);
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            txtStatus.Text = CommonCodes.STATUS_DELETED;
            btnSave_Click(sender, e);

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
                //ListItem liDepartment = new ListItem(txtDepartment.Text, txtDepartment.Text, true);
                ListItem liDepartment = new ListItem(txtDepartment.Text);
                liDepartment.Selected = true;
                chkDepartments.Items.Add(liDepartment);
                txtDepartment.Text = "";
            }
        }

        #region METHODS

        protected string isDataOk()
        {
            List<string> strDataOk = new List<string>();

            if ("".Equals(txtCompanyCode.Text)) strDataOk.Add("code missing");
            if ("".Equals(txtCompanyName.Text)) strDataOk.Add("name missing");
            if ("".Equals(txtAddress.Text)) strDataOk.Add("address missing");
            if ("".Equals(ddlCurrency.Text)) strDataOk.Add("homecurrency missing");
            if ("".Equals(chkApprovers.SelectedValue)) strDataOk.Add("approvers missing");
            if ("".Equals(chkDepartments.SelectedValue)) strDataOk.Add("departments missing");

            return string.Join("&#10;", strDataOk);
        }

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
                txtStatus.Text = rJECompany.status;
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
                //txtHomeCurrency.Text = rJECompany.homecurrency;
                ddlCurrency.Text = rJECompany.homecurrency;

            }

        }

        protected void hideEditForm()
        {
            NewCompany.Visible = false;
            CompanyList.Visible = true;
            updateXML("id");
            //mpPageTitle.Text = strPagetitle;
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

        protected void updateXML(string vstrOrderKey)
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                XDocument xDoc = new XDocument();
                XElement newxDoc;
                xDoc = XDocument.Load(CommonCodes.gCompanyDatafile);
                if (xDoc != null)
                {

                    if (vstrOrderKey.Equals("id"))
                    {
                        newxDoc = new XElement("company", xDoc.Root
                            .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                            .OrderBy(x => (int)x.Element(vstrOrderKey))
                            );
                    }
                    else
                    {
                        newxDoc = new XElement("company", xDoc.Root
                            .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                            .OrderBy(x => (string)x.Element(vstrOrderKey))

                            );
                    }

                    //ltTableHead.Text = "<tr><th>Id</th><th>Code</th><th>Name</th><th>Address</th><th>Departments</th><th>approvers</th><th>home Currency</th><th></th></tr>";
                    ltTableHead.Text = "<tr><th>Id</th><th>Code</th><th>Name</th><th>Address</th><th>Departments</th><th>approvers</th><th>home Currency</th></tr>";
                    ltTableData.Text = "";
                    string strEditUrl = Request.Url.ToString();

                    foreach (XElement xcompany in newxDoc.Descendants("company"))
                    {
                        string xcompanyid = xcompany.Element("id").Value;
                        string xcompanycode = xcompany.Element("code").Value;
                        string xcompanyname = xcompany.Element("name").Value;
                        string xcompanyaddress = xcompany.Element("address").Value;
                        string xdepartments = string.Join("<br />", xcompany.Element("departments").Descendants());
                        string xcompanyapprovers = string.Join("<br />", xcompany.Element("approvers").Descendants().Distinct());
                        string xhomecurrency = xcompany.Element("homecurrency").Value;
                        string strDataOk = xcompany.Element("dataok").Value;
                        if (!"".Equals(strDataOk))
                        {
                            strDataOk = string.Format("<span title='{0}' style='color:red'><b>X</b></span>", strDataOk);
                            
                        }
                        ltTableData.Text += string.Format("<tr class='listRow'><td><a href='{7}?{8}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{9}</td></tr>", xcompanyid, xcompanycode, xcompanyname, xcompanyaddress, xdepartments, xcompanyapprovers, xhomecurrency, strEditUrl, strQueryKey, strDataOk);
                        
                    }
                    int childrenCount = newxDoc.Elements().Count();

                    mpMessage.Text = string.Format("Company count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                CommonCodes.gLog.logError(strPagetitle + " : " + ex.Message);
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpMessage.Text = "<br />" + ex.Message;
            }
        }

        protected void FillControls()
        {

            //updating user selection list loop through table
            ddlUser.Items.Clear();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.ReadXml(CommonCodes.gUserDatafile);
            dt = ds.Tables[0];
            /*
            foreach (DataRow dr in dt.Rows)
            {
                if (!CommonCodes.STATUS_DELETED.Equals(dr["status"]))
                {
                    string strUsername = dr["username"].ToString();
                    ddlUser.Items.Add(new ListItem(strUsername, strUsername));
                }
            }
            */
            //using xdocument  linq
            XDocument xDoc = new XDocument();
            XElement newxDoc;
            xDoc = XDocument.Load(CommonCodes.gUserDatafile);
            if (xDoc != null)
            {
                newxDoc = new XElement("user", xDoc.Root.Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED && "".Equals(x.Element("dataok").Value))
                        .OrderBy(x => (string)x.Element("username"))
                        );
                DataSet ds2 = new DataSet();
                System.IO.MemoryStream memstr = new System.IO.MemoryStream();
                newxDoc.Save(memstr);
                memstr.Position = 0;
                ds2.ReadXml(memstr, XmlReadMode.Auto);

                ddlUser.DataSource = ds2.Tables[0];
                ddlUser.DataValueField = "username";
                ddlUser.DataTextField = "username";
                ddlUser.DataBind();
            }

            //  ddlUser.DataSource = ds.Tables[0];
            //  ddlUser.DataTextField = "username";
            //  ddlUser.DataValueField = "username";
            //  ddlUser.DataBind();

            //empty element at selection list
            ddlUser.Items.Insert(0, string.Empty);
            ddlUser.SelectedIndex = 0;

            //currencies
            fillDropdownListFromWebConfig(ddlCurrency, "JECurrencies", true);

            chkApprovers.Items.Clear();
            chkDepartments.Items.Clear();
        }

        protected void fillDropdownListFromWebConfig(DropDownList rddl, string strKey, bool rAddEmptyline) //string[] rArray, bool rAddEmptyline)
        {
            string strValues = System.Configuration.ConfigurationManager.AppSettings[strKey];
            string[] strValuesArray = strValues.Split(',');

            rddl.Items.Clear();
            foreach (var itemi in strValuesArray)
            {
                rddl.Items.Add(new ListItem(itemi));
            }

            if (rAddEmptyline)
            {
                //empty element at selection list
                rddl.Items.Insert(0, string.Empty);
                rddl.SelectedIndex = 0;
            }
        }

        #endregion

        protected void btnGetCompaniesByName_Click(object sender, EventArgs e)
        {
            updateXML("code");
        }
    }
}