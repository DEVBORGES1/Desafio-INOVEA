<%@ Page Language="VB" AutoEventWireup="false" Culture="pt-BR" UICulture="pt-BR" CodeFile="Projeto.aspx.vb" Inherits="Projeto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Lista de filmes Kaggle</title>
    <style>
        .mensagem {
            margin: 10px 0;
            padding: 10px;
            border-radius: 4px;
            font-family: Arial;
        }

        .mensagem.sucesso {
            background-color: #e8f5e9;
            color: #1b5e20;
            border: 1px solid #a5d6a7;
        }

        .mensagem.erro {
            background-color: #ffebee;
            color: #b71c1c;
            border: 1px solid #ef9a9a;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:radScriptManager 
        ID="RadScriptManager1" 
        runat="server" ExternaljQueryUrl="">
        </telerik:RadScriptManager>

        <h2>Todos Os filmes importados</h2>
        <asp:Label ID="lblMensagem" runat="server" Visible="false"></asp:Label>
        
        <telerik:RadGrid id="GridFilmes" 
            runat="server" 
            Culture="pt-BR"
            AllowPaging="true"
            PageSize="15"
            AllowSorting="true"
            AllowFilteringByColumn="true"
            AutoGenerateColumns="false"
            OnNeedDataSource="GridFilmes_NeedDataSource"
            OnInit="GridFilmes_Init"
            OnUpdateCommand="GridFilmes_UpdateCommand"
            OnInsertCommand="GridFilmes_InsertCommand"
            OnDeleteCommand="GridFilmes_DeleteCommand"
            OnItemCommand="GridFilmes_ItemCommand"
            >
               <MasterTableView DataKeyNames="id" ClientDataKeyNames="id" CommandItemDisplay ="Top">
                   <CommandItemSettings AddNewRecordText="Adicionar Novo Filme" 
                       RefreshText="Atualizar Grade" />

                   <NoRecordsTemplate>
                       Nenhum filme encontrado. Tente limpar filtros ou cadastrar um novo filme.
                   </NoRecordsTemplate>


                   <Columns>

                       <telerik:GridEditCommandColumn EditText="Editar"/>
                       <telerik:GridBoundColumn DataField="title" HeaderText="Titulo do Filme" />
                       <telerik:GridBoundColumn DataField="overview" HeaderText="Sinopse Oficial" />
                       <telerik:GridBoundColumn DataField="release_date" HeaderText="Lançamento" DataFormatString="{0:dd/MM/yyyy}" />
                       <telerik:GridButtonColumn CommandName="VerDetalhes" Text="Detalhes" ButtonType="PushButton" />

                       <telerik:GridButtonColumn CommandName="Delete" Text="Excluir" ConfirmText="Tem certeza que deseja excluir o filme?"/>
                    
                   </Columns>

                       <EditFormSettings>

                           <EditColumn UpdateText="Salvar Alterações" CancelText="Não Salvar" />
                     
                       </EditFormSettings>

                   </MasterTableView>

            <ClientSettings>

                <%--Funçao que habilita a seleção fisica individual(as linhas vao ficar azuis  ao serem clicadas)--%>

                <Selecting AllowRowSelect="true" />

                <%--vamos usar um gatilha de js que dispara quando o evento 'duplo click' acontece--%>

                <ClientEvents OnRowDblClick="DuploCliqueJavascript" />


            </ClientSettings>

            
            <SortingSettings SortToolTip="Clique para ordenar" SortedAscToolTip="Ordenado em ordem crescente" SortedDescToolTip="Ordenado em ordem decrescente" />
            
            <PagerStyle FirstPageToolTip="Primeira Página" LastPageToolTip="Última Página" NextPagesToolTip="Próximas Páginas" NextPageToolTip="Próxima Página" PrevPagesToolTip="Páginas Anteriores" PrevPageToolTip="Página Anterior" PageSizeLabelText="Tamanho da Página:" PagerTextFormat="Mudar página: {4} &amp;nbsp;Página &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, itens &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." PageSizes="15,30,60" />
            
        </telerik:RadGrid>

       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                function DuploCliqueJavascript(remetente, args) {


                    var tabelaVisual = args.get_tableView();

                    var todasAsLinhas = tabelaVisual.get_dataItems();

                    var indiceDaLinhaClicada = args.get_itemIndexHierarchical();

                    var linhaSelecionada = todasAsLinhas[indiceDaLinhaClicada];

                    var idDoFilme = linhaSelecionada.getDataKeyValue("id");

                    window.location.href = "detalhes.aspx?id=" + idDoFilme;
                }

            </script>
        </telerik:RadCodeBlock>
        
    </form>
</body>
</html>
