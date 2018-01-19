<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="RingDownCentralConsole.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="rdccCSS.css" /> 
    <div  class="center-div">

     Admin Dashboard (Username / Password Administration)

    </div>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

  <asp:Label id="Msg" runat="server" ForeColor="maroon" /><br />

        <div id = "dvGrid" style ="padding:10px;width:800px">


        
 


<asp:GridView ID="GridView1" runat="server"  Width = "550px" AutoGenerateColumns = "false" Font-Names = "Arial" Font-Size = "11pt" 
AlternatingRowStyle-BackColor = "white" HeaderStyle-BackColor = "#507CD1"  ShowFooter = "false" PageSize = "100" 
    EmptyDataText="No Records Entered" onrowcancelingedit="CancelEdit"  onrowediting="EditUser" onrowupdating="UpdateUser" 
    OnRowDataBound="GridView1_RowDataBound" RowStyle-Wrap="false">
      
<Columns>

  
    <asp:TemplateField   Visible="false" >
    <ItemTemplate>
        <asp:Label ID="UserID" runat="server" Text='<%# Eval("UserID")%>'></asp:Label>
    </ItemTemplate>    
</asp:TemplateField>   

   
    <asp:TemplateField  HeaderText = "Username">
    <ItemTemplate>
        <div style="width: 150px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
            </div>
    </ItemTemplate>     
</asp:TemplateField>

    <asp:TemplateField  HeaderText = "Role">
    <ItemTemplate>
        <div style="width: 100px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
        <asp:Label ID="lblRoleName" runat="server" Text='<%# Eval("RoleName")%>'></asp:Label>
            </div>
    </ItemTemplate>     
</asp:TemplateField>
    
     <asp:TemplateField  HeaderText = "Active Status">
    <ItemTemplate>    
        <div style="width: 60px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
         <asp:Label ID="lblIsApproved" Text='<%# Eval("IsApproved").ToString() == "True" ? "Active" : "Inactive" %>' runat="server" />
            </div>
    </ItemTemplate>     
</asp:TemplateField>

     <asp:TemplateField  HeaderText = "First Name">
    <ItemTemplate>
        <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName")%>'></asp:Label>
    </ItemTemplate> 
        
</asp:TemplateField>
    

     <asp:TemplateField  HeaderText = "Last Name">
    <ItemTemplate>
        <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName")%>'></asp:Label>
    </ItemTemplate>     
          
</asp:TemplateField>

      <asp:TemplateField  HeaderText = "Email Address">
    <ItemTemplate>
        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
    </ItemTemplate>     
          
</asp:TemplateField>
     
    


     <asp:TemplateField  HeaderText = "Account Created Date">
    <ItemTemplate>
        <asp:Label ID="lblCreateDate" runat="server" Text='<%# (Convert.ToDateTime(Eval("CreateDate"))).ToShortDateString()  %>' ></asp:Label>
    </ItemTemplate>     
</asp:TemplateField>

     <asp:TemplateField  HeaderText = "Last Login Date">
    <ItemTemplate>
        <asp:Label ID="lblLastLoginDate" runat="server"  Text='<%# (Convert.ToDateTime(Eval("LastLoginDate"))).ToShortDateString()  %>' ></asp:Label>
    </ItemTemplate>     
</asp:TemplateField>

    <asp:TemplateField   HeaderText = "Last Password Changed Date">
    <ItemTemplate>
        <asp:Label ID="lblPasswordChangedDate" runat="server" Text='<%# (Convert.ToDateTime(Eval("LastPasswordChangedDate"))).ToShortDateString()  %>' ></asp:Label>
    </ItemTemplate>     
</asp:TemplateField>


   

      <asp:TemplateField >
    <ItemTemplate>
        <asp:LinkButton ID="lnkRoles" runat="server" CommandArgument = '<%# Eval("UserName")%>' 
            OnClientClick = "return confirm('Would you like to set the role for this user?')" 
                Text = "Set Role" OnClick = "Roles_OnClick"  ></asp:LinkButton>
    </ItemTemplate>
    </asp:TemplateField>


 
     <asp:TemplateField >
    <ItemTemplate>
        <asp:LinkButton ID="lnkActDeactivate" runat="server" CommandArgument = '<%# Eval("UserName")%>' 
            OnClientClick = "return confirm('Would you like to Activate/Inactivate this user?')" 
                Text = "Active/Inactive" OnClick = "ActivateDeactivate_OnClick"  ></asp:LinkButton>
    </ItemTemplate>
    </asp:TemplateField>


    <asp:TemplateField >
    <ItemTemplate>
        <asp:LinkButton ID="lnkRstPassword" runat="server" CommandArgument = '<%# Eval("UserName")%>' 
            OnClientClick = "return confirm('Would you like to reset the password for this user?')" 
                Text = "Reset Password" OnClick = "ResetPassword_OnClick"  ></asp:LinkButton>
    </ItemTemplate>
    </asp:TemplateField>


</Columns>
</asp:GridView>
               </div>
            </ContentTemplate> 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID = "GridView1" /> 
    </Triggers> 

       
    </asp:UpdatePanel> 

  


</asp:Content>
