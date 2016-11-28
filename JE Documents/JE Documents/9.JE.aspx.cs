using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace JE_Documents
{


    public partial class _9_JE : System.Web.UI.Page
    {

        Label mpPageTitle;
        Label mpMessage;
        static string companyDataFile;
        static string userDataFile;
        static string jeDataFile;
        static List<string> selectedApprovers;
        static List<string> selectedDepartments;
        static string strQueryKey = "jeid";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
                mpPageTitle.Text = "JE page";
                mpMessage = (Label)Page.Master.FindControl("lblMessage");
                mpMessage.Text = "...";
                companyDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CompanyDataFile"]);
                userDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);
                jeDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["JEDataFile"]);
                FillControls();
            }

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

        //CANCEL
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hideEditForm();
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }


        //SAVE
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        #region METHODS

        protected void FillControls()
        {
            //periods
            string strPeriods = System.Configuration.ConfigurationManager.AppSettings["JEPeriods"];
            string[] strPeriodsArray = strPeriods.Split(',');
            fillDropdownList(ddlPeriod, strPeriodsArray, true);

            //document types
            string strDocTypes = System.Configuration.ConfigurationManager.AppSettings["JETransactionTypes"];
            string[] strDocTypeArray = strDocTypes.Split(',');
            fillDropdownList(ddlType, strDocTypeArray, true);

        }

        protected void fillDropdownList(DropDownList rddl, string[] rArray, bool rAddEmptyline)
        {

            rddl.Items.Clear();
            foreach (var period in rArray)
            {
                rddl.Items.Add(new ListItem(period));
            }
            if (rAddEmptyline)
            {
                //empty element at selection list
                rddl.Items.Insert(0, string.Empty);
                rddl.SelectedIndex = 0;
            }
        }

        protected void hideEditForm()
        {

            JEDoc.Visible = false;
            JEList.Visible = true;
            updateXML();

        }

        protected void updateXML()
        {
            //Listaa kaikki käyttäjät XML-tiedostosta
            try
            {
                int docCount = 0;

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(jeDataFile);

                ltTableHead.Text = "<tr><th>Id</th><th>Code</th><th>Name</th><th>Address</th><th>Departments</th><th>approvers</th><th>home Currency</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                if (xDoc != null)
                {
                    // xDoc.Descendants("Team").OrderByDescending(p => DateTime.Parse(p.Element("LastAccessed").Value));


                    foreach (XElement xcompany in xDoc.Root.Descendants("jedoc"))
                    {
                        string xid = xcompany.Element("id").Value;
                        string xstatus = xcompany.Element("code").Value;
                        string xcompanyname = xcompany.Element("name").Value;
                        string xcompanyaddress = xcompany.Element("address").Value;
                        string xdepartments = string.Join("<br />", xcompany.Element("departments").Descendants());
                        string xcompanyapprovers = string.Join("<br />", xcompany.Element("approvers").Descendants().Distinct());
                        string xhomecurrency = xcompany.Element("homecurrency").Value;

                        //ltTableData.Text += string.Format("<tr class='w3-row listRow'><td><a href='{7}?{8}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", xcompanyid, xcompanycode, xcompanyname, xcompanyaddress, xdepartments, xcompanyapprovers, xhomecurrency, strEditUrl, strQueryKey);
                        docCount += 1;
                    }
                    //                    ltCompanies.Text += "</table>";
                    lblAllJEDocuments.Text = string.Format("JE documents count {0} pcs", docCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text += "<br />" + ex.Message;
            }
        }

        protected void displayEditForm(string strTitle, bool blnShowSelectCompany, bool blnShowCompanyData, bool blnShowSave, bool blnShowCancel, bool blnShowDelete)
        {
            titleJE.InnerText = strTitle;
            JEDoc.Visible = true;
            JEList.Visible = false;
            btnSave.Visible = blnShowSave;
            btnCancel.Visible = blnShowCancel;
            btnDelete.Visible = blnShowDelete;
        }

        #endregion



        protected void btnGetJEDocs_Click(object sender, EventArgs e)
        {

        }

    }

}