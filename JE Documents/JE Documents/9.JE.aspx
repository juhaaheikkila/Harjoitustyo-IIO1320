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
                Text="by Status" />
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
                </li>
                <li>
                    <label for="txtID">Id:</label>
                    <asp:TextBox ID="txtID" runat="server" CssClass="" Enabled="false"></asp:TextBox>

                </li>
                <li>
                    <label for="txtStatus">Status:</label>
                    <asp:Label ID="lblStatus" runat="server" Text="" />
                </li>
                <li>
                    <label for="ddlPeriod">Period:</label>
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="select"></asp:DropDownList>

                </li>
                <li>
                    <label for="ddlType">Document type:</label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="select"></asp:DropDownList>

                </li>
                <li>
                    <label for="ddlType">Document number:</label>
                    <asp:TextBox ID="txtDocumentNumber" runat="server" CssClass="" Enabled="false"></asp:TextBox>

                </li>
                <li>
                    <label for="txtDate">Date:</label>
                    <div class='input-group date' id='datetimepicker1'>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <span class="input-group-addon btn" for="txtDate">
                            <span class="glyphicon glyphicon-calendar open-datetimepicker"></span>
                        </span>
                    </div>
                </li>
                <li>
                    <label for="txtAuthor">Author:</label>
                    <asp:TextBox ID="txtAuthor" runat="server" Enabled="false"></asp:TextBox>
                </li>

                <li>
                    <label for="txtCompany">Company:</label>
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select" ValidateRequestMode="Disabled" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList><br />
                </li>
                <li style="display: none;">
                    <label for="txtCompany">Company code:</label>
                    <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </li>
                <li style="display: none;">
                    <label for="txtCompanyName">Company name:</label>
                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </li>
                <li>
                    <label for="ddlDepartment">Department:</label>
                    <asp:DropDownList ID="ddlDepartment" CssClass="select" runat="server"></asp:DropDownList>
                </li>
                <li>
                    <label for="ddlDepartment">Approver:</label>
                    <asp:DropDownList ID="ddlApprover" CssClass="select" runat="server" OnSelectedIndexChanged="ddlApprover_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                </li>

                <li>
                    <label for="txtHeadertext">Reference:</label>
                    <asp:TextBox ID="txtHeadertext" runat="server" CssClass="form-control textarea" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </li>
                <li>
                    <label for="txtHomeCurrency">Home currency:</label>
                    <asp:TextBox ID="txtHomeCurrency" runat="server" CssClass="form-control" Enabled="false" Style="width: 120px;"></asp:TextBox>
                </li>

                <li>
                    <label for="txtDate">Currency:</label>
                    <asp:DropDownList ID="ddlCurrency" CssClass="select" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="True" Style="width: 120px;"></asp:DropDownList>
                </li>
                <li>
                    <label for="txtCurrencyRate">Currency rate:</label>
                    <asp:TextBox ID="txtCurrencyRate" runat="server" CssClass="form-control" Style="width: 120px;"></asp:TextBox>
                </li>
                <li>
                    <label for="txtInformation">Further information:</label>
                    <asp:TextBox ID="txtInformation" runat="server" CssClass="form-control textarea" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </li>


                <li>
                    <label for="lblProcessingHistory">Document processing history:</label>
                    <table>
                        <tr>
                            <th width="10%">id</th>
                            <th width="20%">status</th>
                            <th width="20%">username</th>
                            <th width="20%">date</th>
                            <th width="30%">message</th>
                        </tr>
                        <asp:Literal ID="ltProcessingHistoryAll" runat="server"></asp:Literal>
                    </table>
                </li>

            </ul>
        </div>

        <br />
        <label>Document data rows:</label>
        <div id="FileUploadControls" visible="false" runat="server">
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <!-- append / replace -->
            <asp:RadioButton ID="rbAppend" runat="server" Text="Append" GroupName="rbtnAppendReplace" Style="width: 120px;" /><br />
            <asp:RadioButton ID="rbReplace" runat="server" Text="Replace" GroupName="rbtnAppendReplace" Style="width: 120px;" />
            <asp:Button ID="btnImportCSV" Text="import data" runat="server" CssClass="btn btn-link" OnClick="btnImportCSV_Click" />

        </div>
        <table>
            <tr>
                <th width="5%">id</th>
                <th width="5%">company</th>
                <th width="5%">account</th>
                <th width="5%">d/c</th>
                <th width="5%">project</th>
                <th width="5%">dim1</th>
                <th width="5%">element</th>
                <th width="10%">total</th>
                <th width="5%">co</th>
                <th width="5%">tax</th>
                <th width="45%">reference</th>
            </tr>
            <asp:Literal ID="ltDataRows" runat="server"></asp:Literal>

            <tr>
                <td colspan="6">Credit total:
                </td>
                <td colspan="2" style="text-align: right;">
                    <asp:Label ID="lblCreditTotal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="6">Debit total:
                </td>
                <td colspan="2" style="text-align: right;">
                    <asp:Label ID="lblDebetTotal" runat="server" Text="" />

                </td>
            </tr>
            <tr>
                <td colspan="6">Difference:
                </td>
                <td colspan="2" style="text-align: right;">
                    <asp:Label ID="lblDifference" runat="server" Text="" />
                </td>
            </tr>

        </table>

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
            PostBackUrl="~/9.JE.aspx" />

        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn stbutton ccbutton"
            OnClick="btnDelete_Click"
            Text="Delete"
            formnovalidate="true"
            Visible="false"
            OnClientClick="return confirm('Do you want to delete the user ? ');"
            PostBackUrl="~/9.JE.aspx" />

        <br />
        <asp:Button ID="btnToBeApproved" runat="server"
            CssClass="btn stbutton ccbutton"
            Text="To be approved"
            OnClick="btnToBeApproved_Click"
            Visible="false"
            OnClientClick="return confirm('Do you want to send this JE Document to be approved? ');"
            PostBackUrl="~/9.JE.aspx" />
        <asp:Button ID="btnApprove" runat="server"
            CssClass="btn stbutton ccbutton"
            Text="Approve"
            OnClick="btnApprove_Click"
            Visible="false"
            OnClientClick="return confirm('Do you want to approve this JE Document? ');"
            PostBackUrl="~/9.JE.aspx" />
        <asp:Button ID="btnReject" runat="server"
            CssClass="btn stbutton ccbutton"
            Text="Reject"
            OnClick="btnReject_Click"
            Visible="false"
            OnClientClick="return confirm('Do you want reject this JE Document? ');"
            PostBackUrl="~/9.JE.aspx" />
        <asp:Button ID="btnTransfer" runat="server"
            CssClass="btn stbutton ccbutton"
            Text="Approve"
            OnClick="btnTransfer_Click"
            Visible="false"
            OnClientClick="return confirm('Do you want to transfer this JE Document? ');"
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
