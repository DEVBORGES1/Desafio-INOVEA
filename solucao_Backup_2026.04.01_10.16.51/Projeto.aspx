<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Desafio.aspx.vb" Inherits="Desafio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radScriptManager ID="RadScriptManager1" runat="server"></telerik:radScriptManager>
        <div>
            <telerik:Radgrid ID="RadGrid1" runat="server"></telerik:Radgrid>
        </div>
    </form>
    <asp:SqlDataSource ID="SqlDataSource" runat="server"></asp:SqlDataSource>
</body>
</html>
