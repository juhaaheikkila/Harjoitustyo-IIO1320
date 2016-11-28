<%@ Page Title="" Language="C#" MasterPageFile="~/0.Site.Master" AutoEventWireup="true" CodeBehind="9.JE.aspx.cs" Inherits="JE_Documents._9_JE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigations" runat="server">
    <ul class="nav">
        <li>..</li>
        <li>..</li>
        <li>..</li>
        <li>
            <asp:Button
                ID="btnGetJEDocs" runat="server"
                OnClick="btnGetJEDocs_Click"
                CssClass="btn btn-link"
                Text="Display all JE documents" />
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
                    <asp:TextBox ID="txtID" runat="server" required></asp:TextBox>
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
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" required></asp:TextBox>
                        <span class="input-group-addon btn" for="txtDate">
                            <span class="glyphicon glyphicon-calendar open-datetimepicker"></span>
                        </span>
                    </div>
                    <span class="form_hint">Date</span>
                </li>

                <li>
                    <label for="txtCompany">Company code:</label>
                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Company code</span>
                </li>
                <li>
                    <label for="txtDepartment">Department:</label>
                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Department</span>
                </li>
                <li>
                    <label for="txtReference">Date:</label>
                    <asp:TextBox ID="txtReference" runat="server" CssClass="form-control" required></asp:TextBox>
                    <span class="form_hint">Header text / reference</span>
                </li>
                <li>
                    <label for="txtDate">Currency:</label>
                    <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control" required></asp:TextBox>
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
            <p>Document processing history:</p>
            <table class="table table-striped ">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>status</th>
                        <th>User</th>
                        <th>date</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>Draft</td>
                        <td>jheikkil</td>
                        <td>27.11.2016</td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>To be approved</td>
                        <td>jheikkil</td>
                        <td>27.11.2016</td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>Approved</td>
                        <td>jheikkil</td>
                        <td>27.11.2016</td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td>Transfered</td>
                        <td>jheikkil</td>
                        <td>27.11.2016</td>
                    </tr>
                </tbody>
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
