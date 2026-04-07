<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detalhes.aspx.vb" Inherits="detalhes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Arquivo Extras - Detalhes do Filme</title>
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
            margin-bottom: 5px;
        }
        p {
            color: #94a3b8;
            margin-bottom: 25px;
            border-bottom: 1px solid rgba(255,255,255,0.1);
            padding-bottom: 15px;
        }
        /* Proteção Responsiva de Quebra de Tela para a Tabela de Detalhes */
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
        
        .btn-voltar {
            display: inline-block;
            margin-top: 25px;
            padding: 12px 24px; 
            background: linear-gradient(90deg, #0284c7, #2563eb); 
            color: white; 
            text-decoration: none; 
            border-radius: 8px; 
            font-weight: 600;
            box-shadow: 0 4px 15px rgba(37, 99, 235, 0.4);
            transition: all 0.3s ease;
        }
        .btn-voltar:hover { /* Efeito de Brilho */
            box-shadow: 0 6px 20px rgba(37, 99, 235, 0.6);
            transform: translateY(-2px);
        }
    </style>
</head>
<body>
    <div class="glass-container">
    <form id="form1" runat="server">
        
       
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>

        <h2 style="font-family: 'Outfit', sans-serif;"> Relatório Extras sobre o filme selecionado</h2>
        <p>Abaixo estão as informações estendidas (Popularidade, Notas, Flag Adulto) cruzadas pelo sistema:</p>

        <div class="grid-wrapper">
        <telerik:RadGrid id="GridDetalhes" runat="server" AutoGenerateColumns="false" Skin="Bootstrap">
            <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true" FileName="Detalhes_Do_Filme">
                <Pdf PageWidth="297mm" PageHeight="210mm" DefaultFontFamily="Arial" />
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top">
                <CommandItemSettings ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false" />

                <NoRecordsTemplate>
                    Nenhum filme foi encontrado com esse ID secreto!
                </NoRecordsTemplate>

                <Columns>

                    <telerik:GridTemplateColumn HeaderText="Poster Oficial">
                        <ItemTemplate>
                            <img id='imgPoster_<%# Eval("id") %>' src="https://ui-avatars.com/api/?name=Loading&background=1e293b&color=fff" style="width: 150px; border-radius: 10px; box-shadow: 0 8px 20px rgba(0,0,0,0.5);" />
                            <input type="hidden" id='hfTitulo_<%# Eval("id") %>' value='<%# Eval("title") %>' />
                            
                            <script type="text/javascript">
                                // FRONTEND - API OMDB: Consumo de API OMDb pelo Cliente!
                                setTimeout(function() {
                                    var imagem = document.getElementById('imgPoster_<%# Eval("id") %>');
                                    var titulo = document.getElementById('hfTitulo_<%# Eval("id") %>').value;
                                    
                                    fetch('https://www.omdbapi.com/?apikey=319ec42f&t=' + encodeURIComponent(titulo))
                                    .then(res => res.json())
                                    .then(data => {
                                        if(data.Poster && data.Poster !== "N/A") {
                                            imagem.src = data.Poster; // Imagem Oficial
                                        } else {
                                            // Se for um filme customizado/inventado, geramos um Avatar!
                                            imagem.src = "https://ui-avatars.com/api/?name=" + encodeURIComponent(titulo) + "&background=1e293b&color=38bdf8&size=512";
                                        }
                                    });
                                }, 100);
                            </script>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="id" HeaderText="Código Oficial" />
                    <telerik:GridBoundColumn DataField="title" HeaderText="Título" />
                    <telerik:GridBoundColumn DataField="original_language" HeaderText="Idioma" />
                    <telerik:GridBoundColumn DataField="adult" HeaderText="Filme +18" />
                    <telerik:GridBoundColumn DataField="popularity" HeaderText="Índice de Popularidade" />
                    <telerik:GridBoundColumn DataField="vote_average" HeaderText="Nota Média" />
                    <telerik:GridBoundColumn DataField="vote_count" HeaderText="Nº de Votos" />
                    <telerik:GridBoundColumn DataField="release_date" HeaderText="Lançamento" DataFormatString="{0:dd/MM/yyyy}" />
                    <telerik:GridBoundColumn DataField="overview" HeaderText="Sinopse" />

                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        </div>

        <a href="Projeto.aspx" class="btn-voltar"> Retornar a pagina inicial</a>
        
    </form>
    </div>
</body>
</html>
