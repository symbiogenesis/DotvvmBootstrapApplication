<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rdccReports.aspx.cs" Inherits="RingDownCentralConsole.rdccReports" %>
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

      Reports

    </div>

    <div>


    <table style="width:50%;" border="1">
        <tr>
            <td>Main Report </td>
             <td>
                 <asp:HyperLink ID="Report1" runat="server" NavigateUrl="~/Reports/rdccMainReport.aspx" Target="_blank" ToolTip="Report 1">Report 1</asp:HyperLink>
            </td>
        </tr>
        </table>
    

     </div>


</asp:Content>
