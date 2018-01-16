﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statuses.aspx.cs" Inherits="RingDownCentralConsole.Statuses" %>
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
    <div class="center-div">

     Statuses

    </div>

  <table>
      <tr><td>&nbsp;</td> 
          <td> 
              &nbsp;</td> <td> 
               &nbsp;</td></tr>
        <tr><td>Status Name</td>
            <td><asp:TextBox ID="txtName" runat="server" ></asp:TextBox></td> 
            <td>&nbsp;</td></tr>
        <tr><td>&nbsp;</td>
            <td><asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" >
               </asp:RequiredFieldValidator> </td> 
            <td>&nbsp;</td></tr>
             <tr><td>Image</td> <td>  <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                 <td> </td></tr></table>
    <br />

   

    <asp:Button ID="upload" runat="server" Font-Bold="true" Text="Insert Record" OnClick="upload_Click"  />
    <asp:Label ID="lblResult" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />
        <div id = "dvGrid" style ="padding:10px;width:550px">
     
<asp:GridView ID="GridView1" runat="server"  Width = "550px" AutoGenerateColumns = "false" Font-Names = "Arial" Font-Size = "11pt" 
AlternatingRowStyle-BackColor = "white" HeaderStyle-BackColor = "#507CD1" AllowPaging ="true"  ShowFooter = "false" 
OnPageIndexChanging = "OnPaging" PageSize = "100" EmptyDataText="No Records Entered"    onrowediting="EditStatus" onrowupdating="UpdateStatus"  onrowcancelingedit="CancelEdit">
    
<Columns>
    <asp:TemplateField ItemStyle-Width = "50px" visible="false" >
    <ItemTemplate>
        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>     
       
<asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Status Name">
     <ItemTemplate>
         <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
     </ItemTemplate>  

     <EditItemTemplate>
        <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name")%>' ></asp:TextBox>
         <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red">
         </asp:RequiredFieldValidator>
    </EditItemTemplate> 
    </asp:TemplateField>

<asp:TemplateField HeaderText="Image" HeaderStyle-Width="200px">  
         <ItemTemplate>  
             <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="70px" Width="90px" />  
          </ItemTemplate>  
      <EditItemTemplate>
           <asp:FileUpload ID="FileUpload2" runat="server" />      
              <asp:RequiredFieldValidator ID="ReqImage" runat="server" ControlToValidate="FileUpload2" ErrorMessage="*" ForeColor="Red">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_ImageSmall" runat="server"
                             ControlToValidate ="FileUpload2" ValidationExpression=".*((\.jpg)|(\.bmp)|(\.gif) |(\.png))"
                              ErrorMessage="Images">Images Only</asp:RegularExpressionValidator>
         </asp:RequiredFieldValidator>

      </EditItemTemplate>

</asp:TemplateField>
    <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Image Name">
     <ItemTemplate>
         <asp:Label ID="lblImageName" runat="server" Text='<%# Eval("ImageName")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server" CausesValidation="false" CommandArgument = '<%# Eval("Id")%>' 
         OnClientClick = "return confirm('Would you like to make this record inactive?')" 
        Text = "Inactive" OnClick = "InactivateRecord"></asp:LinkButton></ItemTemplate></asp:TemplateField><asp:CommandField  ShowEditButton="True" />

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
