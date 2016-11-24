<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="3.Companies.aspx.cs" Inherits="JE_Documents._02_Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="w3-row">
        <div class="w3-third">
            <div id="divNavigation" class="w3-third" runat="server">

                <asp:Button
                    ID="btnGetCompanies" runat="server"
                    CssClass="w3-btn stbutton acbutton"
                    OnClick="btnGetCompanies_Click"
                    Text="Display all companies" />
                <br />
                <br />

                <asp:Button ID="btnAddNew" runat="server"
                    CssClass="w3-btn stbutton dcbutton"
                    OnClick="btnAddNew_Click"
                    Text="Create new company" />
                <br />

                <asp:DropDownList ID="ddlCompanies" runat="server"
                    CssClass="w3-btn stbutton dcbutton select"
                    OnSelectedIndexChanged="ddlCompanies_SelectedIndexChanged"
                    AutoPostBack="true" />

                <br />

            </div>
        </div>
        <div class="w3-twothird">
            <div id="NewCompany" runat="server" visible="false">
                <div class="contact_form">
                    <ul id="liCompanyData" runat="server">
                        <li>
                            <h2 id="titleCompany" runat="server">New company</h2>
                            <span class="required_notification">* Denotes Required Field</span>
                        </li>
                        <li>
                            <label for="txtCompanyID">Id:</label>
                            <asp:TextBox ID="txtCompanyID" runat="server" required></asp:TextBox>
                            <span class="form_hint">Company ID</span>
                        </li>
                        <li>
                            <label for="txtCompanyCode">Code:</label>
                            <asp:TextBox ID="txtCompanyCode" runat="server" required></asp:TextBox>
                            <span class="form_hint">Company code</span>
                        </li>
                        <li>
                            <label for="txtCompanyName">Name:</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" required></asp:TextBox>
                            <span class="form_hint">Company name</span>
                        </li>
                        <li>
                            <label for="txtAddress">Address:</label>
                            <asp:TextBox ID="txtAddress" runat="server" required></asp:TextBox>
                            <span class="form_hint">Address</span>
                        </li>
                        <li>
                            <label for="txtApprovers">Approvers:</label>
                            <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="True" Style="height: 30px; width: 300px; padding: 5px 8px;" />
                            <asp:Button ID="addApprover" runat="server" Style="height: 30px; width: 45px; padding: 5px 8px;" Text="Add" formnovalidate="true" OnClick="addApprover_Click" />
                            <span class="form_hint">Approvers</span>
                        </li>
                        <li>
                            <label for="txtApprovers">Selected Approvers:</label>
                            <asp:CheckBoxList ID="chkApprovers" runat="server" />
                            <span class="form_hint">Selected approvers</span>
                        </li>
                        <li>
                            <label for="txtDepartments">Departments:</label>
                            <asp:TextBox ID="txtDepartment" runat="server"></asp:TextBox>
                            <asp:Button ID="addDepartment" runat="server" Style="height: 30px; width: 45px; padding: 5px 8px;" Text="Add" formnovalidate="true" OnClick="addDepartment_Click" />
                            <span class="form_hint">Department codes</span>
                        </li>
                        <li>
                            <label for="txtApprovers">Selected departments:</label>
                            <asp:CheckBoxList ID="chkDepartments" runat="server" />
                            <span class="form_hint">Selected departments</span>

                        </li>
                        <li>
                            <label for="txtHomeCurrency">Home currency:</label>
                            <asp:TextBox ID="txtHomeCurrency" runat="server" required></asp:TextBox>
                            <span class="form_hint">Company home currency</span>
                        </li>
                    </ul>
                </div>
                <br />

                <!--Action-buttons -->
                <asp:Button ID="btnSave" runat="server"
                    CssClass="w3-btn stbutton acbutton"
                    Text="Save"
                    OnClick="btnSave_Click" />

                <asp:Button ID="btnCancel" runat="server"
                    CssClass="w3-btn stbutton w3-deep-orange"
                    Text="Cancel"
                    formnovalidate="true"
                    OnClick="btnCancel_Click" />

                <asp:Button ID="btnDelete" runat="server"
                    CssClass="w3-btn stbutton"
                    Text="Delete"
                    formnovalidate="true"
                    Visible="false"
                    OnClientClick="return confirm('Do you want to delete the user ? ');"
                    OnClick="btnDelete_Click" />

            </div>
            <div id="CompanyList" runat="server">
                <h2>All companies:
            <asp:Label ID="lblAllCompaniesXML" runat="server" Text="..."></asp:Label></h2>
                <br />
                <table class="w3-table listTable">
                    <asp:Literal ID="ltCompanies" runat="server"></asp:Literal>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
