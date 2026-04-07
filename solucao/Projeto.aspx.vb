Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Configuration
Imports Telerik.Web.UI.Gantt

Partial Class Projeto
    Inherits System.Web.UI.Page

    ' Evento executado a cada carregamento da tela
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CarregarDashboardAnalitico()
        End If
    End Sub

    ' Nossa Rotina Customizada de BI
    Private Sub CarregarDashboardAnalitico()
        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        
        Using conexaoSql As New SqlConnection(strConexao)
            conexaoSql.Open()

            ' 1. Busca Ágil: Acervo Total
            Using cmdTotal As New SqlCommand("SELECT COUNT(id) FROM Movies", conexaoSql)
                lblTotalFilmes.Text = cmdTotal.ExecuteScalar().ToString()
            End Using

            ' 2. Busca Ágil: Filme Mais Popular do Mundo
            Using cmdPopular As New SqlCommand("SELECT TOP 1 title FROM Movies ORDER BY popularity DESC", conexaoSql)
                Dim filmePop = cmdPopular.ExecuteScalar()
                lblMaisPopular.Text = If(filmePop IsNot Nothing, filmePop.ToString(), "Nenhum")
            End Using

            ' 3. Busca Ágil: Maior Nota Geral da Crítica
            Using cmdNota As New SqlCommand("SELECT TOP 1 title FROM Movies ORDER BY vote_average DESC", conexaoSql)
                Dim filmeNota = cmdNota.ExecuteScalar()
                lblMaiorNota.Text = If(filmeNota IsNot Nothing, filmeNota.ToString(), "Nenhum")
            End Using
        End Using
    End Sub
    Protected Sub GridFilmes_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)

        ' 1. Aqui ele vai lá no Web.config e lê aquele link ("Data Source=.\SQLEX...")
        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        ' 2. Cria uma planilha vazia na memória do servidor
        Dim tabelaVazia As New DataTable()

        Using conexaoSql As New SqlConnection(strConexao)

            ' 3. Cria a ponte oficial com o Banco SqlConnection
            ' Construtor de Filtro de Epoca Especial (FilterTemplate)
            ' Construindo o Lançamento Inteligente
            Dim sqlFiltroExtra As String = ""
            If ViewState("FiltroEpoca") IsNot Nothing Then
                Dim epoca As String = ViewState("FiltroEpoca").ToString()
                If epoca = "Recentes" Then
                    sqlFiltroExtra = " AND release_date >= DATEADD(year, -3, GETDATE())"
                ElseIf epoca = "Classicos" Then
                    sqlFiltroExtra = " AND release_date < '2000-01-01'"
                End If
            End If

            ' Traduz a barragem de funil do Telerik nativo em TSQL cru
            Dim radGridFiltro As String = ""
            If Not String.IsNullOrEmpty(GridFilmes.MasterTableView.FilterExpression) Then
                Dim filtroTelerik As String = GridFilmes.MasterTableView.FilterExpression
                ' Usa Regex para matar qualquer variacao de espacamento (ex: '=True' ou '=  True')
                filtroTelerik = System.Text.RegularExpressions.Regex.Replace(filtroTelerik, "=\s*True\b", "= 'True'", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                filtroTelerik = System.Text.RegularExpressions.Regex.Replace(filtroTelerik, "=\s*False\b", "= 'False'", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                radGridFiltro = " AND " & filtroTelerik
            End If

            ' PAGINAÇÃO ENTERPRISE DE DADOS
            ' ETAPA 1: O SQL responde qual o Ponto de Extensão das páginas baseado nos filtros VirtualItemCount
            Dim totalRegistros As Integer = 0
            Dim countQuery As String = "SELECT COUNT(id) FROM Movies WHERE 1=1 " & sqlFiltroExtra & radGridFiltro

            ' Abrimos a conexão manualmente com checagem de estado para proteger contra Exception de Estado Duplo
            If conexaoSql.State = ConnectionState.Closed Then
                conexaoSql.Open()
            End If

            Using comandoCount As New SqlCommand(countQuery, conexaoSql)
                Try
                    totalRegistros = Convert.ToInt32(comandoCount.ExecuteScalar())
                Catch ex As Exception
                End Try
            End Using
            GridFilmes.VirtualItemCount = totalRegistros

            ' ETAPA 2: Calculando os Pulos para transferir apenas 10 MB em vez de 40.000 (OFFSET)
            Dim pgOffset As Integer = GridFilmes.CurrentPageIndex * GridFilmes.PageSize
            Dim pgFetch As Integer = GridFilmes.PageSize

            ' Blindagem Master: Usamos TRY_CAST(id AS INT). Qualquer id sujo vai pro fundo e nao quebra a pesquisa!
            Dim comandoDeBusca As String = "SELECT id, title, overview, release_date, vote_average, adult FROM Movies WHERE 1=1 " & sqlFiltroExtra & radGridFiltro & " ORDER BY TRY_CAST(id AS INT) DESC OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY"

            ' 4. Disparo!
            Using comandoSql As New SqlCommand(comandoDeBusca, conexaoSql)
                comandoSql.Parameters.AddWithValue("@offset", pgOffset)
                comandoSql.Parameters.AddWithValue("@fetch", pgFetch)


                Using adaptador As New SqlDataAdapter(comandoSql)



                    adaptador.Fill(tabelaVazia)

                End Using
            End Using
        End Using

        ' 4. aqui retorna em que a fonte de dados do GridFilmes agora esta planilha está cheia!
        GridFilmes.DataSource = tabelaVazia

    End Sub

    ' Método: abrimos o Menu de Filtro do Telerik e forçamos o português!
    Protected Sub GridFilmes_Init(sender As Object, e As EventArgs)
        Dim menu As Telerik.Web.UI.GridFilterMenu = GridFilmes.FilterMenu

        ' Removemos cirurgicamente todas as opcoes nativas inuteis do Telerik
        Dim itensPermitidos As New List(Of String) From {"NoFilter", "Contains", "EqualTo", "GreaterThan", "LessThan"}
        For i As Integer = menu.Items.Count - 1 To 0 Step -1
            If Not itensPermitidos.Contains(menu.Items(i).Value) Then
                menu.Items.RemoveAt(i)
            End If
        Next


        For Each item As Telerik.Web.UI.RadMenuItem In menu.Items
            If item.Value = "NoFilter" Then item.Text = " Limpar Todos os Filtros"
            If item.Value = "Contains" Then item.Text = " Contem a palavra..."
            If item.Value = "EqualTo" Then item.Text = " Pesquisa Exata (Especial Filme +18)"
            If item.Value = "GreaterThan" Then item.Text = " Maior que (Aclamados pela Critica)"
            If item.Value = "LessThan" Then item.Text = " Menor que (Avaliacao Negativa)"
        Next
    End Sub


    'Operação de deletar'

    Protected Sub GridFilmes_DeleteCommand(sender As Object, e As GridCommandEventArgs)

        'CAPTURA FISICAMENTE APENAS DA LINHAS'
        Dim ItemClicado As GridDataItem = CType(e.Item, GridDataItem)

        Dim idDoFilme As String = ItemClicado.GetDataKeyValue("id").ToString()

        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conexaoSql As New SqlConnection(strConexao)

            Dim comandoDeExclusao As String = "Delete FROM Movies Where id = @id"

            Using comandoSql As New SqlCommand(comandoDeExclusao, conexaoSql)

                comandoSql.Parameters.AddWithValue("@id", idDoFilme)

                conexaoSql.Open()

                Try
                    comandoSql.ExecuteNonQuery()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemSucessoDelete", "alert('[SUCESSO] Registro ELIMINADO permanentemente!');", True)
                Catch ex As Exception
                    Dim msg As String = ex.Message.Replace("'", "\'")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemErroDelete", "alert('[ERRO] Ops! Falha ao deletar: " & msg & "');", True)
                    e.Canceled = True
                End Try

            End Using


        End Using

    End Sub



    'Operação de atualização'
    Protected Sub GridFilmes_UpdateCommand(sender As Object, e As GridCommandEventArgs)

        'Seleção da linha inteira em modo de edição'
        Dim itemEditado As GridEditableItem = CType(e.Item, GridEditableItem)

        'Arrancar a ancora principal (ID original)'
        Dim idDoFilme As String = itemEditado.GetDataKeyValue("id").ToString()

        'pegamos tudo que o usuário digitou e jogamos em uma hashtable'

        Dim Valores As New Hashtable()

        itemEditado.ExtractValues(Valores)

        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conexaoSql As New SqlConnection(strConexao)

            Dim comandoDeUpdate As String = "UPDATE Movies Set title = @title, overview = @overview, release_date = @release_date, vote_average = @vote_average, adult = @adult WHERE id = @id"

            Using comandoSql As New SqlCommand(comandoDeUpdate, conexaoSql)

                ' Parametros blindados sql injection.
                comandoSql.Parameters.AddWithValue("@id", idDoFilme)
                comandoSql.Parameters.AddWithValue("@title", If(Valores("title"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@overview", If(Valores("overview"), DBNull.Value))

                Dim notaEditada As Double = 0
                If Valores("vote_average") IsNot Nothing Then Double.TryParse(Valores("vote_average").ToString(), notaEditada)
                comandoSql.Parameters.AddWithValue("@vote_average", notaEditada)

                Dim isAdult As String = "False"
                If Valores("adult") IsNot Nothing AndAlso Valores("adult").ToString().ToLower() = "true" Then isAdult = "True"
                comandoSql.Parameters.AddWithValue("@adult", isAdult)

                ' Tratamento Militar de Datas EXTREMO: previne textos digitados em formatos alienígenas ou letras aleatórias
                Dim dataEditada As String = Convert.ToString(Valores("release_date"))
                Dim dtFiltradaUpdate As DateTime

                Dim culturaBR As New System.Globalization.CultureInfo("pt-BR")
                If DateTime.TryParseExact(dataEditada, {"dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy"}, culturaBR, System.Globalization.DateTimeStyles.None, dtFiltradaUpdate) Then
                    comandoSql.Parameters.AddWithValue("@release_date", dtFiltradaUpdate)
                ElseIf DateTime.TryParse(dataEditada, culturaBR, System.Globalization.DateTimeStyles.None, dtFiltradaUpdate) Then
                    comandoSql.Parameters.AddWithValue("@release_date", dtFiltradaUpdate)
                Else
                    comandoSql.Parameters.AddWithValue("@release_date", DateTime.Now) ' Caiu pro plano B caso a data seja impossível de ler
                End If

                conexaoSql.Open()

                Try
                    comandoSql.ExecuteNonQuery()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemSucessoUpdate", "alert('[SUCESSO] Filme ATUALIZADO com extremo sucesso!');", True)
                Catch ex As Exception
                    Dim msg As String = ex.Message.Replace("'", "\'")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemErroUpdate", "alert('[ERRO] Ops! Falha na atualizacao: " & msg & "');", True)
                    e.Canceled = True
                End Try

            End Using

        End Using

    End Sub

    'Operação de criaçao do Insert/criação'

    Protected Sub GridFilmes_InsertCommand(sender As Object, e As GridCommandEventArgs)

        'capturação do form em branco'
        Dim itemNovo As GridEditableItem = CType(e.Item, GridEditableItem)

        Dim valores As New Hashtable()

        itemNovo.ExtractValues(valores)
        'extrai todas as mensagens do form'

        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conexaoSql As New SqlConnection(strConexao)
            conexaoSql.Open()


            ' Força calcular o máximo blindando com TRY_CAST! Evita tela azul com dados sujos Kaggle.
            Dim cmdMaxId As New SqlCommand("SELECT ISNULL(MAX(TRY_CAST(id AS INT)),0) + 1 FROM Movies", conexaoSql)
            Dim proximoId As Integer = Convert.ToInt32(cmdMaxId.ExecuteScalar())

            ' Inserimos o id manualmente na Query e forçamos "zeros" genéricos nas outras colunas para tapar os buracos NOT NULL do banco
            Dim rnd As New Random()
            Dim popularidadeFalsa As Double = rnd.Next(15000, 95000)
            Dim notaFalsa As Double = Math.Round((rnd.NextDouble() * (9.5 - 6.0) + 6.0), 1) ' Notas fantasmas de fallback
            Dim votosFalsos As Integer = rnd.Next(100, 5000)

            Dim comandoDeInsert As String = "INSERT INTO Movies (id, title, overview, release_date, vote_count, vote_average, popularity, adult, original_language, column1, title2) VALUES (@id, @title, @overview, @release_date, @votos, @nota, @pop, @adult, 'pt-BR', 0, @title)"

            Using comandoSql As New SqlCommand(comandoDeInsert, conexaoSql)

                ' Captura a nova nota digitada ou gera a Falsa caso a pessoa tenha deixado em branco
                Dim notaFinal As Double = 0
                If valores("vote_average") IsNot Nothing AndAlso Double.TryParse(valores("vote_average").ToString(), notaFinal) Then
                    comandoSql.Parameters.AddWithValue("@nota", notaFinal)
                Else
                    comandoSql.Parameters.AddWithValue("@nota", notaFalsa)
                End If

                ' Captura se marcaram a caixinha Filme +18
                Dim insertAdulto As String = "False"
                If valores("adult") IsNot Nothing AndAlso valores("adult").ToString().ToLower() = "true" Then insertAdulto = "True"
                comandoSql.Parameters.AddWithValue("@adult", insertAdulto)

                comandoSql.Parameters.AddWithValue("@votos", votosFalsos)
                comandoSql.Parameters.AddWithValue("@pop", popularidadeFalsa)

                comandoSql.Parameters.AddWithValue("@id", proximoId)
                comandoSql.Parameters.AddWithValue("@title", If(valores("title"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@overview", If(valores("overview"), DBNull.Value))

                Dim dataNova As String = Convert.ToString(valores("release_date"))
                Dim dtFiltradaInsert As DateTime
                Dim culturaInsert As New System.Globalization.CultureInfo("pt-BR")

                If DateTime.TryParseExact(dataNova, {"dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy"}, culturaInsert, System.Globalization.DateTimeStyles.None, dtFiltradaInsert) Then
                    comandoSql.Parameters.AddWithValue("@release_date", dtFiltradaInsert)
                ElseIf DateTime.TryParse(dataNova, culturaInsert, System.Globalization.DateTimeStyles.None, dtFiltradaInsert) Then
                    comandoSql.Parameters.AddWithValue("@release_date", dtFiltradaInsert)
                Else
                    comandoSql.Parameters.AddWithValue("@release_date", DateTime.Now)
                End If

                Try

                    comandoSql.ExecuteNonQuery()

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemSucessoInsert", "alert('[SUCESSO] Filme CADASTRADO com sucesso no Acervo!');", True)

                Catch ex As Exception

                    Dim msg As String = ex.Message.Replace("'", "\'")

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MensagemErroInsert", "alert('[ERRO] O Cadastro falhou! Acesso Negado ou Erro Critico: " & msg & "');", True)

                    e.Canceled = True

                End Try


            End Using

        End Using

    End Sub

    ' Gatilho de Customizacao de Datas!
    Protected Sub ddlFiltroEpoca_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = CType(sender, DropDownList)
        ViewState("FiltroEpoca") = ddl.SelectedValue
        GridFilmes.Rebind()
    End Sub

    ' Faz a tela lembrar qual opção você clicou (Evita que o Telerik resete o Dropdown virtualmente)!
    Protected Sub GridFilmes_ItemDataBound(sender As Object, e As GridItemEventArgs)
        If TypeOf e.Item Is GridFilteringItem Then
            Dim filterItem As GridFilteringItem = CType(e.Item, GridFilteringItem)
            Dim ddl As DropDownList = CType(filterItem.FindControl("ddlFiltroEpoca"), DropDownList)
            If ddl IsNot Nothing AndAlso ViewState("FiltroEpoca") IsNot Nothing Then
                ddl.SelectedValue = ViewState("FiltroEpoca").ToString()
            End If
        End If
    End Sub

End Class

