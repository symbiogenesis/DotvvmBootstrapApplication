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


  
<asp:GridView ID="GridView1" runat="server"  Width = "800px" AutoGenerateColumns = "false" ForeColor="#333333" GridLines="None" 
 AllowPaging ="true"  ShowFooter = "true" OnPageIndexChanging = "OnPaging" onrowediting="EditLocation" onrowupdating="UpdateLocation"  
    onrowcancelingedit="CancelEdit"
PageSize = "100" EmptyDataText="No Location Records Entered"    OnSorting="GridView1_Sorting" AllowSorting="true">
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
    
<asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Location Name" SortExpression="Name"  >
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

    <asp:TemplateField ItemStyle-Width = "400px"  HeaderText = "Serial #" SortExpression="SerialNumber" >
     <ItemTemplate>
         <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
     </ItemTemplate>
    <EditItemTemplate>
         <asp:TextBox ID="txtSerialNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:TextBox>
           <asp:RequiredFieldValidator ID="ReqSerialNumber" runat="server" ControlToValidate="txtSerialNumber" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    <FooterTemplate>
       <asp:TextBox ID="txtSerialNumber" Width = "200px" MaxLength = "80" runat="server"></asp:TextBox>  
          <asp:RequiredFieldValidator ID="ReqSerialNumber" runat="server" ControlToValidate="txtSerialNumber" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
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
