<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="3.Companies.aspx.cs" Inherits="JE_Documents._02_Company" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="cntNavigation" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>..</li>
        <li>..</li>
        <li>..</li>
        <li>
            <asp:Button
                ID="btnGetCompanies"
                runat="server"
                CssClass="btn btn-link"
                OnClick="btnGetCompanies_Click"
                Text="Display all companies" />
        </li>
        <li>
            <asp:Button
                ID="btnAddNew"
                runat="server"
                CssClass="btn btn-link"
                OnClick="btnAddNew_Click"
                Text="Create new company" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="cntForm" ContentPlaceHolderID="dataform" runat="server">
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
                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="False" Style="height: 30px; width: 300px; padding: 5px 8px;" />
                    <asp:Button 
                        ID="addApprover" runat="server" 
                        CssClass="fa glyphicon-arrow-down"
                        Style="height: 30px; width: 45px; padding: 5px 8px;" Text="Add" formnovalidate="true" OnClick="addApprover_Click" />
                        
                    
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
            CssClass="btn stbutton acbutton"
            Text="Save"
            OnClick="btnSave_Click"
            PostBackUrl="~/3.Companies.aspx" />

        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn stbutton"
            Text="Cancel"
            formnovalidate="true"
            OnClick="btnCancel_Click"
            PostBackUrl="~/3.Companies.aspx" />

        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn stbutton"
            Text="Delete"
            OnClick="btnDelete_Click"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the user ? ');"
            PostBackUrl="~/3.Companies.aspx" />

    </div>
</asp:Content>

<asp:Content ID="cntTable" ContentPlaceHolderID="datatable" runat="server">
    <div id="CompanyList" runat="server" visible="false">
        <h2>All companies:
            <asp:Label ID="lblAllCompaniesXML" runat="server" Text="..."></asp:Label></h2>
        <br />

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

<asp:Content ID="cntFooter" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
