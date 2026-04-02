<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Projeto.aspx.vb" Inherits="Projeto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Lista de filmes Kaggle</title>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:radScriptManager 
        ID="RadScriptManager1" 
        runat="server" ExternaljQueryUrl="">
        </telerik:RadScriptManager>

        <h2>Todos Os filmes importados</h2>
        
        <telerik:RadGrid id="GridFilmes" runat="server" OnNeedDataSource="GridFilmes_NeedDataSource"></telerik:RadGrid>
        
    </form>
</body>
</html>
