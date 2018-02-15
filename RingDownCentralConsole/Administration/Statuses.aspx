<%@ Page Title="Statuses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statuses.aspx.cs" Inherits="RingDownCentralConsole.Statuses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="status-page">

        <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" GridLines="None" ShowFooter="false"
            OnPageIndexChanging="OnPaging" EmptyDataText="No Status Records Entered" CssClass="table table-striped"
            OnRowCancelingEdit="CancelEdit" AllowSorting="true" OnSorting="GridView1_Sorting">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Status" SortExpression="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name")%>' ValidationGroup="GridViewFields"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="70px" Width="90px" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:FileUpload ID="FileUpload2" runat="server" ValidationGroup="GridViewFields" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="ReqImage" runat="server" ControlToValidate="FileUpload2" ErrorMessage="*" ForeColor="Red">
                            <asp:RegularExpressionValidator ID="reqExFileUpload2" runat="server"
                                ControlToValidate="FileUpload2" ValidationExpression=".*((\.jpg)|(\.bmp)|(\.gif) |(\.png))"
                                ErrorMessage="Images">Images Only</asp:RegularExpressionValidator>
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Image Name" SortExpression="ImageName">
                    <ItemTemplate>
                        <asp:Label ID="lblImageName" runat="server" Text='<%# Eval("ImageName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="False" ValidationGroup="GridViewFields" />

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
