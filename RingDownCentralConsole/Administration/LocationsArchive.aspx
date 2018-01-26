<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationsArchive.aspx.cs" Inherits="RingDownCentralConsole.LocationsArchive" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link rel="stylesheet" type="text/css" href="rdccCSS.css" /> 
    <div  class="center-div">

      Locations Archive

    </div>


    <div>

        <p></p>

    </div>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />
        <div id = "dvGrid" style ="padding:10px;width:550px">


  
<asp:GridView ID="GridView1" runat="server"  Width = "800px" AutoGenerateColumns = "false" ForeColor="#333333" GridLines="None" 
 AllowPaging ="true"  ShowFooter = "false" OnPageIndexChanging = "OnPaging" PageSize = "100" EmptyDataText="No Archived Location Records"    OnSorting="GridView1_Sorting" AllowSorting="true">
     <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#FEFCFF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="False" ForeColor="Black" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                 <EmptyDataRowStyle Font-Bold="True" ForeColor="Black" BorderColor="Red" BorderWidth="2px" />
<Columns>
    <asp:TemplateField ItemStyle-Width = "50px" visible="false"   >
    <ItemTemplate>
        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>

     

    <asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Code" SortExpression="Code">
    <ItemTemplate>
        <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")%>'></asp:Label>
    </ItemTemplate>   
</asp:TemplateField>
    
<asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Name" SortExpression="Name"  >
     <ItemTemplate>
         <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
     </ItemTemplate>   
</asp:TemplateField>

    <asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Serial #" SortExpression="SerialNumber" >
     <ItemTemplate>
         <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
     </ItemTemplate>   
</asp:TemplateField>


<asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument = '<%# Eval("Id")%>' 
         OnClientClick = "return confirm('Would you like to make this record active?')" 
        Text = "Active" OnClick = "ActivateLocation" CausesValidation="false"></asp:LinkButton>
    </ItemTemplate> 
</asp:TemplateField>

      <asp:CommandField  ShowEditButton="False" />

</Columns>
<AlternatingRowStyle BackColor="white"  />
</asp:GridView>
            </ContentTemplate> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID = "GridView1" /> 
    </Triggers> 
    </asp:UpdatePanel> 


</div>

</asp:Content>
