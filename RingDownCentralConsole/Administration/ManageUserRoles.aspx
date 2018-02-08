<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUserRoles.aspx.cs" Inherits="RingDownCentralConsole.ManageUserRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div  class="center-div">
      Manage User Roles
    </div>
    <div>
        <p></p>
    </div>

    <asp:Label ID="Msg" ForeColor="maroon" runat="server" /><br />
    <table>
      <tr>
        <td>Roles:</td>
        <td>
          <asp:ListBox ID="RolesListBox" runat="server" Rows="8" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="RolesListBox_SelectedIndexChanged" />
        </td>
        <td>Users:</td>
        <td>
          <asp:ListBox ID="UsersListBox" DataTextField="Username" Rows="8" SelectionMode="Multiple" runat="server" /></td>
        <td>
          <asp:Button Text="Add User(s) to Role" ID="AddUsersButton" runat="server" OnClick="AddUsers_OnClick" />
        </td>
      </tr>
      <tr>
        <td>Users In Role:</td>
        <td>
            <asp:GridView runat="server" CellPadding="4" ID="UsersInRoleGrid" AutoGenerateColumns="false"
                GridLines="None" CellSpacing="0" OnRowCommand="UsersInRoleGrid_RemoveFromRole">
                <HeaderStyle BackColor="navy" ForeColor="white" />
                <Columns>
                    <asp:TemplateField HeaderText="User Name" >
                    <ItemTemplate>
                        <%# ((RingDownCentralConsole.Models.ApplicationUser) Container.DataItem).UserName %>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField Text="Remove From Role" ButtonType="Link" />
                </Columns>
            </asp:GridView>
        </td>
      </tr>
    </table>






</asp:Content>
