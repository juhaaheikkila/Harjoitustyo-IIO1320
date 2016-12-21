using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JE_Documents.Data;
using System.Xml.Linq;

namespace JE_Documents
{
    public partial class _5_VAT_Codes : System.Web.UI.Page
    {
        Label mpMessage;
        Label mpPageTitle;
        static string strPagetitle = "VAT Code page";
        static string strQueryKey = "id";

        protected void Page_Load(object sender, EventArgs e)
        {
            mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");
            mpPageTitle.Text = strPagetitle;
            mpMessage = (Label)Page.Master.FindControl("lblMessage");
            mpMessage.Text = "...";
            if (!IsPostBack)
            {
                CommonCodes.gLog.logEvent("Opening " + strPagetitle);
                FillControls();
            }
            hideEditForm();


            if (Request.QueryString[strQueryKey] != null)
            {
                string strID = Request.QueryString[strQueryKey];
                if (!"".Equals(strID))
                {

                    JEVATCode vatCode = new JEVATCode(strID, CommonCodes.gVATCodeDatafile, "id");
                    //mpPageTitle.Text = strPagetitle + " / user: " + vatCode.id + " : " + str;
                    getVATCodeData(vatCode);
                }
            }

        }

        protected void btnGetVATCodes_Click(object sender, EventArgs e)
        {
            updateXML("id");
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            int intCounter;

            intCounter = CommonCodes.getCount(CommonCodes.gVATCodeDatafile);

            txtID.Text = Convert.ToString(intCounter);
            txtStatus.Text = CommonCodes.STATUS_DRAFT;
            txtVATCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtVATPercentage.Text = string.Empty;
            rbInput.Checked= false;
            rbOutput.Checked = false;

            displayEditForm("New tax code", false, true, true, true, false);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            txtStatus.Text = CommonCodes.STATUS_DELETED;
            btnSave_Click(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hideEditForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(CommonCodes.gVATCodeDatafile);

            if (xdoc != null)
            {
                //search for user
                var xvatcode = xdoc.Root.Descendants("vatcode").Where(x => x.Element("id").Value == txtID.Text).SingleOrDefault();
                if (xvatcode != null)
                {
                    xvatcode.Remove();
                }

                XElement vatcode = new XElement("vatcode");

                vatcode.Add(new XElement("id", txtID.Text));
                vatcode.Add(new XElement("status", txtStatus.Text));
                vatcode.Add(new XElement("code", txtVATCode.Text));
                vatcode.Add(new XElement("description", txtDescription.Text));
                vatcode.Add(new XElement("percentage", txtVATPercentage.Text));
                if (rbInput.Checked)
                {
                    vatcode.Add(new XElement("inputoutput", rbInput.Text));
                }
                else if (rbOutput.Checked)
                {
                    vatcode.Add(new XElement("inputoutput", rbOutput.Text));
                } else
                {
                    vatcode.Add(new XElement("inputoutput", ""));
                }
               
                vatcode.Add(new XElement("dataok", isDataOk()));
                
                xdoc.Element("vatcodes").Add(vatcode);

            }

            xdoc.Save(CommonCodes.gVATCodeDatafile);
            updateXML("id");
            hideEditForm();

            //updating page
            //Response.Redirect(Request.RawUrl);
        }

        #region METHODS

        protected string isDataOk()
        {
            List<string> strDataOk = new List<string>();
            if ("".Equals(txtDescription.Text)) strDataOk.Add("description missing");
            if ("".Equals(txtVATCode.Text)) strDataOk.Add("VAT Code missing");
            if ("".Equals(txtVATPercentage.Text)) strDataOk.Add("percentage missing");
            if (!rbInput.Checked & !rbOutput.Checked)
            {
                strDataOk.Add("input / output missing");
            }
            return string.Join("<br/>", strDataOk);
        }

        protected void getVATCodeData(JEVATCode rJEVatCode)
        {
            if (rJEVatCode != null)
            {

                displayEditForm(rJEVatCode.id + ": " + rJEVatCode.code, true, true, true, true, true);
                txtID.Text = rJEVatCode.id;
                txtStatus.Text = rJEVatCode.status;
                txtVATCode.Text = rJEVatCode.code;
                txtDescription.Text = rJEVatCode.description;
                txtVATPercentage.Text = rJEVatCode.percentage;
                //radiobutton
                if ("input".Equals(rJEVatCode.inputoutput))
                {
                    rbInput.Checked = true;
                    rbOutput.Checked = false;
                       
                }
                if ("output".Equals(rJEVatCode.inputoutput))
                {
                    rbInput.Checked = false;
                    rbOutput.Checked = true;
                }
                
               
            }
        }

        protected void updateXML(string vstrOrderKey)
        {
            //List all users
            try
            {
                XDocument xDoc = new XDocument();
                XElement newxDoc;
                int userCount = 0;

                xDoc = XDocument.Load(CommonCodes.gVATCodeDatafile);
                if (vstrOrderKey.Equals("id"))
                {
                    newxDoc = new XElement("vatcode", xDoc.Root
                    .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                    .OrderBy(x => (int)x.Element(vstrOrderKey))
                    );
                }
                else
                {
                    newxDoc = new XElement("vatcode", xDoc.Root
                    .Elements().Where(x => x.Element("status").Value != CommonCodes.STATUS_DELETED)
                    .OrderBy(x => (string)x.Element(vstrOrderKey))
                    );
                }
                ltTableHead.Text = "<tr><th>Id</th><th>code</th><th>description</th><th>%</th><th>input / output</th><th></th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                //if (xDoc != null)
                if (newxDoc != null)
                {
                    foreach (XElement xuser in newxDoc.Descendants("vatcode"))
                    {
                        string xid = xuser.Element("id").Value;
                        string xcode = xuser.Element("code").Value;
                        string xdescription = xuser.Element("description").Value;
                        string xpercentage = xuser.Element("percentage").Value;
                        string xinputoutput = xuser.Element("inputoutput").Value;
                        string strDataOk = xuser.Element("dataok").Value;
                        if (!"".Equals(strDataOk))
                        {
                            strDataOk = string.Format("<span title='{0}' style='color:red'><b>X</b></span>", strDataOk);
                        }
                        ltTableData.Text += string.Format("<tr class='listRow'><td><a href='{5}?{6}={0}'>{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{7}</td></tr>", xid, xcode, xdescription, xpercentage, xinputoutput, strEditUrl, strQueryKey, strDataOk);
                        userCount += 1;
                    }
                    int childrenCount = newxDoc.Elements().Count();
                    mpMessage.Text = string.Format("count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                CommonCodes.gLog.logError(strPagetitle + " : " + ex.Message);
                mpMessage.Text = "<br />" + ex.Message;
            }
        }

        protected void FillControls()
        {
            //populating form
            
        }

        protected void hideEditForm()
        {

            NewVAT.Visible = false;
            VATList.Visible = true;
            updateXML("id");
            mpPageTitle.Text = strPagetitle;

        }

        protected void displayEditForm(string strTitle, bool blnShowSelectUser, bool blnShowUserData, bool blnShowSave, bool blnShowCancel, bool blnShowDelete)
        {
            liUserData.Visible = blnShowUserData;
            titleUser.InnerText = strTitle;
            NewVAT.Visible = true;
            VATList.Visible = false;
            FillControls();
            btnSave.Visible = blnShowSave;
            btnCancel.Visible = blnShowCancel;
            btnDelete.Visible = blnShowDelete;
        }

        #endregion


    }
}