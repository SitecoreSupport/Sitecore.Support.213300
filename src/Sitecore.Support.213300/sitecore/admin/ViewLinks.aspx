<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sitecore.Support.sitecore.admin.ViewLinks, Sitecore.Support.213300" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Links Viewer</title>
</head>
<body>
<form id="Form1" runat="server">
    <div class="head">
        <asp:Label runat="server" ID="ItemIdLabel">Item ID: </asp:Label>
        <asp:TextBox runat="server" ID="itemIdTextBox" />
        <asp:RadioButtonList ID="_radioButtons" runat="server">
            <asp:ListItem text="master" value="true" />
            <asp:ListItem text="web" value="false" />
            <asp:ListItem text="core" value="false" />
        </asp:RadioButtonList>
    </div>
            
    <div>
        <asp:Label runat="server">Items that refer to the selected item:</asp:Label>
        <asp:PlaceHolder id="_referrers" runat="server"></asp:PlaceHolder>
    </div>
    <div>
        <asp:Label runat="server">Items that the selected item refer to:</asp:Label>
        <asp:PlaceHolder id="_references" runat="server"></asp:PlaceHolder>
    </div>
</form>
</body>
</html>