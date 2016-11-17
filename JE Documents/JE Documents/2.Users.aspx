<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="2.Users.aspx.cs" Inherits="JE_Documents._2_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="w3-row">
        <div class="w3-third">
            <div id="divNavigation" class="w3-third" runat="server">

                <asp:Button
                    ID="btnGetUsers" runat="server"
                    CssClass="w3-btn stbutton acbutton"
                    OnClick="btnGetUsers_Click"
                    Text="Display all users" />
                <br />
                <br />

                <asp:Button ID="btnAddNew" runat="server"
                    CssClass="w3-btn stbutton dcbutton"
                    OnClick="btnAddNew_Click"
                    Text="Create new user" />
                <br />

                <asp:Button ID="btnModify" runat="server"
                    CssClass="w3-btn stbutton dcbutton"
                    OnClick="btnModify_Click"
                    Text="Edit users" />
                <br />

                <asp:Button ID="btnCloseUser" runat="server"
                    CssClass="w3-btn stbutton dcbutton"
                    OnClick="btnCloseUser_Click"
                    Text="Close" />
                <br />

            </div>
        </div>
        <div class="w3-twothird">

            <div id="NewUser" runat="server" visible="false">
                <div class="contact_form">
                    <ul>
                        <li>
                            <h2 id="titleUser" runat="server">New user</h2>
                            <span class="required_notification">* Denotes Required Field</span>
                        </li>
                        <li>
                            <label for="txtuserID">User id:</label>
                            <asp:TextBox ID="txtUserID" runat="server" required Visible="False"></asp:TextBox>
                            <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" Style="height: 30px; width: 300px; padding: 5px 8px;" />
                            <span class="form_hint">User shortname</span>
                        </li>
                        <li>
                            <label for="txtUsernane">User name:</label>
                            <asp:TextBox ID="txtUsername" runat="server" required></asp:TextBox>
                            <span class="form_hint">User fullname</span>
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
                        <label for="chkRoles">Roles:</label>
                        <asp:CheckBoxList ID="chkRoles" CssClass="w3-check" Style="width: 20px;" runat="server"></asp:CheckBoxList>
                        <span class="form_hint">User roles</span>
                    </ul>
                </div>

                <!--Action-buttons -->

                <br />

                <asp:Button ID="btnCustomerSave" runat="server"
                    CssClass="w3-btn stbutton acbutton"
                    Text="Save"
                    OnClick="btnCustomerSave_Click" />

                <asp:Button ID="btnCustomerCancel" runat="server"
                    CssClass="w3-btn stbutton w3-deep-orange"
                    OnClick="btnCustomerCancel_Click"
                    Text="Cancel"
                    formnovalidate="true" />

                <br />
                <asp:Button ID="btnUserDelete" runat="server"
                    CssClass="w3-btn stbutton"
                    OnClick="btnUserDelete_Click"
                    Text="Delete"
                    formnovalidate="true"
                    Visible="false"
                    OnClientClick="return confirm('Do you want to delete the user ? ');" />


            </div>

            <div id="UserList" runat="server">
                <h2>All users:
                    <asp:Label ID="lblAllUsersXML" runat="server" Text="..."></asp:Label></h2>
                <br />
                <br />
                <asp:GridView ID="gvUsers" runat="server"></asp:GridView>
                <table class="w3-table listTable">
                    <asp:Literal ID="ltUsers" runat="server"></asp:Literal>
                </table>

            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
