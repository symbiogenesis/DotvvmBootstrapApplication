<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUserRoles.aspx.cs" Inherits="RingDownCentralConsole.CreateUserRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .center-div
        {
          text-align: center;
          font-size: large;
          font-weight: bold;
        }
        
    th
    {
        text-align:center;
    }
    </style>
    <div class="center-div">

      Create A Role

    </div>

  <asp:Label id="Msg" ForeColor="maroon" runat="server" /><br />

  Role name: 

  <asp:TextBox id="RoleTextBox" runat="server" />
    <asp:RequiredFieldValidator ID="ReqRole" runat="server" ControlToValidate="RoleTextBox" ErrorMessage="*" ForeColor="Red" >
         </asp:RequiredFieldValidator>


  <asp:Button Text="Create Role" id="CreateRoleButton"
              runat="server" OnClick="CreateRole_OnClick" />

  <br />

  <asp:GridView runat="server" CellPadding="2" id="RolesGrid" 
                Gridlines="Both" CellSpacing="2" AutoGenerateColumns="false" >
    <HeaderStyle BackColor="navy" ForeColor="white" />
    <Columns>
      <asp:TemplateField HeaderText="Roles" >
        <ItemTemplate>
          <%# Container.DataItem.ToString() %>
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>
   </asp:GridView>
</asp:Content>
