<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="1. Index.aspx.cs" Inherits="JE_Documents.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="w3-row">
        <div id="navigation" class="w3-third">
            <asp:Button
                ID="btnJEDocuments"
                runat="server"
                CssClass="w3-btn stbutton acbutton"
                OnClick="btnJEDocuments_Click"
                Text="JE Documents" />
            <br />
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
        <div class="w3-twothird">
            <h1>JE Documents</h1>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
