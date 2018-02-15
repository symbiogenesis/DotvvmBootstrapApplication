<%@ Page Title="Locations Archive" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationsArchive.aspx.cs" Inherits="RingDownCentralConsole.LocationsArchive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />
            <div id="dvGrid">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-hover table-striped"
                    ShowFooter="false" OnPageIndexChanging="OnPaging" EmptyDataText="No Archived Location Records" OnSorting="GridView1_Sorting" AllowSorting="true">
                    <RowStyle CssClass="cursor-pointer" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="50px" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="400px" HeaderText="Location Code" SortExpression="Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="400px" HeaderText="Location Name" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="400px" HeaderText="Serial #" SortExpression="SerialNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%# Eval("Id")%>'
                                    OnClientClick="return confirm('Would you like to make this record active?')"
                                    Text="Active" OnClick="ActivateLocation" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="False" />

                    </Columns>
                </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
