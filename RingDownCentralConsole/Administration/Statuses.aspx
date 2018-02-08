<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statuses.aspx.cs" Inherits="RingDownCentralConsole.Statuses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="center-div">Statuses</div>

    <asp:Label ID="Msg" runat="server" ForeColor="maroon" /><br />

    <asp:GridView ID="GridView1" runat="server" Width="800px" AutoGenerateColumns="false" GridLines="None" ShowFooter="false"
        OnPageIndexChanging="OnPaging" EmptyDataText="No Status Records Entered" CssClass="table table-hover table-striped" 
        OnRowCancelingEdit="CancelEdit" AllowSorting="true" OnSorting="GridView1_Sorting">
        <RowStyle CssClass="cursor-pointer" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="50px" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ItemStyle-Width="200px" HeaderText="Status" SortExpression="Name">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name")%>' ValidationGroup="GridViewFields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ReqName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Image" HeaderStyle-Width="200px">
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
            <asp:TemplateField ItemStyle-Width="200px" HeaderText="Image Name" SortExpression="ImageName">
                <ItemTemplate>
                    <asp:Label ID="lblImageName" runat="server" Text='<%# Eval("ImageName")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="False" ValidationGroup="GridViewFields" />

        </Columns>
        <AlternatingRowStyle BackColor="white" />
    </asp:GridView>
</asp:Content>
