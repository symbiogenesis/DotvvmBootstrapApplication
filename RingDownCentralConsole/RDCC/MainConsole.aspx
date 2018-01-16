﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainConsole.aspx.cs" Inherits="RingDownCentralConsole.MainConsole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <p></p>

    </div>
        <div>
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"  Font-Size="Medium" Font-Bold="true" AutoGenerateColumns="false" width="800px" 
                AllowSorting="True"  OnSorting="GridView1_Sorting" >
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                <Columns>
                    <asp:BoundField DataField="Code" HeaderText="Location Code" HeaderStyle-Width="20%"   ItemStyle-Wrap="true"  SortExpression="Code" />                    
                    <asp:BoundField DataField="LocationName"  HeaderText="Location Name"  HeaderStyle-Width="30%"  ItemStyle-Wrap="true" SortExpression="LocationName"/>
                    <asp:BoundField DataField="Status"  HeaderText="Status"  HeaderStyle-Width="20%"  ItemStyle-Wrap="true" SortExpression="Status"/>
                 <asp:TemplateField HeaderText="" HeaderStyle-Width="200px">  
         <ItemTemplate>  
             <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="65px" Width="85px" />  
          </ItemTemplate>             
 </asp:TemplateField>               
                    
                    </Columns>
            </asp:Gridview>
        </div>



</asp:Content>
