<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sitecore.Support.sitecore.admin.ViewLinks, Sitecore.Support.213300" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Links Viewer</title>
</head>
<body>
<form id="Form1" runat="server">
    <div class="label">
        <asp:Label runat="server" ID="label"></asp:Label>
    </div>
            
    <div>
        <asp:Label runat="server">Referrers</asp:Label>
        <asp:PlaceHolder id="_referrers" runat="server"></asp:PlaceHolder>
    </div>
    <div>
        <asp:Label runat="server">References</asp:Label>
        <asp:PlaceHolder id="_references" runat="server"></asp:PlaceHolder>
    </div>
</form>
</body>
</html>