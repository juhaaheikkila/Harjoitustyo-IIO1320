using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using JE_Documents.Data;

namespace JE_Documents
{
    public partial class _3_Logs : System.Web.UI.Page
    {
        string logDataFile;
        Label mpMessage;
        Label mpPageTitle;

        protected void Page_Load(object sender, EventArgs e)
        {
            mpPageTitle = (Label)Page.Master.FindControl("lblPageTitle");

            mpMessage = (Label)Page.Master.FindControl("lblMessage");

            logDataFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["LogFile"]);

            if (!IsPostBack)
            {
                mpPageTitle.Text = "Logs page";
                mpMessage.Text = "";
                LogList.Visible = true;
                updateXML("id");
            }


        }

        //Navigation
        protected void btnLogsByDate_Click(object sender, EventArgs e)
        {
            updateXML("date");
        }

        protected void btnLogsByUser_Click(object sender, EventArgs e)
        {
            updateXML("username");
        }

        protected void btnLogsById_Click(object sender, EventArgs e)
        {
            updateXML("id");
        }

        protected void btnLogsBySeverity_Click(object sender, EventArgs e)
        {
            updateXML("severity");
        }

        protected void btnLogsByError_Click(object sender, EventArgs e)
        {
            updateXML("error");
        }

        //updating list
        protected void updateXML(string vstrOrderKey)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement newxDoc;
                int userCount = 0;

                xDoc = XDocument.Load(logDataFile);
                if (vstrOrderKey.Equals("id"))
                {
                    newxDoc = new XElement("logentry", xDoc.Root
                        .Elements()
                        .OrderBy(x => (int)x.Element(vstrOrderKey))
                        );
                }
                else if (vstrOrderKey.Equals("error"))
                {
                    newxDoc = new XElement("logentry", xDoc.Root
                        .Elements()
                        .Where(x => x.Element("severity").Value == "9")
                        .OrderBy(x => (string)x.Element(vstrOrderKey))
                        );
                }
                else
                {
                    newxDoc = new XElement("logentry", xDoc.Root
                        .Elements()
                        .OrderByDescending(x => (string)x.Element(vstrOrderKey))
                        );
                }
                ltTableHead.Text = "<tr><th>id</th><th>date</th><th>Log entry</th><th>Username</th><th>Severity</th></tr>";
                ltTableData.Text = "";
                string strEditUrl = Request.Url.ToString();
                //if (xDoc != null)
                if (newxDoc != null)
                {
                    foreach (XElement xlog in newxDoc.Descendants("logentry"))
                    {
                        string xcol1 = xlog.Element("id").Value;
                        string xcol2 = xlog.Element("date").Value;
                        string xcol3 = xlog.Element("entry").Value;
                        string xcol4 = xlog.Element("username").Value;
                        string xcol5 = xlog.Element("severity").Value;
                        ltTableData.Text += string.Format("<tr class='listRow'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", xcol1, xcol2, xcol3, xcol4, xcol5);
                        userCount += 1;
                    }
                    int childrenCount = newxDoc.Elements().Count();
                    mpMessage.Text = string.Format("Log entries count {0} pcs", childrenCount);
                }
            }
            catch (Exception ex)
            {
                CommonCodes.gLog.logError(ex.Message);
                mpMessage.Text = "<br />" + ex.Message;
            }
        }


    }
}