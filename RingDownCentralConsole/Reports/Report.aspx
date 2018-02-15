<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="RingDownCentralConsole.Reports.Report" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="/Content/date-input-polyfill.dist.js"></script>
    <script>
        $(document).ready(function () {
            $("#txtStartDate").attr('type', 'date');
            $("#txtEndDate").attr('type', 'date');
        });
    </script>

    <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />

    <div id="dvGrid">

        <div class="row">
            <div class="col-md-6">
                Start Date:<asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>

                <br class="col-md-12" />
            </div>
            <div class="col-md-6">
                End Date:
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>


                <br class="col-md-12" />
            </div>
            <div class="col-md-6" style="padding: 10px;">
                <asp:Button ID="btnFilter" CssClass="form-control btn-primary" runat="server" Text="Filter" OnClick="btnSearch_Click" />
            </div>
            <div class="col-md-6" style="padding: 10px;">
                <asp:Button ID="btnClear" runat="server" CssClass="form-control btn-warning" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
            </div>
        </div>
        <div class="row">
            <asp:Label ID="Msg2" runat="server" ForeColor="black" Font-Bold="true" />

            <br class="col-md-12" />
            <br class="col-md-12" />
        </div>

        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" AllowSorting="True" OnSorting="GridView1_Sorting" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Code" HeaderText="Location Code" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="Code" HeaderStyle-VerticalAlign="Middle">
                    <HeaderStyle Width="20%"></HeaderStyle>

                    <ItemStyle Wrap="True"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="LocationName" HeaderText="Location Name" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="LocationName" HeaderStyle-VerticalAlign="Middle">
                    <HeaderStyle Width="20%"></HeaderStyle>

                    <ItemStyle Wrap="True"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="Status" HeaderStyle-VerticalAlign="Middle">
                    <HeaderStyle Width="10%"></HeaderStyle>

                    <ItemStyle Wrap="True"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="RecordedDate" HeaderText="Recorded Date" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="RecordedDate" DataFormatString="{0:M/dd/yyyy hh:mm tt}" HeaderStyle-VerticalAlign="Middle">
                    <HeaderStyle Width="20%"></HeaderStyle>

                    <ItemStyle Wrap="True"></ItemStyle>
                </asp:BoundField>
            </Columns>

        </asp:GridView>
    </div>

    <div>
        <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" class="btn btn-success" OnClick="btnExcelExport_Click" Visible="false" />
    </div>
</asp:Content>
