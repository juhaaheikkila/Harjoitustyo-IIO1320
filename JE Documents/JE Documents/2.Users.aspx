<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="2.Users.aspx.cs" Inherits="JE_Documents._2_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="cntNavigation" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>
            <asp:Button
                ID="btnGetUsers" runat="server"
                OnClick="btnGetUsers_Click"
                CssClass="btn btn-link"
                Text="by id" />
        </li>
        <li>
            <asp:Button
                ID="btnGetUserByName" runat="server"
                OnClick="btnGetUserByName_Click"
                CssClass="btn btn-link"
                Text="by name" />
        </li>

        <li>
            <asp:Button ID="btnAddNew" runat="server"
                OnClick="btnAddNew_Click"
                CssClass="btn btn-link"
                Text="Create new user" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="cntForm" ContentPlaceHolderID="dataform" runat="server" Visible="false">
    <!-- New / Edit / Delete -->
    <div id="NewUser" style="margin-left:15px;" class="" runat="server" visible="false">
        <div class="contact_form">
            <ul id="liUserData" runat="server">
                <li>
                    <h2 id="titleUser" runat="server">New user</h2>
                    <span class="required_notification">* Denotes Required Field</span>
                </li>
                <li>
                    <label for="txtuserID">User id:</label>
                    <asp:TextBox ID="txtUserID" runat="server" required></asp:TextBox>
                    <span class="form_hint">User id</span>
                </li>
                <li>
                    <label for="txtUsername">User name:</label>
                    <asp:TextBox ID="txtUsername" runat="server" required></asp:TextBox>
                    <span class="form_hint">User shorname</span>
                </li>
                <li>
                    <label for="txtFirstname">Firstname:</label>
                    <asp:TextBox ID="txtFirstname" runat="server" required></asp:TextBox>
                    <span class="form_hint">User firstname</span>
                </li>
                <li>
                    <label for="txtLastname">Lastname:</label>
                    <asp:TextBox ID="txtLastname" runat="server" required></asp:TextBox>
                    <span class="form_hint">User lastname</span>
                </li>
                <li>
                    <label for="txtDepartment">Department:</label>
                    <asp:TextBox ID="txtDepartment" runat="server" required></asp:TextBox>
                    <span class="form_hint">Department</span>
                </li>
                <li>
                    <label for="txtEmail">Email:</label>
                    <asp:TextBox ID="txtEmail" runat="server" required></asp:TextBox>
                    <span class="form_hint">User's email address</span>

                </li>
                <li>
                    <label for="chkRoles">Roles:</label>
                    <asp:CheckBoxList ID="chkRoles" CssClass="w3-check" Style="width: 20px;" runat="server"></asp:CheckBoxList>
                    <span class="form_hint">User roles</span>
                </li>
            </ul>
        </div>
        <br />
        <!--Action-buttons for data -->

        <asp:Button ID="btnSave" runat="server"
            CssClass="btn stbutton acbutton"
            Text="Save"
            OnClick="btnSave_Click"
            PostBackUrl="~/2.Users.aspx" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn stbutton"
            OnClick="btnCancel_Click"
            Text="Cancel"
            formnovalidate="true"
            PostBackUrl="~/2.Users.aspx" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn stbutton ccbutton"
            OnClick="btnDelete_Click"
            Text="Delete"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the user ? ');"
            PostBackUrl="~/2.Users.aspx" />

    </div>
</asp:Content>

<asp:Content ID="cntTable" ContentPlaceHolderID="datatable" runat="server">
    <!-- Data list -->
    <div id="UserList" runat="server" visible="false">

        <h2 class="sub-header">All users:</h2>
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
