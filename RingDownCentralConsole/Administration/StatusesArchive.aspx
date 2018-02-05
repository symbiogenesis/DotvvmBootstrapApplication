<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatusesArchive.aspx.cs" Inherits="RingDownCentralConsole.StatusesArchive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../Css/rdccCSS.css" /> 
    <div  class="center-div">

     Statuses

        Archive</div>

   

    <br /><br />

    <asp:Label ID="lblResult" runat="server"  Font-Bold="true"/>

   
    <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />
 
     
<asp:GridView ID="GridView1" runat="server"  Width = "800px" AutoGenerateColumns = "false" ForeColor="#333333" GridLines="None" 
AlternatingRowStyle-BackColor = "white" HeaderStyle-BackColor = "#507CD1" AllowPaging ="true"  ShowFooter = "false" 
OnPageIndexChanging = "OnPaging" PageSize = "100" EmptyDataText="No Archived Status Records"    
     AllowSorting="true"  OnSorting="GridView1_Sorting" >
 <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#FEFCFF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="False" ForeColor="Black" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"  />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center"/>
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />    
     <EmptyDataRowStyle Font-Bold="True" ForeColor="Black" BorderColor="Red" BorderWidth="2px" />
<Columns>
    <asp:TemplateField ItemStyle-Width = "50px" visible="false" >
    <ItemTemplate>
        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>     
       
<asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Status" SortExpression="Name">
     <ItemTemplate>
         <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
     </ItemTemplate>     
    </asp:TemplateField>

<asp:TemplateField HeaderText="Image" HeaderStyle-Width="200px" >  
         <ItemTemplate>  
             <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="70px" Width="90px" />  
          </ItemTemplate>  
    
</asp:TemplateField>
    <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Image Name" SortExpression="ImageName">
     <ItemTemplate>
         <asp:Label ID="lblImageName" runat="server" Text='<%# Eval("ImageName")%>'></asp:Label>
     </ItemTemplate></asp:TemplateField> 

     <asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server" CausesValidation="false" CommandArgument = '<%# Eval("Id")%>' 
         OnClientClick = "return confirm('Would you like to make this record active?')" 
        Text = "Active" OnClick = "ActivateRecord"></asp:LinkButton>
    </ItemTemplate>
    </asp:TemplateField>


    <asp:CommandField  ShowEditButton="False"  />




</Columns>
<AlternatingRowStyle BackColor="white"  />
</asp:GridView>
         
 



</asp:Content>
