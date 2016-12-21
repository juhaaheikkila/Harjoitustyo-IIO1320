<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo1_Hae_oppilaat_1.aspx.cs" Inherits="WebApplication1.Demo1_Hae_oppilaat_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Demo 1 Hae oppilaat LOO11</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Oppilaat syksy 2016</h1>
            <div id="buttoset">
                <asp:Button ID="btnHae3Oppilasta" runat="server" Text="Hae kolme oppislasta" OnClick="btnHae3Oppilasta_Click" />
                <asp:Button ID="btnHaeKaikki" runat="server" Text="Hae oppilaat tietokannasta" OnClick="btnHaeKaikki_Click" />
                <asp:Button ID="btnHae4OppilasOliota" runat="server" Text="Hae neljä oppilas oliota" OnClick="btnHae4OppilasOliota_Click" />
            </div>
            <div id="data">
                <asp:GridView ID="gvStudents" runat="server" />
            </div>
            <div id="footer">
                <asp:Label ID="lblMessages" runat="server" />
            </div>

        </div>
    </form>
</body>
</html>
