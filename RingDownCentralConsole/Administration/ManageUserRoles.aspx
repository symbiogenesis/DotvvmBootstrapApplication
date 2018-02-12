<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUserRoles.aspx.cs" Inherits="RingDownCentralConsole.ManageUserRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div  class="center-div">
      Manage User Roles
    </div>
    <div>
        <p></p>
    </div>

    <asp:Label ID="Msg" ForeColor="maroon" runat="server" /><br />

    <div class="row">
        <div class="col-md-6" style="padding: 10px;">
            <p style="font-weight: bold; font-size: 1.2em;">Roles:</p>
            <asp:ListBox ID="RolesListBox" runat="server" CssClass="form-control" Rows="8" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="RolesListBox_SelectedIndexChanged" />
        </div>
        <div class="col-md-6" style="padding: 10px;">
            <p style="font-weight: bold; font-size: 1.2em;">Users:</p>
            <asp:ListBox ID="UsersListBox" runat="server" CssClass="form-control" Rows="8" DataTextField="Username" SelectionMode="Multiple" />
        </div>
        <div class="col-md-6">
        </div>
        <div class="col-md-6" style="padding: 10px;">
            <asp:Button Text="Add User(s) to Role" CssClass="form-control btn-primary" ID="AddUsersButton" runat="server" OnClick="AddUsers_OnClick" />
        </div>
    </div>

    <p style="font-weight: bold; font-size: 1.2em;">Users In Role:</p>

    <asp:GridView runat="server" ID="UsersInRoleGrid" AutoGenerateColumns="false" CssClass="table table-hover table-striped" Width="500px"
        GridLines="None" OnRowCommand="UsersInRoleGrid_RemoveFromRole">
        <RowStyle HorizontalAlign="Center" />
        <Columns>            
            <asp:TemplateField HeaderText="User Name" >
            <ItemTemplate>
                <%# ((RingDownCentralConsole.Models.ApplicationUser) Container.DataItem).UserName %>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField Text="Remove From Role" ButtonType="Button" ControlStyle-CssClass="form-control btn-warning" ControlStyle-Width="160" />
        </Columns>
    </asp:GridView>

</asp:Content>
