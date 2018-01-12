<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminStations.aspx.cs" Inherits="RingDownCentralConsole.AdminStations" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .center-div
        {
          text-align: center;
          font-size: large;
          font-weight: bold;
        }
        .auto-style1 {
            height: 24px;
        }
        th
    {
        text-align:center;
    }
    
    </style>
      

    <div  class="center-div">

      Stations

    </div>


    <div>

        <p></p>

    </div>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />
        <div id = "dvGrid" style ="padding:10px;width:550px">


  
<asp:GridView ID="GridView1" runat="server"  Width = "550px" AutoGenerateColumns = "false" Font-Names = "Arial" Font-Size = "11pt" 
AlternatingRowStyle-BackColor = "white" HeaderStyle-BackColor = "#507CD1" AllowPaging ="true"  ShowFooter = "true" 
OnPageIndexChanging = "OnPaging" onrowediting="EditStation" onrowupdating="UpdateStation"  onrowcancelingedit="CancelEdit"
PageSize = "100" EmptyDataText="No Station Records Entered" >
    
<Columns>
    <asp:TemplateField ItemStyle-Width = "50px" visible="false"  >
    <ItemTemplate>
        <asp:Label ID="lblStationID" runat="server" Text='<%# Eval("StationID")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>

     

    <asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location ID">
    <ItemTemplate>
        <asp:Label ID="lblLocID" runat="server" Text='<%# Eval("LocID")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtLocID" runat="server" Text='<%# Eval("LocID")%>'></asp:TextBox>
         <asp:RequiredFieldValidator ID="ReqLocationID" runat="server" ControlToValidate="txtLocID" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtLocID" Width = "200px" MaxLength = "60" runat="server"></asp:TextBox>     
         <asp:RequiredFieldValidator ID="ReqLocationID" runat="server" ControlToValidate="txtLocID" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
         </asp:RequiredFieldValidator>
    </FooterTemplate>
</asp:TemplateField>
    
<asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Name"  >
     <ItemTemplate>
         <asp:Label ID="lblLocName" runat="server" Text='<%# Eval("LocName")%>'></asp:Label>
     </ItemTemplate>
    <EditItemTemplate>
         <asp:TextBox ID="txtLocName" runat="server" Text='<%# Eval("LocName")%>'></asp:TextBox>
           <asp:RequiredFieldValidator ID="ReqLocationName" runat="server" ControlToValidate="txtLocName" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    <FooterTemplate>
       <asp:TextBox ID="txtLocName" Width = "200px" MaxLength = "60" runat="server"></asp:TextBox>  
          <asp:RequiredFieldValidator ID="ReqLocationName" runat="server" ControlToValidate="txtLocName" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
         </asp:RequiredFieldValidator>
    </FooterTemplate>
</asp:TemplateField>

<asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument = '<%# Eval("StationID")%>' 
         OnClientClick = "return confirm('Would you like to inactivate this record?')" 
        Text = "Inactive" OnClick = "InactivateStation" CausesValidation="false"></asp:LinkButton>
    </ItemTemplate>

   
    <FooterTemplate>
      
            <asp:Button ID="btnAdd" runat="server" Text="Add Station" OnClick = "AddNewStation" ValidationGroup="GroupFooterInsert"   />
           
    </FooterTemplate>
    

</asp:TemplateField>

      <asp:CommandField  ShowEditButton="True" />

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
