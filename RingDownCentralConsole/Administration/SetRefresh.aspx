<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetRefresh.aspx.cs" Inherits="RingDownCentralConsole.Administration.SetRefresh" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" type="text/css" href="rdccCSS.css" /> 
    <div  class="center-div">
        Set Refresh Interval</div>
    <div>
        <p><asp:Label id="Msg" runat="server" ForeColor="maroon" /><br /></p>
    </div>
       <br />

    <p>

        <asp:Label ID="Label1" runat="server" Text="Use the drop box to select the number of seconds (1 second = 1000 milliseconds)."></asp:Label>
    </p>
    <br />
    <p>

    <asp:DropDownList ID="ddlIntervalSeconds" runat="server">
        <asp:ListItem Selected="True">1000</asp:ListItem>
        <asp:ListItem>2000</asp:ListItem>
        <asp:ListItem>3000</asp:ListItem>
        <asp:ListItem>4000</asp:ListItem>
          <asp:ListItem>5000</asp:ListItem>
        <asp:ListItem>6000</asp:ListItem>
         <asp:ListItem>7000</asp:ListItem>
          <asp:ListItem>8000</asp:ListItem>
          <asp:ListItem>9000</asp:ListItem>
          <asp:ListItem>10000</asp:ListItem>     
    </asp:DropDownList>


    </p>

    
    <br />
    <asp:TextBox ID="txtRefreshVal" runat="server" BackColor="#EFF3FB" ReadOnly="true"></asp:TextBox>
    <br /><br />
    <asp:Button ID="Button1" runat="server" Text="Update"  OnClick="SetInterval_Click"/>
</asp:Content>
