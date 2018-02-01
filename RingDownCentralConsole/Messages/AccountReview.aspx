<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountReview.aspx.cs" Inherits="RingDownCentralConsole.Messages.AccountReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    
      <br />
    <br />

    <p class="text-center" style="color: #000000; font-size: medium;">
        <em>The system administratrator will have to review and approve your account before you gain access to the RDCC system. <br />
            Please make sure that you log into your email account and click to confirm your account. <br />
            Thank you, RDCC Staff
        </em>. 
        
         
 
    <br />
    <p class="text-center" style="color: #000000; font-size: medium;">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="OK" />
    </p>
</asp:Content>
