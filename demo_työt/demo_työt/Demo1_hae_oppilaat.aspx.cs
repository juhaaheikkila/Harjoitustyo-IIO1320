using System;
using System.Data; //ADO.NETin kaikki data perusluokat ml DataTable

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace demo_työt
{
    public partial class Demo1_hae_oppilaat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGet3_Click(object sender, EventArgs e)
        {
            DataTable dt = JAMK.ICT.Data.DBPlacebo.Get3TestStudents();

            gvStudents.DataSource = dt;
            gvStudents.DataBind();
        }
    }
}