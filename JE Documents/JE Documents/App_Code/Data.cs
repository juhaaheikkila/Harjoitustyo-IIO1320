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
    public class JEuser
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public string[] roles { get; set; }

        public JEuser()
        {

        }

        public JEuser(string rstrusername, string rstrUserDataFile)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(rstrUserDataFile);
            if (xDoc != null)
            {
                var xuser = xDoc.Root.Descendants("user").Where(x => x.Element("id").Value == rstrusername).SingleOrDefault();
                string xusername = xuser.Element("name").Value;
                string xuserdepartment = xuser.Element("department").Value;
                string xuseremail = xuser.Element("email").Value;
                List<string> xuserRoles = new List<string>();
                foreach (XElement xrole in xuser.Descendants("role"))
                {
                    xuserRoles.Add(xrole.Value);
                }
                this.username = rstrusername;
                this.fullname = xusername;
                this.department = xuserdepartment;
                this.email = xuseremail;
                this.roles = xuserRoles.ToArray();
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


}
