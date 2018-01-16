<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="RingDownCentralConsole.Locations" %>

  
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

      Locations

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
OnPageIndexChanging = "OnPaging" onrowediting="EditLocation" onrowupdating="UpdateLocation"  onrowcancelingedit="CancelEdit"
PageSize = "100" EmptyDataText="No Location Records Entered" >
    
<Columns>
    <asp:TemplateField ItemStyle-Width = "50px" visible="false"  >
    <ItemTemplate>
        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>

     

    <asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Code">
    <ItemTemplate>
        <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("Code")%>'></asp:TextBox>
         <asp:RequiredFieldValidator ID="ReqCode" runat="server" ControlToValidate="txtCode" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtCode" Width = "200px" MaxLength = "60" runat="server"></asp:TextBox>     
         <asp:RequiredFieldValidator ID="ReqCode" runat="server" ControlToValidate="txtCode" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
         </asp:RequiredFieldValidator>
    </FooterTemplate>
</asp:TemplateField>
    
<asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Name"  >
     <ItemTemplate>
         <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
     </ItemTemplate>
    <EditItemTemplate>
         <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name")%>'></asp:TextBox>
           <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    <FooterTemplate>
       <asp:TextBox ID="txtName" Width = "200px" MaxLength = "60" runat="server"></asp:TextBox>  
          <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
         </asp:RequiredFieldValidator>
    </FooterTemplate>
</asp:TemplateField>

<asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument = '<%# Eval("Id")%>' 
         OnClientClick = "return confirm('Would you like to make this record inactive?')" 
        Text = "Inactive" OnClick = "InactivateLocation" CausesValidation="false"></asp:LinkButton>
    </ItemTemplate>

   
    <FooterTemplate>
      
            <asp:Button ID="btnAdd" runat="server" Text="Add Location" OnClick = "AddNewLocation" ValidationGroup="GroupFooterInsert"   />
           
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
