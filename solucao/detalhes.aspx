<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detalhes.aspx.vb" Inherits="detalhes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Arquivo Secreto - Detalhes do Filme</title>
</head>
<body>
    <form id="form1" runat="server">
        
       
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>

        <h2 style="font-family: Arial;">Relatório Completo do Filme</h2>
        <p>Abaixo estão as informações estendidas (Popularidade, Notas, etc) do ID selecionado:</p>

        <telerik:RadGrid id="GridDetalhes" runat="server" AutoGenerateColumns="false">

            <MasterTableView>

                <NoRecordsTemplate>
                    Nenhum filme foi encontrado com esse ID secreto!
                </NoRecordsTemplate>

                <Columns>

                    <telerik:GridBoundColumn DataField="id" HeaderText="Código Oficial" />
                    <telerik:GridBoundColumn DataField="title" HeaderText="Título" />
                    <telerik:GridBoundColumn DataField="original_language" HeaderText="Idioma" />
                    <telerik:GridBoundColumn DataField="adult" HeaderText="Filme +18" />
                    <telerik:GridBoundColumn DataField="popularity" HeaderText="Índice de Popularidade" />
                    <telerik:GridBoundColumn DataField="vote_average" HeaderText="Nota Média" />
                    <telerik:GridBoundColumn DataField="vote_count" HeaderText="Nº de Votos" />
                    <telerik:GridBoundColumn DataField="release_date" HeaderText="Lançamento" DataFormatString="{0:dd/MM/yyyy}" />
                    <telerik:GridBoundColumn DataField="overview" HeaderText="Sinopse Brasileira" />

                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

        <br /><br />
        <a href="Projeto.aspx" style="padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; font-family: Arial;">Voltar para a Lista Mestra</a>
        
    </form>
</body>
</html>
