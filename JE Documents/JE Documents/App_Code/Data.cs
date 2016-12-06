using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JE_Documents.Data
{
    public class CSVReader
    {
        //
        private Stream objStream;
        private StreamReader objReader;
        public static string CSVSeparator = ";";
        public static string CSVDesimalToBe = ",";

        //add name space System.IO.Stream
        public CSVReader(Stream filestream) : this(filestream, null) { }

        public CSVReader(Stream filestream, Encoding enc)
        {
            this.objStream = filestream;
            //check the Pass Stream whether it is readable or not
            if (!filestream.CanRead)
            {
                return;
            }
            objReader = (enc != null) ? new StreamReader(filestream, enc) : new StreamReader(filestream);
        }
        //parse the Line
        public string[] GetCSVLine()
        {
            string data = objReader.ReadLine();
            if (data == null) return null;
            if (data.Length == 0) return new string[0];
            //System.Collection.Generic
            ArrayList result = new ArrayList();
            //parsing CSV Data
            ParseCSVData(result, data);
            return (string[])result.ToArray(typeof(string));
        }

        private void ParseCSVData(ArrayList result, string data)
        {
            int position = -1;
            while (position < data.Length)
                result.Add(ParseCSVField(ref data, ref position));
        }

        private string ParseCSVField(ref string data, ref int StartSeperatorPos)
        {
            if (StartSeperatorPos == data.Length - 1)
            {
                StartSeperatorPos++;
                return "";
            }

            int fromPos = StartSeperatorPos + 1;
            if (data[fromPos] == '"')
            {
                int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                int lines = 1;
                while (nextSingleQuote == -1)
                {
                    data = data + "\n" + objReader.ReadLine();
                    nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                    lines++;
                    if (lines > 20)
                        throw new Exception("lines overflow: " + data);
                }
                StartSeperatorPos = nextSingleQuote + 1;
                string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
                tempString = tempString.Replace("'", "''");
                return tempString.Replace("\"\"", "\"");
            }

            //int nextComma = data.IndexOf(';', fromPos);
            int nextComma = data.IndexOf(CSVSeparator, fromPos);
            if (nextComma == -1)
            {
                StartSeperatorPos = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                StartSeperatorPos = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }

        private int GetSingleQuote(string data, int SFrom)
        {
            int i = SFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    else
                        return i;
                }
            return -1;
        }
    }

    public static class CommonCodes
    {
        public static string gUsername;
        public static string gUserDatafile;
        public static string gCompanyDatafile;
        public static string gJEDocDatafile;
        public static string gLogDatafile;
        public static string gVATCodeDatafile;
        public static JEDoc gJEDoc;
        public const string STATUS_DRAFT = "draft";
        public const string STATUS_TO_BE_APPROVED = "to be approved";
        public const string STATUS_APPROVED = "approved";
        public const string STATUS_TRANFERED = "transfered";
        public const string STATUS_DELETED = "deleted";


        public static JELogHelper gLog; //= new JELogHelper(gLogDatafile, gUserDatafile, gUsername);
        public static JEuser gUser;

        public static int getCount(string strDataFile)
        {
            int intCounter;
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(strDataFile);
            if (xDoc != null)
            {
                intCounter = xDoc.Root.Elements().Count() + 1;
            }
            else
            {
                intCounter = 0;
            }
            return (intCounter);
        }

        public static void initialize()
        {
            CommonCodes.gUsername = System.Configuration.ConfigurationManager.AppSettings["Username"];

            CommonCodes.gUserDatafile = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserDataFile"]);
            CommonCodes.gCompanyDatafile = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CompanyDataFile"]);
            CommonCodes.gJEDocDatafile = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["JEDataFile"]);
            CommonCodes.gLogDatafile = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["LogFile"]);
            CommonCodes.gVATCodeDatafile = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["VATCodeFile"]);
            
            CommonCodes.gUser = new JEuser(CommonCodes.gUsername, CommonCodes.gUserDatafile, "username");
            CommonCodes.gLog = new JELogHelper(CommonCodes.gLogDatafile, CommonCodes.gUserDatafile, CommonCodes.gUsername);
        }

    }

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
        public string approver { get; set; }
        public string department { get; set; }
        public string headertext { get; set; }
        public string homecurrency { get; set; }
        public string currency { get; set; }
        public double currencyrate { get; set; }
        public string information { get; set; }
        public List<JEDocRow> rows { get; set; }
        public List<JEDocStatus> processinghistory { get; set; }
        public double debettotal { get; set; }
        public double credittotal { get; set; }
        public string dataok { get; set; }


        public JEDoc()
        {

        }

        public JEDoc(string rstrID, string rstrJEDatafile, string rstrKeyField)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrJEDatafile);
            if (xDoc != null)
            {
                var xjedoc = xDoc.Root.Descendants("jedoc").Where(x => x.Element(rstrKeyField).Value == rstrID).SingleOrDefault();
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
                    string xapprover = xjedoc.Element("approver").Value;
                    string xdepartment = xjedoc.Element("department").Value;
                    string xheadertext = xjedoc.Element("headertext").Value;
                    string xhomecurrency = xjedoc.Element("homecurrency").Value;
                    string xcurrency = xjedoc.Element("currency").Value;
                    string xcurrencyrate = xjedoc.Element("currencyrate").Value;
                    string xinformation = xjedoc.Element("information").Value;
                    string xdebettotal = xjedoc.Element("debettotal").Value.Replace(".", CSVReader.CSVDesimalToBe);
                    string xcredittotal = xjedoc.Element("credittotal").Value.Replace(".", CSVReader.CSVDesimalToBe);
                    string xdataok = xjedoc.Element("dataok").Value;

                    foreach (XElement xrow in xjedoc.Descendants("jerow"))
                    {
                        JEDocRow docRow = new JEDocRow();
                        docRow.id = xrow.Element("id").Value;
                        docRow.company = xrow.Element("company").Value;
                        docRow.account = xrow.Element("account").Value;
                        docRow.debetcredit = xrow.Element("debetcredit").Value;
                        docRow.project = xrow.Element("project").Value;
                        docRow.dim1 = xrow.Element("dim1").Value;
                        docRow.element = xrow.Element("element").Value;
                        if ("".Equals(xrow.Element("total").Value))
                        {
                            docRow.total = 0;
                        }
                        else
                        {
                            docRow.total = Convert.ToDouble(xrow.Element("total").Value.Replace(".", CSVReader.CSVDesimalToBe));
                        }
                        docRow.country = xrow.Element("country").Value;
                        docRow.vatcode = xrow.Element("vatcode").Value;
                        docRow.reference = xrow.Element("reference").Value;
                        if (rows == null)
                        {
                            rows = new List<JEDocRow>();
                        }
                        rows.Add(docRow);
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
                    this.approver = xapprover;
                    this.department = xdepartment;
                    this.headertext = xheadertext;
                    this.homecurrency = xhomecurrency;
                    this.currency = xcurrency;
                    if ("".Equals(xcurrencyrate))
                    {
                        this.currencyrate = 0;
                    }
                    else
                    {
                        this.currencyrate = Convert.ToDouble(xcurrencyrate);
                    }
                    this.information = xinformation;
                    this.debettotal = Convert.ToDouble(xdebettotal);
                    this.credittotal = Convert.ToDouble(xcredittotal);
                    this.dataok = xdataok;

                    //statuses
                    foreach (XElement status in xjedoc.Descendants("jestatus"))
                    {
                        JEDocStatus docStatus = new JEDocStatus();
                        docStatus.id = status.Element("id").Value;
                        docStatus.status = status.Element("status").Value;
                        docStatus.username = status.Element("username").Value;
                        docStatus.date = status.Element("date").Value;
                        docStatus.message = status.Element("message").Value;
                        if (processinghistory == null)
                        {
                            processinghistory = new List<JEDocStatus>();
                        }
                        processinghistory.Add(docStatus);
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
        public string message { get; set; }

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
        public string status { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public string[] roles { get; set; }
        public string dataok { get; set; }

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
                    string xstatus = xuser.Element("status").Value;
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
                    string xdataok = xuser.Element("dataok").Value;

                    this.id = xuserid;
                    this.status = xstatus;
                    this.username = xusername;
                    this.firstname = xfirstname;
                    this.lastname = xlastname;
                    this.department = xuserdepartment;
                    this.email = xuseremail;
                    this.roles = xuserRoles.ToArray();
                    this.dataok = xdataok;
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
        public string status { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string[] approvers { get; set; }
        public string[] departments { get; set; }
        public string homecurrency { get; set; }
        public string dataok { get; set; }

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
                string xstatus = xcompany.Element("status").Value;
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
                string xdataok = xcompany.Element("dataok").Value;
                this.id = xid;
                this.status = xstatus;
                this.code = xcode;
                this.name = xname;
                this.address = xaddress;
                this.approvers = xapprovers.ToArray();
                this.departments = xdepartments.ToArray();
                this.homecurrency = xhomecurrency;
                this.dataok = xdataok;
            }
            else
            {

            }
        }


    }

    public class JELogHelper
    {
        private const int SEVERITY_ERROR = 9;
        private const int SEVERITY_LOW = 0;
        private JEuser user;
        private XDocument xdoc;
        private string strJELogFile;

        public JELogHelper(string rstrJELogFile, string rstrJEUsers, string rstrUsername)
        {

            xdoc = new XDocument();
            xdoc = XDocument.Load(rstrJELogFile);
            user = new JEuser(rstrUsername, rstrJEUsers, "username");
            this.strJELogFile = rstrJELogFile;

        }

        public void logEvent(string vstrLogEntry)
        {
            JELog logging = new JELog(xdoc, user.username, vstrLogEntry, SEVERITY_LOW.ToString(), this.strJELogFile);
        }

        public void logError(string vstrLogEntry)
        {
            JELog logging = new JELog(xdoc, user.username, vstrLogEntry, SEVERITY_ERROR.ToString(), this.strJELogFile);
        }

        public void logError(string vstrLogEntry, Exception ex)
        {
            JELog logging = new JELog(xdoc, user.username, vstrLogEntry + ", " + ex.Message + ": " + ex.StackTrace, SEVERITY_ERROR.ToString(), this.strJELogFile);
        }

    }

    public class JELog
    {
        public int id { get; set; }
        public string date { get; set; }
        public string entry { get; set; }
        public string username { get; set; }
        public string severity { get; set; }


        public JELog(XDocument xdoc, string username, string logentry, string strSeverity, string rstrJELogfile)
        {
            int childrenCount = getCount(xdoc);
            var jeLog = new JELog();
            jeLog.id = childrenCount + 1;
            jeLog.date = DateTime.Now.ToString();
            jeLog.username = username;
            jeLog.entry = logentry;
            jeLog.severity = strSeverity;

            XElement logEntry = new XElement("logentry");

            logEntry.Add(new XElement("id", jeLog.id));
            logEntry.Add(new XElement("date", jeLog.date));
            logEntry.Add(new XElement("entry", jeLog.entry));
            logEntry.Add(new XElement("username", jeLog.username));
            logEntry.Add(new XElement("severity", jeLog.severity));
            xdoc.Element("logentries").Add(logEntry);
            xdoc.Save(rstrJELogfile);

        }

        private int getCount(XDocument xdoc)
        {
            int childrenCount;

            if (xdoc != null)
            {
                childrenCount = xdoc.Root.Elements().Count();
            }
            else
            {
                childrenCount = 0;
            }
            return childrenCount;
        }

        public JELog()
        {

        }

        public JELog(string rstrJELogfile, string strUsername, string strEntry, string strDate, string strSeverity)
        {
            XDocument xDoc = new XDocument();

            xDoc = XDocument.Load(rstrJELogfile);
            int childrenCount = getCount(xDoc);

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

    }

    public class JEVATCode
    {
        public string id { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string percentage { get; set; }
        public string inputoutput { get; set; }
        public string dataok { get; set; }

        public JEVATCode()
        {

        }

        public JEVATCode(string rstrvatcode, string rstrVATCodeDataFile, string rstrKeyField)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrVATCodeDataFile);
            if (xDoc != null)
            {
                var xvatcode = xDoc.Root.Descendants("vatcode").Where(x => x.Element(rstrKeyField).Value == rstrvatcode).SingleOrDefault();
                if (xvatcode != null)
                {
                    string xid = xvatcode.Element("id").Value;
                    string xstatus = xvatcode.Element("status").Value;
                    string xcode = xvatcode.Element("code").Value;
                    string xdescription = xvatcode.Element("description").Value;
                    string xpercentage = xvatcode.Element("percentage").Value;
                    string xinputoutput = xvatcode.Element("inputoutput").Value;
                    string xdataok = xvatcode.Element("dataok").Value;

                    this.id = xid;
                    this.status = xstatus;
                    this.code = xcode;
                    this.description = xdescription;
                    this.percentage = xpercentage;
                    this.inputoutput = xinputoutput;
                    this.dataok = xdataok;
                }
            }
            else
            {

            }
        }

    }

}
