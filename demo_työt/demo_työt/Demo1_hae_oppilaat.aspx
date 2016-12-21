<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo1_hae_oppilaat.aspx.cs" Inherits="demo_työt.Demo1_hae_oppilaat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hae oppilaat</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Oppilaat syksy 2016</h1>
            <div id="buttoset">
                <asp:Button ID="btnGet3" runat="server" Text="Hae kolme oppilasta" OnClick="btnGet3_Click" />

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
