<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="RingDownCentralConsole.Reports.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <webopt:bundlereference runat="server" path="~/Content/css/jQuery/theme" />    
    <script>
        $(document).ready(function () {
            $("#txtStartDate").attr('readonly', true).datepicker({ onSelect: function () { } })
            $("#txtEndDate").attr('readonly', true).datepicker({ onSelect: function () { } });
        });    

        $(function () {
            $("#<%= txtStartDate.ClientID %>").datepicker();
        });
    </script>

    <div class="center-div">
        Report
    </div>
    <div>
        <p></p>
    </div>

    <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />
    <div id="dvGrid" style="padding: 10px; width: 550px">

        <div>
            <div>
                Start Date:<asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ReqStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="*" ForeColor="Red" EnableClientScript="false">
                </asp:RequiredFieldValidator>
                <br />
                <br />
                End Date:
           <asp:TextBox ID="txtEndDate" runat="server"  CssClass="form-control" ClientIDMode="Static" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="ReqEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="*" ForeColor="Red" EnableClientScript="false">
                </asp:RequiredFieldValidator>
                <br />
                <br />
                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnSearch_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
                <br />
                <br />
                <asp:Label ID="Msg2" runat="server" ForeColor="black" Font-Bold="true" /><br />
            </div>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-striped" AutoGenerateColumns="False" AllowSorting="True" OnSorting="GridView1_Sorting" GridLines="None">
                <RowStyle CssClass="cursor-pointer" />
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
                    <asp:BoundField DataField="RecordedDate" HeaderText="Recorded Date" HeaderStyle-Width="25%" ItemStyle-Wrap="true" SortExpression="RecordedDate" DataFormatString="{0:M/dd/yyyy}" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle Width="20%"></HeaderStyle>

                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:BoundField>
                </Columns>

            </asp:GridView>
        </div>
    </div>
</asp:Content>
