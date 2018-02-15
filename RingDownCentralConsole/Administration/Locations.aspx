<%@ Page Title="Locations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="RingDownCentralConsole.Locations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="Msg" runat="server" ForeColor="maroon" Visible="false" />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-striped"
                GridLines="None" ShowFooter="true" OnPageIndexChanging="OnPaging" OnRowEditing="EditLocation" OnRowUpdating="UpdateLocation" OnRowCancelingEdit="CancelEdit" EmptyDataText="No Location Records Entered" OnSorting="GridView1_Sorting" AllowSorting="true">
                <RowStyle CssClass="cursor-pointer" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Location Code" SortExpression="Code">
                        <ItemTemplate>
                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" Text='<%# Eval("Code")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqCode" runat="server" ControlToValidate="txtCode" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCode" CssClass="form-control" MaxLength="60" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqCode" runat="server" ControlToValidate="txtCode" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Location Name" SortExpression="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Eval("Name")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtName" CssClass="form-control" MaxLength="60" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Serial #" SortExpression="SerialNumber">
                        <ItemTemplate>
                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSerialNumber" CssClass="form-control" runat="server" Text='<%# Eval("SerialNumber")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqSerialNumber" runat="server" ControlToValidate="txtSerialNumber" ErrorMessage="*" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSerialNumber" CssClass="form-control" MaxLength="80" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqSerialNumber" runat="server" ControlToValidate="txtSerialNumber" ErrorMessage="*" ForeColor="Red" ValidationGroup="GroupFooterInsert">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%# Eval("Id")%>'
                                OnClientClick="return confirm('Would you like to make this record inactive?')"
                                Text="Inactive" OnClick="InactivateLocation" CausesValidation="false"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnAdd" runat="server" CssClass="form-control btn-primary" Text="Add Location" OnClick="AddNewLocation" ValidationGroup="GroupFooterInsert" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:CommandField ShowEditButton="True" />

                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridView1" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
