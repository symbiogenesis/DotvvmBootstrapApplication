<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="RingDownCentralConsole.Reports.Report" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <link rel="stylesheet" type="text/css" href="../Css/rdccCSS.css" /> 
     <script>
         $(function () {
             $("#<%= txtStartDate.ClientID %>").datepicker();
         });
     </script>

    <div  class="center-div">
      Report
    </div>
    <div>        <p></p>    </div>


    
    <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />
        <div id = "dvGrid" style ="padding:10px;width:550px">
   
  

   <div>
       <div>
           Start Date:<asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static"   ></asp:TextBox>
            <asp:RequiredFieldValidator ID="ReqStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="*" ForeColor="Red" EnableClientScript="false">
         </asp:RequiredFieldValidator>
       <br />
           <br />
           End Date:
           <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static"  ></asp:TextBox>
           <asp:RequiredFieldValidator ID="ReqEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="*" ForeColor="Red" EnableClientScript="false">
         </asp:RequiredFieldValidator>
           <br />
           <br />
           <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnSearch_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
           <br />
           <br />
            <asp:Label id="Msg2" runat="server" ForeColor="black" Font-Bold="true" /><br />
       </div>
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="GridView1_Sorting" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" width="600px">
    
              <AlternatingRowStyle BackColor="White" ForeColor="#000000"  />
  
           <RowStyle HorizontalAlign="Center" />
              <Columns>

            

                        <asp:BoundField DataField="Code" HeaderText="Location Code" HeaderStyle-Width="20%" ItemStyle-Wrap="true" SortExpression="Code"  HeaderStyle-VerticalAlign="Middle">
<HeaderStyle Width="20%"></HeaderStyle>

<ItemStyle Wrap="True"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="LocationName" HeaderText="Location Name" HeaderStyle-Width="20%" ItemStyle-Wrap="true" SortExpression="LocationName" HeaderStyle-VerticalAlign="Middle">
<HeaderStyle Width="20%"></HeaderStyle>

<ItemStyle Wrap="True"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="10%" ItemStyle-Wrap="true" SortExpression="Status" HeaderStyle-VerticalAlign="Middle">                        
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle Wrap="True"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="RecordedDate" HeaderText="Recorded Date" HeaderStyle-Width="20%" ItemStyle-Wrap="true" SortExpression="RecordedDate" DataFormatString="{0:M/dd/yyyy}" HeaderStyle-VerticalAlign="Middle">
<HeaderStyle Width="20%"></HeaderStyle>

<ItemStyle Wrap="True"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
          
              <EditRowStyle BackColor="#999999" />
              <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
              <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
              <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
              <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
              <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#000000" />
              <SortedAscendingCellStyle BackColor="#E9E7E2" />
              <SortedAscendingHeaderStyle BackColor="#506C8C" />
              <SortedDescendingCellStyle BackColor="#FFFDF8" />
              <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
          
       </asp:GridView>
   </div>
            </div>
</asp:Content>
