<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="JE_Documents.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    head
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigations" runat="server">
    <div id="navigation" class="w3-third">
        <asp:Button
            ID="btnJEDocuments"
            runat="server"
            CssClass="w3-btn stbutton acbutton"
            OnClick="btnJEDocuments_Click"
            Text="JE Documents" />
        <br />
        <asp:HyperLink ID="hlUsers"
            runat="server"
            NavigateUrl="~/2.Users.aspx"
            Target="_blank"
            CssClass="w3-btn stbutton acbutton"
            Visible="false">
                    <span class="fa fa-user"></span> Open users
        </asp:HyperLink>
        <br />
        <asp:HyperLink ID="hlCompanies"
            runat="server"
            NavigateUrl="~/3.Companies.aspx"
            Target="_blank"
            CssClass="w3-btn stbutton acbutton"
            Visible="false">
                    <span class="fa fa-building"></span> Open companies
        </asp:HyperLink>
        <br />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    body
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
    footer
</asp:Content>
