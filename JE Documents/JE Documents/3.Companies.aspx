<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="3.Companies.aspx.cs" Inherits="JE_Documents._02_Company" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="cntNavigation" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>
            <asp:Button
                ID="btnGetCompanies" runat="server"
                CssClass="btn btn-link"
                OnClick="btnGetCompanies_Click"
                Text="by id" />
        </li>
        <li>
            <asp:Button
                ID="btnGetCompaniesByName" runat="server"
                CssClass="btn btn-link"
                OnClick="btnGetCompaniesByName_Click"
                Text="by name" />
        </li>
        <li>
            <asp:Button
                ID="btnAddNew" runat="server"
                CssClass="btn btn-link"
                OnClick="btnAddNew_Click"
                Text="Create new company" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="cntForm" ContentPlaceHolderID="dataform" runat="server">
    <div id="NewCompany" style="margin-left: 15px;" class="" runat="server" visible="false">
        <div class="contact_form">
            <ul id="liCompanyData" runat="server">
                <li>
                    <h2 id="titleCompany" runat="server">New company</h2>
                </li>
                <li>
                    <label for="txtCompanyID">Id:</label>
                    <asp:TextBox ID="txtCompanyID" runat="server" Enabled="false"></asp:TextBox>

                </li>
                <li>
                    <label for="txttxtStatus">Status:</label>
                    <asp:TextBox ID="txtStatus" runat="server" Enabled="false"></asp:TextBox>

                </li>
                <li>
                    <label for="txtCompanyCode">Code:</label>
                    <asp:TextBox ID="txtCompanyCode" runat="server"></asp:TextBox>

                </li>
                <li>
                    <label for="txtCompanyName">Name:</label>
                    <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>

                </li>
                <li>
                    <label for="txtAddress">Address:</label>
                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>

                </li>
                <li>
                    <label for="txtApprovers">Approvers:</label>
                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="False" Style="height: 30px; width: 300px; padding: 5px 8px;" />
                    <asp:Button
                        ID="addApprover" runat="server"
                        CssClass="fa glyphicon-arrow-down"
                        Style="height: 30px; width: 45px; padding: 5px 8px;" Text="Add" formnovalidate="true" OnClick="addApprover_Click" />
                    <br />
                    <br />
                    <label for="txtApprovers">Selected Approvers:</label><br />
                    <asp:CheckBoxList ID="chkApprovers" runat="server"/>
                </li>
                <li>
                    <label for="txtDepartments">Departments:</label>
                    <asp:TextBox ID="txtDepartment" runat="server"></asp:TextBox>
                    <asp:Button ID="addDepartment" runat="server" Style="height: 30px; width: 45px; padding: 5px 8px;" Text="Add" formnovalidate="true" OnClick="addDepartment_Click" />
                    <br />
                    <br />
                    <label for="txtApprovers">Selected departments:</label><br />
                    <asp:CheckBoxList ID="chkDepartments" runat="server"/>
                </li>
                <li>
                    <label for="txtHomeCurrency">Home currency:</label>
                    <asp:DropDownList ID="ddlCurrency" CssClass="select" runat="server" Style="width: 120px;"></asp:DropDownList>
                </li>
            </ul>
        </div>
        <br />

        <!--Action-buttons -->
        <asp:Button
            ID="btnSave" runat="server"
            CssClass="btn stbutton acbutton"
            Text="Save"
            OnClick="btnSave_Click"
            PostBackUrl="~/3.Companies.aspx" />

        <asp:Button
            ID="btnCancel" runat="server"
            CssClass="btn stbutton"
            Text="Cancel"
            formnovalidate="true"
            OnClick="btnCancel_Click"
            PostBackUrl="~/3.Companies.aspx" />

        <asp:Button
            ID="btnDelete" runat="server"
            CssClass="btn stbutton ccbutton"
            Text="Delete"
            OnClick="btnDelete_Click"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the user ? ');"
            PostBackUrl="~/3.Companies.aspx" />

    </div>
</asp:Content>

<asp:Content ID="cntTable" ContentPlaceHolderID="datatable" runat="server">
    <!-- Data list -->
    <div id="CompanyList" runat="server" visible="false">
        <h2>All companies:</h2>
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
