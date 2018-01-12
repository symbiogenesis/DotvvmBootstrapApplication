<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RingDownCentralConsole.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Login   ID="Login1"   runat="server"  OnLoggedIn="Login1_LoggedIn" ></asp:Login>
</asp:Content>
