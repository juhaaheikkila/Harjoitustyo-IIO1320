<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="5.VAT_Codes.aspx.cs" Inherits="JE_Documents._5_VAT_Codes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>
            <asp:Button
                ID="btnGetVATCodes" runat="server"
                CssClass="btn btn-link"
                OnClick="btnGetVATCodes_Click"
                Text="by id" />
        </li>

        <li>
            <asp:Button
                ID="btnAddNew" runat="server"
                CssClass="btn btn-link"
                OnClick="btnAddNew_Click"
                Text="Create new tax code" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataform" runat="server">
    <!-- New / Edit / Delete -->
    <div id="NewVAT" style="margin-left: 15px;" class="" runat="server" visible="false">
        <div class="contact_form">
            <ul id="liUserData" runat="server">
                <li>
                    <h2 id="titleUser" runat="server">New VAT Code</h2>
                </li>
                <li>
                    <label for="txtID">Id:</label>
                    <asp:TextBox ID="txtID" runat="server" Enabled="false"></asp:TextBox>
                </li>
                <li>
                    <label for="txtID">Status:</label>
                    <asp:TextBox ID="txtStatus" runat="server" Enabled="false"></asp:TextBox>
                </li>

                <li>
                    <label for="txtVATCode">Tax code:</label>
                    <asp:TextBox ID="txtVATCode" runat="server" required></asp:TextBox>
                </li>
                <li>
                    <label for="txtDescription">Description:</label>
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                </li>
                <li>
                    <label for="txtVATPercentage">%:</label>
                    <asp:TextBox ID="txtVATPercentage" runat="server"></asp:TextBox>
                </li>
                <li>
                    <!-- input / output -->
                    <asp:RadioButton ID="rbInput" runat="server" Text="Input" GroupName="rbtnInputOutut" Style="width: 120px;" /><br />
                    <asp:RadioButton ID="rbOutput" runat="server" Text="Output" GroupName="rbtnInputOutut" Style="width: 120px;" />
                </li>

            </ul>
        </div>
        <br />
        <!--Action-buttons for data -->

        <asp:Button ID="btnSave" runat="server"
            CssClass="btn stbutton acbutton"
            Text="Save"
            OnClick="btnSave_Click"
            PostBackUrl="~/5.VAT_Codes.aspx" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn stbutton"
            OnClick="btnCancel_Click"
            Text="Cancel"
            formnovalidate="true"
            PostBackUrl="~/5.VAT_Codes.aspx" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn stbutton ccbutton"
            OnClick="btnDelete_Click"
            Text="Delete"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the code ? ');"
            PostBackUrl="~/5.VAT_Codes.aspx" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="datatable" runat="server">
    <!-- Data list -->
    <div id="VATList" runat="server" visible="false">

        <h2 class="sub-header">All VAT codes:</h2>
        <table class="table table-striped">
            <thead>
                <asp:Literal ID="ltTableHead" runat="server"></asp:Literal>
            </thead>
            <tbody>
                <asp:Literal ID="ltTableData" runat="server"></asp:Literal>
            </tbody>
        </table>
        <!-- listaus XMLData sourcella, ei päivity uutta lisättäessa ilman refresh
        <p>Tadaa, sama listaus käyttäen XMLDataSourcea</p>
        
        <asp:XmlDataSource ID="srcVatCodes" runat="server" DataFile="~/App_Data/VATCodes.xml" XPath="/vatcodes/vatcode[status!='deleted']"></asp:XmlDataSource>
        <asp:Repeater ID="Repeater1" DataSourceID="srcVatCodes" runat="server">
            <HeaderTemplate>
                <table class="table table-striped">
                    <thead>
                        <th>ID</th>
                        <th>Code</th>
                        <th>Description</th>
                        <th>%</th>
                        <th>Input/Output</th>
                        <th></th>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="listRow">
                    <td><a href="5.VAT_Codes.aspx?id=<%# XPath ("id") %>"><%# XPath ("id") %></a></td>
                    <td><%# XPath ("code") %></td>
                    <td><%# XPath ("description") %></td>
                    <td><%# XPath ("percentage") %></td>
                    <td><%# XPath ("inputoutput") %></td>
                    <td><%# "".Equals(XPath("dataok")) ? "" : "<span title='" + XPath("dataok") + "' style='color:red'><b>X</b></span>" %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
            -->
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
