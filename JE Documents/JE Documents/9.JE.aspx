<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="9.JE.aspx.cs" Inherits="JE_Documents._9_JE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>
            <asp:Button
                ID="btnGetJEDocs" runat="server"
                OnClick="btnGetJEDocs_Click"
                CssClass="btn btn-link"
                Text="by ID" />
        </li>
        <li>
            <asp:Button
                ID="btnGetJEDocsByCompany" runat="server"
                OnClick="btnGetJEDocsByCompany_Click"
                CssClass="btn btn-link"
                Text="by Company" />
        </li>
         <li>
            <asp:Button
                ID="btnGetJEDocsByStatus" runat="server"
                OnClick="btnGetJEDocsByStatus_Click"
                CssClass="btn btn-link"
                Text="by Company" />
        </li>
        <li>
            <asp:Button ID="btnAddNew" runat="server"
                OnClick="btnAddNew_Click"
                CssClass="btn btn-link"
                Text="Create new JE document" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataform" runat="server">
    <div id="JEDoc" runat="server" visible="false">
        <div class="contact_form">
            <ul id="liJEData" runat="server">
                <li>
                    <h2 id="titleJE" runat="server">...</h2>
                    <span class="required_notification">* Denotes Required Field</span>
                </li>
                <li>
                    <label for="txtID">Id:</label>
                    <asp:TextBox ID="txtID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    <span class="form_hint">JE ID</span>
                </li>
                <li>
                    <label for="ddlPeriod">Period:</label>
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="select"></asp:DropDownList>
                    <span class="form_hint">Period</span>
                </li>
                <li>
                    <label for="ddlType">Document type:</label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="select"></asp:DropDownList>
                    <span class="form_hint">Document type</span>
                </li>
                <li>
                    <label for="txtDate">Date:</label>
                    <div class='input-group date' id='datetimepicker1'>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <span class="input-group-addon btn" for="txtDate">
                            <span class="glyphicon glyphicon-calendar open-datetimepicker"></span>
                        </span>
                    </div>
                    <span class="form_hint">Date</span>
                </li>

                <li>
                    <label for="txtCompany">Company:</label>
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select" ValidateRequestMode="Disabled" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList><br />
                    <span class="form_hint">Company code</span>
                </li>
                <li style="display:none;">
                    <label for="txtCompany">Company code:</label>
                    <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    <span class="form_hint">Company code</span>
                </li>
                <li style="display:none;">
                    <label for="txtCompany">Company name:</label>
                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    <span class="form_hint">Company name</span>
                </li>
                <li>
                    <label for="txtDepartment">Department:</label>
                    <asp:DropDownList ID="ddlDepartment" CssClass="select" runat="server"></asp:DropDownList>
                    <span class="form_hint">Department</span>
                </li>
                <li>
                    <label for="txtReference">Reference:</label>
                    <asp:TextBox ID="txtReference" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Header text / reference</span>
                </li>
                <li>
                    <label for="txtHomeCurrency">Home currency:</label>
                    <asp:TextBox ID="txtHomeCurrency" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    <span class="form_hint">Company's home currency</span>
                </li>
                <li>
                    <label for="txtDate">Currency:</label>
                    <asp:DropDownList ID="ddlCurrency" CssClass="select" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" required AutoPostBack="True"></asp:DropDownList>
                    <span class="form_hint">Currency</span>
                </li>
                <li>
                    <label for="txtCurrencyRate">Currency rate:</label>
                    <asp:TextBox ID="txtCurrencyRate" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Currency rate</span>
                </li>
                <li>
                    <label for="txtInformation">Further information:</label>
                    <asp:TextBox ID="txtInformation" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Further information</span>
                </li>
            </ul>
            <ul>
                <li>Document processing history:<asp:LinkButton ID="hlToggleProcessingHistory" runat="server" OnClick="hlToggleProcessingHistory_Click">
                    <asp:Label ID="lblProcessingHistoryToggle" runat="server" Text="Label">>>></asp:Label>
                </asp:LinkButton></li>
                <div id="processingHistory_Latest" runat="server">
                    <asp:Literal ID="ltProcessingHistoryLatest" runat="server"></asp:Literal>
                </div>
                <div id="processingHistory_All" runat="server" visible="false">
                    <asp:Literal ID="ltProcessingHistoryAll" runat="server"></asp:Literal>
                </div>
            </ul>
        </div>
        <br />
        <!--Action-buttons for data -->
        <asp:Button ID="btnSave" runat="server"
            CssClass="btn stbutton acbutton"
            Text="Save"
            OnClick="btnSave_Click"
            PostBackUrl="~/9.JE.aspx" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn stbutton"
            OnClick="btnCancel_Click"
            Text="Cancel"
            formnovalidate="true"
            PostBackUrl="~/9.JE.aspx" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn stbutton"
            OnClick="btnDelete_Click"
            Text="Delete"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the JE Document ? ');"
            PostBackUrl="~/9.JE.aspx" />
    </div>

    <script type="text/javascript">
        $(function () {
            $('#<%=txtDate.ClientID %>').datetimepicker({
                format: 'DD.MM.YYYY',
                allowInputToggle: true
            });
            $('.open-datetimepicker').click(function (event) {
                event.preventDefault();
                $('#<%=txtDate.ClientID %>').focus();
            });
        });

    </script>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="datatable" runat="server">
    <div id="JEList" runat="server" visible="false">
        <h2>All JE Documents:
            <asp:Label ID="lblAllJEDocuments" runat="server" Text="..."></asp:Label></h2>
        <br />

        <table class="table table-striped ">
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
