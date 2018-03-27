<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sitecore.Support.sitecore.admin.ViewLinks, Sitecore.Support.213300" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Links Viewer</title>
</head>
<body>
<form id="Form1" runat="server">
    <div>
        <asp:Label runat="server">Item ID: </asp:Label>
        <asp:TextBox runat="server" ID="_itemIdTextBox" />
        <asp:Button runat="server" ID="_getLinksButton" Text="Ok" OnClick="GetLinksButton_Click" />
        <asp:RadioButtonList ID="_radioButtons" runat="server">
            <asp:ListItem text="master" value="true" />
            <asp:ListItem text="web" value="false" />
            <asp:ListItem text="core" value="false" />
        </asp:RadioButtonList>
    </div>
    <asp:Label runat="server" ID="_errorMessage" />
    <div>
        <asp:Label runat="server"><h3>Items that refer to the selected item:</h3></asp:Label>
        <asp:PlaceHolder id="_referrers" runat="server"></asp:PlaceHolder>
    </div>
    <div>
        <asp:Label runat="server"><h3>Items that the selected item refer to:</h3></asp:Label>
        <asp:PlaceHolder id="_references" runat="server"></asp:PlaceHolder>
    </div>
</form>
</body>
</html>