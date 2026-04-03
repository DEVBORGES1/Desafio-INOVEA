<%@ Page Language="VB" AutoEventWireup="false" Culture="pt-BR" UICulture="pt-BR" CodeFile="Projeto.aspx.vb" Inherits="Projeto" %>
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
        
        <telerik:RadGrid id="GridFilmes" 
            runat="server" 
            Culture="pt-BR"
            AllowPaging="true"
            AllowSorting="true"
            AllowFilteringByColumn="true"
            OnNeedDataSource="GridFilmes_NeedDataSource"
            OnInit="GridFilmes_Init">
            
            <SortingSettings SortToolTip="Clique para ordenar" SortedAscToolTip="Ordenado em ordem crescente" SortedDescToolTip="Ordenado em ordem decrescente" />
            
            <PagerStyle FirstPageToolTip="Primeira Página" LastPageToolTip="Última Página" NextPagesToolTip="Próximas Páginas" NextPageToolTip="Próxima Página" PrevPagesToolTip="Páginas Anteriores" PrevPageToolTip="Página Anterior" PageSizeLabelText="Tamanho da Página:" PagerTextFormat="Mudar página: {4} &amp;nbsp;Página &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, itens &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." />
            
        </telerik:RadGrid>
        
    </form>
</body>
</html>
