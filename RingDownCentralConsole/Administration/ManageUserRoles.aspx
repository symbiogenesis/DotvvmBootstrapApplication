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
          <asp:ListBox ID="RolesListBox" runat="server" CssClass="form-control" Rows="8" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="RolesListBox_SelectedIndexChanged" />
        </td>
        <td>Users:</td>
        <td>
          <asp:ListBox ID="UsersListBox" runat="server" CssClass="form-control" Rows="8" DataTextField="Username" SelectionMode="Multiple" /></td>
        <td>
          <asp:Button Text="Add User(s) to Role" CssClass="form-control" ID="AddUsersButton" runat="server" OnClick="AddUsers_OnClick" />
        </td>
      </tr>
      <tr>
        <td>Users In Role:</td>
        <td>
            <asp:GridView runat="server" CellPadding="4" ID="UsersInRoleGrid" AutoGenerateColumns="false" CssClass="table table-hover table-striped" 
                GridLines="None" CellSpacing="0" OnRowCommand="UsersInRoleGrid_RemoveFromRole">
                <Columns>
                    <asp:TemplateField HeaderText="User Name" >
                    <ItemTemplate>
                        <%# ((RingDownCentralConsole.Models.ApplicationUser) Container.DataItem).UserName %>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField Text="Remove From Role" ButtonType="Link" ControlStyle-CssClass="form-control" />
                </Columns>
            </asp:GridView>
        </td>
      </tr>
    </table>






</asp:Content>
