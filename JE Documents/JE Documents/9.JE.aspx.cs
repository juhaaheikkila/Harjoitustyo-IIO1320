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
using System.Data.OleDb;
using System.IO;

namespace JE_Documents
{
    public partial class _9_JE : System.Web.UI.Page
    {

        Label mpPageTitle;

        Label mpMessage;
        Label mpUsername;

        static List<string> selectedApprovers;
        static List<string> selectedDepartments;
        static string strPagetitle = "JE page";
        static string strQueryKey = "jeid";
        static string strRedirectTo = "9.JE.aspx";
        //JEDoc jeDocument;

        protected void Page_Load(object sender, EventArgs e)
        {
            mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
            mpMessage = (Label)Page.Master.FindControl("lblMessage");
            mpUsername = (Label)Page.Master.FindControl("lblUsername");
            if (!IsPostBack)
            {
                mpPageTitle.Text = strPagetitle;
                mpMessage.Text = "";
                FillControls();
                hideEditForm();

                CommonCodes.gLog.logEvent("Opening " + strPagetitle);
                if (Request.QueryString[strQueryKey] != null)
                {
                    string strID = Request.QueryString[strQueryKey];
                    if (!"".Equals(strID))
                    {
                        CommonCodes.gJEDoc = new JEDoc(strID, CommonCodes.gJEDocDatafile, "id");
                        if (CommonCodes.gJEDoc != null)
                        {
                            mpPageTitle.Text = strPagetitle + " / JEdoc: " + strID + " : " + CommonCodes.gJEDoc.documenttype + " " + CommonCodes.gJEDoc.documentnumber;
                            getJEData(CommonCodes.gJEDoc);
                            CommonCodes.gLog.logEvent("Opening jedoc " + strID);
                            mpMessage.Text = "";
                        }
                    }
                }
            }
        }

        //ACTION-BUTTONS
        //NEW
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            CommonCodes.gJEDoc = new JEDoc();
            int intCounter = CommonCodes.getCount(CommonCodes.gJEDocDatafile);

            CommonCodes.gJEDoc.id = Convert.ToString(intCounter);

            CommonCodes.gJEDoc.author = CommonCodes.gUsername;

            addNewStatus(CommonCodes.STATUS_DRAFT, "New JE document");

            //populating fields
            txtID.Text = Convert.ToString(intCounter);
            ddlPeriod.Text = string.Empty;
            ddlType.Text = string.Empty;
            txtDocumentNumber.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtAuthor.Text = CommonCodes.gUsername;
            ddlApprover.Text = string.Empty;
            ddlApprover.Items.Clear();
            txtCompanyCode.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            ddlCompany.Text = string.Empty;
            ddlDepartment.Text = string.Empty;
            txtHomeCurrency.Text = string.Empty;

            ddlDepartment.Text = string.Empty;
            ddlDepartment.Items.Clear();
            txtHeadertext.Text = string.Empty;
            txtHomeCurrency.Text = string.Empty;
            ddlCurrency.Text = string.Empty;
            txtCurrencyRate.Text = string.Empty;
            txtInformation.Text = string.Empty;
            ltProcessingHistoryAll.Text = "";
            lblDebetTotal.Text = string.Empty;
            lblCreditTotal.Text = string.Empty;
            lblDifference.Text = string.Empty;
            //display processing history
            if (CommonCodes.gJEDoc.processinghistory != null)
            {
                foreach (JEDocStatus status in CommonCodes.gJEDoc.processinghistory)
                {
                    ltProcessingHistoryAll.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                        status.id, status.status, status.username, status.date, status.message);
                }
            }

