<%@ Page Language="VB" AutoEventWireup="false" Culture="pt-BR" UICulture="pt-BR" CodeFile="Projeto.aspx.vb" Inherits="Projeto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Dashboard Corporativo Kaggle</title>
    <!-- Tipografia Moderna do Google -->
    <link href="https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;600&display=swap" rel="stylesheet" />
    <style>
        body {
            font-family: 'Outfit', sans-serif;
            background: linear-gradient(135deg, #0f172a, #1e293b);
            color: #f8fafc;
            margin: 0; 
            padding: 40px;
        }
        .glass-container {
            background: rgba(255, 255, 255, 0.03);
            backdrop-filter: blur(16px);
            -webkit-backdrop-filter: blur(16px);
            border: 1px solid rgba(255, 255, 255, 0.05);
            border-radius: 16px;
            padding: 20px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
            max-width: 100%;
            margin: 0 auto;
        }
        h2 {
            margin-top: 0;
            font-weight: 600;
            font-size: 28px;
            letter-spacing: 0.5px;
            color: #38bdf8;
            margin-bottom: 25px;
            border-bottom: 1px solid rgba(255,255,255,0.1);
            padding-bottom: 15px;
        }
        .dashboard-container {
            display: flex; gap: 20px; justify-content: space-between; margin-bottom: 30px; flex-wrap: wrap;
        }
        /* Proteção Responsiva de Quebra de Tela para a Tabela */
        .grid-wrapper {
            width: 100%;
            max-width: 100vw;
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
            border-radius: 8px;
            padding-bottom: 15px;
        }
        .grid-wrapper::-webkit-scrollbar { height: 8px; }
        .grid-wrapper::-webkit-scrollbar-thumb { background: #38bdf8; border-radius: 4px; }
        .stat-card {
            background: rgba(255, 255, 255, 0.05);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-left: 4px solid #38bdf8;
            border-radius: 12px;
            padding: 20px; flex: 1;
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
            transition: transform 0.2s ease;
        }
        .stat-card:hover { transform: translateY(-3px); }
        .stat-card h4 { margin: 0 0 10px 0; color: #94a3b8; font-size: 14px; text-transform: uppercase; letter-spacing: 1px; }
        .stat-card h3 { margin: 0; font-size: 22px; color: #f8fafc; font-weight: 600; }
    </style>
</head>
<body>
    <div class="glass-container">
    <form id="form1" runat="server">

        <telerik:radScriptManager 
        ID="RadScriptManager1" 
        runat="server" ExternaljQueryUrl="">
        </telerik:RadScriptManager>

        <h2>Painel de Filmes (Kaggle)</h2>
        
        <div class="dashboard-container">
            <div class="stat-card">
                <h4>Acervo Total</h4>
                <h3><asp:Label ID="lblTotalFilmes" runat="server" Text="0" /> Filmes</h3>
            </div>
            <div class="stat-card">
                <h4>Maior Bilheteria </h4>
                <h3><asp:Label ID="lblMaisPopular" runat="server" Text="-" /></h3>
            </div>
            <div class="stat-card">
                <h4>Aclamação da Crítica </h4>
                <h3><asp:Label ID="lblMaiorNota" runat="server" Text="-" /></h3>
            </div>
        </div>
        
        <div class="grid-wrapper">
            <telerik:RadGrid id="GridFilmes" 
                runat="server" 
            Skin="Bootstrap"
            Culture="pt-BR"
            AllowPaging="true"
            AllowCustomPaging="true"
            AllowSorting="true"
            AllowFilteringByColumn="true"
            AutoGenerateColumns="false"
            OnNeedDataSource="GridFilmes_NeedDataSource"
            OnInit="GridFilmes_Init"
            OnUpdateCommand="GridFilmes_UpdateCommand"
            OnInsertCommand="GridFilmes_InsertCommand"
            OnDeleteCommand="GridFilmes_DeleteCommand"
            OnItemDataBound="GridFilmes_ItemDataBound"
            EnableLinqExpressions="false"
            >
               <GroupingSettings CaseSensitive="false" />
               <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true" FileName="Relatorio_Filmes">
                   <Pdf PageWidth="297mm" PageHeight="210mm" DefaultFontFamily="Arial" />
               </ExportSettings>

               <MasterTableView DataKeyNames="id" ClientDataKeyNames="id" CommandItemDisplay="Top">
                   <CommandItemSettings AddNewRecordText="Adicionar Filme" 
                       RefreshText="Atualizar"
                       ShowExportToPdfButton="true" 
                       ShowExportToExcelButton="true" />


                   <Columns>

                       <telerik:GridEditCommandColumn EditText="Editar"/>
                       <telerik:GridBoundColumn DataField="title" HeaderText="Titulo do Filme" ShowFilterIcon="false" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" />
                       <telerik:GridBoundColumn DataField="overview" HeaderText="Sinopse Oficial" ShowFilterIcon="false" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" />
                       <telerik:GridNumericColumn DataField="vote_average" HeaderText="Nota da Crítica" DecimalDigits="1" />
                       <telerik:GridCheckBoxColumn DataField="adult" HeaderText="Filme +18" />
                       <telerik:GridBoundColumn DataField="release_date" HeaderText="Lançamento" DataFormatString="{0:dd/MM/yyyy}" ShowFilterIcon="false">
                            <FilterTemplate>
                                <asp:DropDownList ID="ddlFiltroEpoca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEpoca_SelectedIndexChanged" 
                                    style="width: 100%; border-radius: 4px; padding: 4px; background: rgba(16,25,43,0.8); color: #38bdf8; border: 1px solid #38bdf8;">
                                    <asp:ListItem Text="[X] Limpar Filtro (Todas as Datas)" Value="Todos"></asp:ListItem>
                                    <asp:ListItem Text="Recentes (ultimos 3 anos)" Value="Recentes"></asp:ListItem>
                                    <asp:ListItem Text="Classicos (antigos anos 90 e 80)" Value="Classicos"></asp:ListItem>
                                </asp:DropDownList>
                            </FilterTemplate>
                       </telerik:GridBoundColumn>

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
            
            <PagerStyle FirstPageToolTip="Primeira Página" LastPageToolTip="Última Página" NextPagesToolTip="Próximas Páginas" NextPageToolTip="Próxima Página" PrevPagesToolTip="Páginas Anteriores" PrevPageToolTip="Página Anterior" PageSizeLabelText="Tamanho da Página:" PagerTextFormat="Mudar página: {4} &amp;nbsp;Página &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, itens &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." />
            
        </telerik:RadGrid>
        </div>

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
    </div>
</body>
</html>
