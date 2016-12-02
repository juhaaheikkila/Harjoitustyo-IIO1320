using System;
using System.Collections.Generic;
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

            hideEditForm();

            if (Request.QueryString[strQueryKey] != null)
            {
                string strID = Request.QueryString[strQueryKey];
                if (!"".Equals(strID))
                {
                    mpPageTitle.Text = "JE page / JEdoc: " + strID;
                    JEDoc jeDoc = new JEDoc(strID, jeDataFile, "id");
                    getJEData(jeDoc);
                }
            }


        }

        //NEW
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            int intCounter;
            XmlDocument doc = new XmlDocument();
            doc.Load(jeDataFile);
            if (doc != null)
            {
                // XmlNodeList root = doc.DocumentElement;

                // Get and display all the book titles.
                XmlElement root = doc.DocumentElement;
                XmlNodeList elemList = root.GetElementsByTagName("jedoc");
                intCounter = elemList.Count + 1;
            }
            else
            {
                intCounter = 0;
            }

            txtID.Text = Convert.ToString(intCounter);
            selectedApprovers = new List<string>();
            selectedDepartments = new List<string>();
            selectedApprovers.Add("");
            selectedDepartments.Add("");
            updateProcessingHistory();
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
            fillDropdownListFromWebConfig(ddlPeriod, "JEPeriods", true);

            //document types
            fillDropdownListFromWebConfig(ddlType, "JETransactionTypes", true);

            //currencies
            fillDropdownListFromWebConfig(ddlCurrency, "JECurrencies", true);

            //companycodes
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.ReadXml(companyDataFile);
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string strCompany = dr["code"].ToString() + " : " + dr["name"].ToString();
                ddlCompany.Items.Add(new ListItem(dr["name"].ToString(), dr["code"].ToString()));
            }

            ddlCompany.Items.Insert(0, string.Empty);


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

        protected void fillDropdownList(DropDownList rddl, string[] rArray, bool rAddEmptyline)
        {
            

            rddl.Items.Clear();
            foreach (var itemi in rArray)
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

        protected void hideEditForm()
        {

            JEDoc.Visible = false;
            JEList.Visible = true;
            updateXML("id");

        }

        protected void updateXML(string vstrOrderKey)
        {
            try
            {
                JEList.Visible = true;

                int docCount = 0;

                XDocument xDoc = new XDocument();
                XElement newxDoc;
                xDoc = XDocument.Load(jeDataFile);
                mpMessage = (Label)Page.Master.FindControl("lblMessage");

                ltTableHead.Text = "<tr><th>Id</th><th>Status</th><th>Company</th><th>Period</th><th>Document type</th><th>Document number</th><th>Author</th><th>Department</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                if (xDoc != null)
                {
                    // xDoc.Descendants("Team").OrderByDescending(p => DateTime.Parse(p.Element("LastAccessed").Value));
                    if (vstrOrderKey.Equals("id"))
                    {
                        newxDoc = new XElement("jedoc", xDoc.Root
                        .Elements()
                        .OrderBy(x => (int)x.Element(vstrOrderKey))
                        );
                    }
                    else
                    {
                        newxDoc = new XElement("jedoc", xDoc.Root
                        .Elements()
                        .OrderBy(x => (string)x.Element(vstrOrderKey))
                        );
                    }

                    foreach (XElement xcompany in newxDoc.Descendants("jedoc"))
                    {
                        string xid = xcompany.Element("id").Value;
                        string xstatus = xcompany.Element("status").Value;
                        string xcompanyname = xcompany.Element("companyname").Value;
                        string xperiod = xcompany.Element("period").Value;
                        string xdoctype = xcompany.Element("documenttype").Value;
                        string xdocnro = xcompany.Element("documentnumber").Value;
                        string xauthor = xcompany.Element("author").Value;
                        string xdepartment = xcompany.Element("department").Value;

                        ltTableData.Text += string.Format("<tr class='listRow'><td><a href='{8}?{9}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>", 
                            xid, xstatus, xcompanyname, xperiod, xdoctype, xdocnro, xauthor, xdepartment, strEditUrl, strQueryKey);
                        docCount += 1;
                    }
                    int childrenCount = newxDoc.Elements().Count();
                    lblAllJEDocuments.Text = string.Format("JE documents count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                mpMessage.Text = "<br />" + ex.Message;
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


        protected void hlToggleProcessingHistory_Click(object sender, EventArgs e)
        {
            bool showProsessing = processingHistory_All.Visible;

            processingHistory_All.Visible = !showProsessing;
            processingHistory_Latest.Visible = showProsessing;
            if (showProsessing)
            {
                lblProcessingHistoryToggle.Text = ">>>";
            } else
            {
                lblProcessingHistoryToggle.Text = "<<<";
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCompanyCode.Text = ddlCompany.SelectedValue;
            txtCompanyName.Text = ddlCompany.SelectedItem.Text;
            JECompany company = new JECompany(ddlCompany.SelectedValue, companyDataFile, "code");
            fillDropdownList(ddlDepartment, company.departments, true);
            txtHomeCurrency.Text = company.homecurrency;
        }

        protected void updateProcessingHistory()
        {
            ltProcessingHistoryAll.Text = "All";
            ltProcessingHistoryLatest.Text = "Latest";
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtHomeCurrency.Text.Equals(ddlCurrency.SelectedValue))
            {
                txtCurrencyRate.Text = "1";
            } else
            {
                txtCurrencyRate.Text = "";
            }
        }

//Navigation, display JE doc lists
        protected void btnGetJEDocs_Click(object sender, EventArgs e)
        {
            updateXML("id");
        }

        protected void btnGetJEDocsByCompany_Click(object sender, EventArgs e)
        {
            updateXML("companycode");
        }

        protected void btnGetJEDocsByStatus_Click(object sender, EventArgs e)
        {
            updateXML("status");
        }
    }

}