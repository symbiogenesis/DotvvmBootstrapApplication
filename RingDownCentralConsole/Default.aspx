<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RingDownCentralConsole._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <p>
            <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />
        </p>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick"></asp:Timer>
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-striped" GridLines="None" AutoGenerateColumns="false" Width="800px"
                    AllowSorting="True" OnSorting="GridView1_Sorting" EmptyDataText="No Records Available" OnRowDataBound="GridView1_RowDataBound">
                    <RowStyle CssClass="cursor-pointer" />
                    <Columns>
                        <asp:BoundField DataField="Code" HeaderText="Location Code" HeaderStyle-Width="20%" ItemStyle-Wrap="true" SortExpression="Code" />
                        <asp:BoundField DataField="LocationName" HeaderText="Location Name" HeaderStyle-Width="30%" ItemStyle-Wrap="true" SortExpression="LocationName" />
                        <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="20%" ItemStyle-Wrap="true" SortExpression="Status" />
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="65px" Width="85px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
