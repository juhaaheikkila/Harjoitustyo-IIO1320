<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="4.Logs.aspx.cs" Inherits="JE_Documents._3_Logs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>
            <asp:Button
                ID="btnLogsById" runat="server"
                CssClass="btn btn-link"
                Text="by id"
                OnClick="btnLogsById_Click" />
        </li>
        <li>
            <asp:Button
                ID="btnLogsByDate" runat="server"
                CssClass="btn btn-link"
                Text="by date"
                OnClick="btnLogsByDate_Click" />
        </li>

        <li>
            <asp:Button ID="btnLogsByUser" runat="server"
                CssClass="btn btn-link"
                Text="by user"
                OnClick="btnLogsByUser_Click" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataform" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="datatable" runat="server">
    <!-- Data list -->
    <div id="LogList" runat="server" visible="false">

        <h2 class="sub-header">All logs:</h2>
        <table class="table table-striped">
            <thead>
                <asp:Literal ID="ltTableHead" runat="server"></asp:Literal>
            </thead>
            <tbody>
                <asp:Literal ID="ltTableData" runat="server"></asp:Literal>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
