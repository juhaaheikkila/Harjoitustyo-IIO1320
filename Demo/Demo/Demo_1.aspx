<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo_1.aspx.cs" Inherits="Demo.Demo_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hae oppilaat, syksy 2016</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Oppilaat syksy 2016</h1>
            <div>
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
        </div>
    </form>
</body>
</html>
