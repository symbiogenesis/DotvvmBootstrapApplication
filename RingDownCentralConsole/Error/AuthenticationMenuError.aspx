<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthenticationMenuError.aspx.cs" Inherits="RingDownCentralConsole.Error.AuthenticationMenuError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <br />
    <br />

    <p class="text-center" style="color: #FF0000; font-size: large;">
        <em>User Role Error, please contact your system administrator</em></p>
 
    <br />
    <p class="text-center" style="color: #FF0000; font-size: medium;">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Login" />
    </p>


</asp:Content>
