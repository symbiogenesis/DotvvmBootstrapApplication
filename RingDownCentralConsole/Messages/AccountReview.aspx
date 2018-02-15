<%@ Page Title="Account Review" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountReview.aspx.cs" Inherits="RingDownCentralConsole.Messages.AccountReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <p class="text-center p-5">
        <em>The system administratrator will have to review and approve your registration before you gain access to the RDCC system. <br />
            Please make sure that you log into your email inbox and click to confirm your registration. <br />
            Thank you, RDCC Staff
        </em>
        <em>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="OK" />
        </em>
    </p>

</asp:Content>