            displayEditForm("New JE document", false, true, true, true, false);
        }

        //CANCEL
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hideEditForm();
        }

        //DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strDocStatus = CommonCodes.STATUS_DELETED;

            CommonCodes.gJEDoc.status = strDocStatus;
            addNewStatus(strDocStatus, "JE document deleted");

            saveJEDoc();

            updateXML("id");

            Response.Redirect(strRedirectTo);

        }

        //SAVE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            saveJEDoc();
            Response.Redirect(strRedirectTo);
        }

        //TO BE APPROVED
        protected void btnToBeApproved_Click(object sender, EventArgs e)
        {
            addNewStatus(CommonCodes.STATUS_TO_BE_APPROVED, "Document sent for approval to");
            //send mail to approver

            saveJEDoc();

            Response.Redirect(strRedirectTo);


        }

        //REJECT
        protected void btnReject_Click(object sender, EventArgs e)
        {
            addNewStatus(CommonCodes.STATUS_DRAFT, "Document rejected by approver");

            saveJEDoc();

            Response.Redirect(strRedirectTo);
        }

        //APPROVE
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            addNewStatus(CommonCodes.STATUS_APPROVED, "Document approver by " + CommonCodes.gUsername);

            saveJEDoc();

            Response.Redirect(strRedirectTo);
        }

        //TRANSFER
        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            addNewStatus(CommonCodes.STATUS_TRANFERED, "Document tranfered to financial system, by " + CommonCodes.gUsername);


            saveJEDoc();

            Response.Redirect(strRedirectTo);
        }

        //SELECT COMPANY
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCompanyCode.Text = ddlCompany.SelectedValue;
            txtCompanyName.Text = ddlCompany.SelectedItem.Text;
            JECompany company = new JECompany(ddlCompany.SelectedValue, CommonCodes.gCompanyDatafile, "code");
            fillDropdownList(ddlDepartment, company.departments, true);

            fillDropdownList(ddlApprover, company.approvers, true);

            txtHomeCurrency.Text = company.homecurrency;
        }

        //SELECT CURRENCY
        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtHomeCurrency.Text.Equals(ddlCurrency.SelectedValue))
            {
                txtCurrencyRate.Text = "1";
            }
            else
            {
                txtCurrencyRate.Text = "";
            }
        }

        //Navigation, display JE doc lists
        //BY ID
        protected void btnGetJEDocs_Click(object sender, EventArgs e)
        {
            updateXML("id");
        }

        //BY COMPANY
        protected void btnGetJEDocsByCompany_Click(object sender, EventArgs e)
        {
            updateXML("companycode");
        }

        //BY STATUS
        protected void btnGetJEDocsByStatus_Click(object sender, EventArgs e)
        {
            updateXML("status");
        }

        //IMPORT DATA
        protected void btnImportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                mpMessage.Text = "";
                if (FileUpload1.HasFile)   //Upload file here
                {
                    FileInfo fileInfo = new FileInfo(FileUpload1.PostedFile.FileName);

                    string fileExt = System.IO.Path.GetExtension(FileUpload1.FileName);  //Get extension

                    if (fileExt == ".csv")   //check to see if its a .csv file
                    {
                        //Saving file
                        FileUpload1.SaveAs(Server.MapPath("UploadedCSVFiles//" + FileUpload1.FileName));        //save file to the specified folder

                    }
                    else
                    {
                        //no csv file selected
                        return;
                    }
                    //create object for CSVReader and pass the stream
                    CSVReader reader = new CSVReader(FileUpload1.PostedFile.InputStream);
                    string[] headers = reader.GetCSVLine();

                    string[] data;
                    int intRowCount = 0;
                    if (rbReplace.Checked & CommonCodes.gJEDoc.rows != null)
                    {
                        CommonCodes.gJEDoc.rows.Clear();
                    }
                    while ((data = reader.GetCSVLine()) != null)
                    {

                        JEDocRow row = new JEDocRow();
                        if (CommonCodes.gJEDoc.rows != null)
                        {
                            row.id = string.Format("{0}", CommonCodes.gJEDoc.rows.Count + 1);//data[0];
                        }
                        else
                        {
                            row.id = "1";
                        }
                        row.company = data[1];
                        row.account = data[2];
                        row.debetcredit = data[3];
                        row.project = data[4];
                        row.dim1 = data[5];
                        row.element = data[6];
                        //total
                        string strTotal = data[7];
                        row.total = Convert.ToDouble(strTotal.Replace(".", CSVReader.CSVDesimalToBe));
                        row.country = data[8];
                        row.vatcode = data[9];
                        row.reference = data[10];

                        if (CommonCodes.gJEDoc.rows == null)
                        {
                            CommonCodes.gJEDoc.rows = new List<JEDocRow>();
                        }
                        CommonCodes.gJEDoc.rows.Add(row);
                        intRowCount += 1;
                    }
                    mpMessage.Text = string.Format("Imported from {0} {1} rows", FileUpload1.FileName, intRowCount);
                    updatingRows(CommonCodes.gJEDoc);
                }
            }
            catch (Exception ex)
            {
                CommonCodes.gLog.logError(strPagetitle + ", importing rows: " + ex.Message + " " + ex.InnerException);

                mpMessage.Text = "<br />" + ex.Message;


            }
        }

        protected string valid(OleDbDataReader myreader, int stval)  //this method checks for null values in the .CSV file, if there are null replace them with 0

        {
            object val = myreader[stval];
            if (val != DBNull.Value)
            {

                return val.ToString();
            }
            else
            {
                return Convert.ToString(0);
            }


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
            ds.ReadXml(CommonCodes.gCompanyDatafile);
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string strCompany = dr["code"].ToString() + " : " + dr["name"].ToString();
                ddlCompany.Items.Add(new ListItem(dr["name"].ToString(), dr["code"].ToString()));
            }
            //add empty line in selection
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
            mpPageTitle.Text = strPagetitle;

        }

        protected void updateXML(string vstrOrderKey)
        {
            try
            {
                JEList.Visible = true;

                int docCount = 0;

                XDocument xDoc = new XDocument();
                XElement newxDoc;
                xDoc = XDocument.Load(CommonCodes.gJEDocDatafile);
                mpMessage = (Label)Page.Master.FindControl("lblMessage");

                ltTableHead.Text = "<tr><th>Id</th><th>Status</th><th>Company</th><th>Period</th><th>Document type</th><th>Document number</th><th>Author</th><th>Approver</th><th>Department</th><th>credit</th><th>debet</th><th>Reference</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                if (xDoc != null)
                {
                    // xDoc.Descendants("Team").OrderByDescending(p => DateTime.Parse(p.Element("LastAccessed").Value));
                    if (vstrOrderKey.Equals("id"))
                    {
                        newxDoc = new XElement("jedoc", xDoc.Root
                        .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                        .OrderBy(x => (int)x.Element(vstrOrderKey))
                        );
                    }
                    else
                    {
                        newxDoc = new XElement("jedoc", xDoc.Root
                        .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                        .OrderBy(x => (string)x.Element(vstrOrderKey))
                        );
                    }

                    foreach (XElement xdoc in newxDoc.Descendants("jedoc"))
                    {
                        string xid = xdoc.Element("id").Value;
                        string xstatus = xdoc.Element("status").Value;
                        string xcompanyname = xdoc.Element("companyname").Value;
                        string xperiod = xdoc.Element("period").Value;
                        string xdoctype = xdoc.Element("documenttype").Value;
                        string xdocnro = xdoc.Element("documentnumber").Value;
                        string xauthor = xdoc.Element("author").Value;
                        string xapprover = xdoc.Element("approver").Value;
                        string xdepartment = xdoc.Element("department").Value;
                        string xheadertext = xdoc.Element("headertext").Value;
                        string xcredittotal = xdoc.Element("credittotal").Value;
                        string xdebettotal = xdoc.Element("debettotal").Value;

                        ltTableData.Text += string.Format("<tr class='listRow'><td><a href='{8}?{9}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{11}<td>{7}</td><td style='text-align:right;'>{12}</td><td style='text-align:right;'>{13}</td><td>{10}</td></tr>",
                            xid, xstatus, xcompanyname, xperiod, xdoctype, xdocnro, xauthor, xdepartment, strEditUrl, strQueryKey, xheadertext, xapprover, xcredittotal, xdebettotal);
                        docCount += 1;
                    }
                    int childrenCount = newxDoc.Elements().Count();
                    mpMessage.Text = string.Format("JE documents count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                CommonCodes.gLog.logError(strPagetitle + ": " + ex.Message + " " + ex.InnerException);

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
            //resetting buttons
            btnDelete.Visible = false;
            btnToBeApproved.Visible = false;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnTransfer.Visible = false;
            FileUploadControls.Visible = false;

            //buttons per status
            switch (CommonCodes.gJEDoc.status)
            {
                case CommonCodes.STATUS_DRAFT:
                    btnToBeApproved.Visible = true;
                    FileUploadControls.Visible = true;
                    btnDelete.Visible = true;
                    break;

                case CommonCodes.STATUS_TO_BE_APPROVED:
                    if ((CommonCodes.gUser.isUserRoleOn("admin") | CommonCodes.gUser.isUserRoleOn("approver")) & (CommonCodes.gUsername).Equals(ddlApprover.Text))
                    {
                        btnApprove.Visible = true;
                        btnReject.Visible = true;
                    }
                    break;

                case CommonCodes.STATUS_APPROVED:
                    if (CommonCodes.gUser.isUserRoleOn("admin") | CommonCodes.gUser.isUserRoleOn("approver"))
                    {
                        btnTransfer.Visible = true;
                    }
                    break;
                case CommonCodes.STATUS_TRANFERED:
                    btnSave.Visible = false;
                    break;
            }
        }

        protected void getJEData(JEDoc rJEDoc)
        {
            if (rJEDoc != null)
            {


                txtID.Text = rJEDoc.id;

                ddlPeriod.Text = rJEDoc.period;
                lblStatus.Text = rJEDoc.status;
                ddlType.Text = rJEDoc.documenttype;
                txtDocumentNumber.Text = rJEDoc.documentnumber;
                txtDate.Text = rJEDoc.date;
                txtAuthor.Text = rJEDoc.author;
                txtCompanyCode.Text = rJEDoc.companycode;
                txtCompanyName.Text = rJEDoc.companyname;
                if (ddlCompany.Items.FindByValue(rJEDoc.companycode) != null)
                {
                    ddlCompany.Items.FindByValue(rJEDoc.companycode).Selected = true;
                }

                //fill in department list
                if (!"".Equals(ddlCompany.SelectedValue))
                {
                    JECompany company = new JECompany(ddlCompany.SelectedValue, CommonCodes.gCompanyDatafile, "code");
                    fillDropdownList(ddlDepartment, company.departments, true);
                    txtHomeCurrency.Text = company.homecurrency;

                    fillDropdownList(ddlApprover, company.approvers, true);
                    ddlApprover.Text = rJEDoc.approver;


                    if (ddlDepartment.Items.FindByValue(rJEDoc.department) != null)
                    {
                        ddlDepartment.Items.FindByValue(rJEDoc.department).Selected = true;
                    }
                }
                ddlDepartment.Text = rJEDoc.department;
                txtHeadertext.Text = rJEDoc.headertext;
                txtHomeCurrency.Text = rJEDoc.homecurrency;
                ddlCurrency.Text = rJEDoc.currency;
                txtCurrencyRate.Text = Convert.ToString(rJEDoc.currencyrate);
                txtInformation.Text = rJEDoc.information;

                //updating rows
                updatingRows(rJEDoc);
                /*
                ltDataRows.Text = "";
                double creditTotal = 0;
                double debetTotal = 0;
                if (rJEDoc.rows != null)
                {

                    foreach (JEDocRow row in rJEDoc.rows)
                    {
                        ltDataRows.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td style='text-align:right;'>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>",
                            row.id, row.company, row.account, row.debetcredit, row.project, row.dim1, row.element, row.total.ToString("0.00"), row.country, row.vatcode, row.reference);
                        if ("c".Equals(row.debetcredit))
                        {
                            creditTotal += row.total;
                        }
                        else if ("d".Equals(row.debetcredit))
                        {
                            debetTotal += row.total;
                        }
                    }
                    rJEDoc.debettotal = debetTotal;
                    rJEDoc.credittotal = creditTotal;
                    lblCreditTotal.Text = creditTotal.ToString("0.00"); // String.Format("{0.00}", creditTotal);
                    lblDebetTotal.Text = debetTotal.ToString("0.00"); // String.Format("{0.00}", debetTotal);
                    lblDifference.Text = (debetTotal - creditTotal).ToString("0.00"); //String.Format("{0.00}", debetTotal - creditTotal);
                }
                */
                ltProcessingHistoryAll.Text = "";

                if (rJEDoc.processinghistory != null)
                {

                    foreach (JEDocStatus status in rJEDoc.processinghistory)
                    {
                        ltProcessingHistoryAll.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                            status.id, status.status, status.username, status.date, status.message);
                    }
                }

                displayEditForm(rJEDoc.id + ": " + rJEDoc.documenttype + "/" + rJEDoc.documentnumber, true, true, true, true, true);

            }
        }

        protected void updatingRows(JEDoc rJEDoc)
        {

            //updating rows
            ltDataRows.Text = "";
            double creditTotal = 0;
            double debetTotal = 0;
            if (rJEDoc.rows != null)
            {

                foreach (JEDocRow row in rJEDoc.rows)
                {
                    ltDataRows.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td style='text-align:right;'>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>",
                        row.id, row.company, row.account, row.debetcredit, row.project, row.dim1, row.element, row.total.ToString("0.00"), row.country, row.vatcode, row.reference);
                    if ("c".Equals(row.debetcredit))
                    {
                        creditTotal += row.total;
                    }
                    else if ("d".Equals(row.debetcredit))
                    {
                        debetTotal += row.total;
                    }
                }
                rJEDoc.debettotal = debetTotal;
                rJEDoc.credittotal = creditTotal;
                lblCreditTotal.Text = creditTotal.ToString("0.00"); // String.Format("{0.00}", creditTotal);
                lblDebetTotal.Text = debetTotal.ToString("0.00"); // String.Format("{0.00}", debetTotal);
                lblDifference.Text = (debetTotal - creditTotal).ToString("0.00"); //String.Format("{0.00}", debetTotal - creditTotal);
            }
        }

        protected void addNewStatus(string strStatus, string strMessage)
        {
            lblStatus.Text = strStatus;
            CommonCodes.gJEDoc.status = strStatus;
            JEDocStatus docStatus = new JEDocStatus();
            docStatus.username = CommonCodes.gUsername;
            docStatus.date = DateTime.Now.ToString();
            docStatus.status = strStatus;
            docStatus.message = strMessage;
            if (CommonCodes.gJEDoc.processinghistory == null)
            {
                docStatus.id = "1";
                CommonCodes.gJEDoc.processinghistory = new List<JEDocStatus>();
            }
            else
            {
                docStatus.id = CommonCodes.gJEDoc.processinghistory.Count().ToString() + 1;
            }
            CommonCodes.gJEDoc.processinghistory.Add(docStatus);
        }

        protected void saveJEDoc()
        {
            string strDataFile = CommonCodes.gJEDocDatafile;
            XDocument xdoc = XDocument.Load(strDataFile);

            if (xdoc != null)
            {
                //search for user
                string strID = txtID.Text;

                var xJEdoc = xdoc.Root.Descendants("jedoc").Where(x => x.Element("id").Value == strID).SingleOrDefault();
                if (xJEdoc != null)
                {
                    xJEdoc.Remove();
                }

                XElement jeDoc = new XElement("jedoc");
                XElement jeDocRows = new XElement("jerows");
                XElement jeDocStatuses = new XElement("jestatuses");
                //XElement approvers = new XElement("approvers");
                //XElement departments = new XElement("departments");
                //document
                jeDoc.Add(new XElement("id", txtID.Text));
                jeDoc.Add(new XElement("status", lblStatus.Text));
                jeDoc.Add(new XElement("period", ddlPeriod.Text));
                jeDoc.Add(new XElement("documenttype", ddlType.Text));
                jeDoc.Add(new XElement("documentnumber", txtDocumentNumber.Text));
                jeDoc.Add(new XElement("date", txtDate.Text));
                jeDoc.Add(new XElement("companycode", txtCompanyCode.Text));
                jeDoc.Add(new XElement("companyname", txtCompanyName.Text));
                jeDoc.Add(new XElement("author", txtAuthor.Text));
                jeDoc.Add(new XElement("approver", ddlApprover.Text));
                jeDoc.Add(new XElement("department", ddlDepartment.Text));
                jeDoc.Add(new XElement("headertext", txtHeadertext.Text));
                jeDoc.Add(new XElement("homecurrency", txtHomeCurrency.Text));
                jeDoc.Add(new XElement("currency", ddlCurrency.Text));
                jeDoc.Add(new XElement("currencyrate", txtCurrencyRate.Text));
                jeDoc.Add(new XElement("information", txtInformation.Text));
                jeDoc.Add(new XElement("debettotal", CommonCodes.gJEDoc.debettotal));
                jeDoc.Add(new XElement("credittotal", CommonCodes.gJEDoc.credittotal));
                jeDoc.Add(new XElement("dataok", ""));

                //statuses
                if (CommonCodes.gJEDoc.processinghistory != null)
                {
                    foreach (JEDocStatus status in CommonCodes.gJEDoc.processinghistory)
                    {
                        XElement jeDocStatus = new XElement("jestatus");
                        jeDocStatus.Add(new XElement("id", status.id));
                        jeDocStatus.Add(new XElement("status", status.status));
                        jeDocStatus.Add(new XElement("username", status.username));
                        jeDocStatus.Add(new XElement("date", status.date));
                        jeDocStatus.Add(new XElement("message", status.message));
                        jeDocStatuses.Add(jeDocStatus);
                    }
                    jeDoc.Add(jeDocStatuses);
                }

                //rows
                if (CommonCodes.gJEDoc.rows != null)
                {
                    foreach (JEDocRow row in CommonCodes.gJEDoc.rows)
                    {
                        XElement jeDocRow = new XElement("jerow");
                        jeDocRow.Add(new XElement("id", row.id));
                        jeDocRow.Add(new XElement("company", row.company));
                        jeDocRow.Add(new XElement("account", row.account));
                        jeDocRow.Add(new XElement("debetcredit", row.debetcredit));
                        jeDocRow.Add(new XElement("project", row.project));
                        jeDocRow.Add(new XElement("dim1", row.dim1));
                        jeDocRow.Add(new XElement("element", row.element));
                        jeDocRow.Add(new XElement("total", row.total));
                        jeDocRow.Add(new XElement("country", row.country));
                        jeDocRow.Add(new XElement("vatcode", row.vatcode));
                        jeDocRow.Add(new XElement("reference", row.reference));
                        jeDocRows.Add(jeDocRow);
                    }
                }

                jeDoc.Add(jeDocRows);

                xdoc.Element("jedocs").Add(jeDoc);
                xdoc.Save(strDataFile);

                hideEditForm();
            }
        }

        #endregion

    }

}