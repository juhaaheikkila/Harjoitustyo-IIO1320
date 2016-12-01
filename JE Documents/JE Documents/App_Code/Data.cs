using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace JE_Documents.Data
{
    public class JEDoc
    {
        public string id { get; set; }
        public string status { get; set; }
        public string period { get; set; }
        public string documenttype { get; set; }
        public string documentnumber { get; set; }
        public string date { get; set; }
        public string companycode { get; set; }
        public string companyname { get; set; }
        public string author { get; set; }
        public string department { get; set; }
        public string headertext { get; set; }
        public string currency { get; set; }
        public double currencyrate { get; set; }
        public string information { get; set; }
        public JEDocRow[] rows { get; set; }
        public JEDocStatus[] processinghistory { get; set; }

        public JEDoc()
        {

        }

        public JEDoc(string rstrID, string rstrJEDatafile, string rstrKeyField)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrJEDatafile);
            if (xDoc != null)
            {
                var xjedoc = xDoc.Root.Descendants("user").Where(x => x.Element(rstrKeyField).Value == rstrJEDatafile).SingleOrDefault();
                if (xjedoc != null)
                {
                    string xid = xjedoc.Element("id").Value;
                    string xstatus = xjedoc.Element("status").Value;
                    string xperiod = xjedoc.Element("period").Value;
                    string xdoctype = xjedoc.Element("documenttype").Value;
                    string xdocnumber = xjedoc.Element("documentnumber").Value;
                    string xdate = xjedoc.Element("date").Value;
                    string xcompanycode = xjedoc.Element("companycode").Value;
                    string xcompanyname = xjedoc.Element("companyname").Value;
                    string xauthor = xjedoc.Element("author").Value;
                    string xdepartment = xjedoc.Element("department").Value;
                    string xheadertext = xjedoc.Element("headertext").Value;
                    string xcurrency = xjedoc.Element("currency").Value;
                    string xcurrencyrate = xjedoc.Element("currencyrate").Value;
                    string xinformation = xjedoc.Element("information").Value;


                    List<string> xuserRoles = new List<string>();
                    foreach (XElement xrole in xjedoc.Descendants("jerows"))
                    {
                        //xuserRoles.Add(xrole.Value);
                    }

                    this.id = xid;
                    this.status = xstatus;
                    this.period = xperiod;
                    this.documenttype = xdoctype;
                    this.documentnumber = xdocnumber;
                    this.date = xdate;
                    this.companycode = xcompanycode;
                    this.companyname = xcompanyname;
                    this.author = xauthor;
                    this.department = xdepartment;
                    this.headertext = xheadertext;
                    this.currency = xcurrency;
                    this.currencyrate = Convert.ToDouble(xcurrencyrate);
                    this.information = xinformation;
                    //rows

                    //statuses
                    foreach (XElement status in xjedoc.Descendants("jestatuses"))
                    {
                        JEDocStatus docStatus = new JEDocStatus();
                        docStatus.id = status.Element("id").Value;
                        docStatus.status = status.Element("status").Value;
                        docStatus.username = status.Element("username").Value;
                        docStatus.date = status.Element("date").Value;

                    }
                }
            }
        }
    }

    public class JEDocStatus
    {
        public string id { get; set; }
        public string status { get; set; }
        public string username { get; set; }
        public string date { get; set; }

        public JEDocStatus()
        {

        }
    }

    public class JEDocRow
    {
        public string id { get; set; }
        public string company { get; set; }
        public string account { get; set; }
        public string debetcredit { get; set; }
        public string project { get; set; }
        public string dim1 { get; set; }
        public string element { get; set; }
        public double total { get; set; }
        public string country { get; set; }
        public string vatcode { get; set; }
        public string reference { get; set; }
        public JEDocRow()
        {

        }
    }

    public class JEuser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public string[] roles { get; set; }

        public JEuser()
        {

        }

        public JEuser(string rstrusername, string rstrUserDataFile, string rstrKeyField)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrUserDataFile);
            if (xDoc != null)
            {
                var xuser = xDoc.Root.Descendants("user").Where(x => x.Element(rstrKeyField).Value == rstrusername).SingleOrDefault();
                if (xuser != null)
                {
                    string xuserid = xuser.Element("id").Value;
                    string xusername = xuser.Element("username").Value;
                    string xfirstname = xuser.Element("firstname").Value;
                    string xlastname = xuser.Element("lastname").Value;
                    string xuserdepartment = xuser.Element("department").Value;
                    string xuseremail = xuser.Element("email").Value;
                    List<string> xuserRoles = new List<string>();
                    foreach (XElement xrole in xuser.Descendants("role"))
                    {
                        xuserRoles.Add(xrole.Value);
                    }
                    this.id = xuserid;
                    this.username = xusername;
                    this.firstname = xfirstname;
                    this.lastname = xlastname;
                    this.department = xuserdepartment;
                    this.email = xuseremail;
                    this.roles = xuserRoles.ToArray();
                }
            }
            else
            {

            }
        }



        public bool isUserRoleOn(string ruserRole)
        {
            return (this.roles.Contains(ruserRole));
        }

    }

    public class JEusers
    {
        public JEuser[] allusers { get; set; }
    }

    public class JECompany
    {

        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string[] approvers { get; set; }
        public string[] departments { get; set; }
        public string homecurrency { get; set; }


        public JECompany()
        {

        }

        public JECompany(string rstrcompany, string rstrCompanyDataFile, string keyField)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrCompanyDataFile);
            if (xDoc != null)
            {
                var xcompany = xDoc.Root.Descendants("company").Where(x => x.Element(keyField).Value == rstrcompany).SingleOrDefault();
                string xid = xcompany.Element("id").Value;
                string xcode = xcompany.Element("code").Value;
                string xname = xcompany.Element("name").Value;
                string xaddress = xcompany.Element("address").Value;
                List<string> xapprovers = new List<string>();
                foreach (XElement approver in xcompany.Descendants("approver"))
                {
                    if (!"".Equals(approver.Value))
                    {
                        xapprovers.Add(approver.Value);
                    }
                }

                List<string> xdepartments = new List<string>();
                foreach (XElement department in xcompany.Descendants("department"))
                {
                    if (!"".Equals(department.Value))
                    {
                        xdepartments.Add(department.Value);
                    }
                }
                string xhomecurrency = xcompany.Element("homecurrency").Value;

                this.id = xid;
                this.code = xcode;
                this.name = xname;
                this.address = xaddress;
                this.approvers = xapprovers.ToArray();
                this.departments = xdepartments.ToArray();
                this.homecurrency = xhomecurrency;
            }
            else
            {

            }
        }


    }

    public class JELog
    {
        public int id { get; set; }
        public string date { get; set; }
        public string entry { get; set; }
        public string username { get; set; }
        public string severity { get; set; }

        public JELog()
        {

        }

        public JELog(string rstrJELogfile, string strUsername, string strEntry, string strDate, string strSeverity)
        {
            XDocument xDoc = new XDocument();
            int childrenCount;
            xDoc = XDocument.Load(rstrJELogfile);
            if (xDoc != null)
            {
                childrenCount = xDoc.Root.Elements().Count();
            }
            else
            {
                childrenCount = 1;
            }

            var jeLog = new JELog();

            jeLog.id = childrenCount + 1;
            jeLog.date = strDate;
            jeLog.username = strUsername;
            jeLog.entry = strEntry;
            jeLog.severity = strSeverity;

            XElement logEntry = new XElement("logentry");

            logEntry.Add(new XElement("id", jeLog.id));
            logEntry.Add(new XElement("date", jeLog.date));
            logEntry.Add(new XElement("entry", jeLog.entry));
            logEntry.Add(new XElement("username", jeLog.username));
            logEntry.Add(new XElement("severity", jeLog.severity));
            xDoc.Element("logentries").Add(logEntry);
            xDoc.Save(rstrJELogfile);
        }
        public void save()
        {

        }
    }

}
