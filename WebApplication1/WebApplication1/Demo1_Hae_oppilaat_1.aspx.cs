using System;
using System.Data; //ADO.NETin kaikki data perusluokat ml DataTable
using JAMK.ICT.BL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Demo1_Hae_oppilaat_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnHae3Oppilasta_Click(object sender, EventArgs e)
    {
        DataTable dt = JAMK.ICT.Data.DBPlacebo.Get3TestStudents();

    }

    protected void btnHaeKaikki_Click(object sender, EventArgs e)
    {

    }

    protected void btnHae4OppilasOliota_Click(object sender, EventArgs e)
    {

    }
}