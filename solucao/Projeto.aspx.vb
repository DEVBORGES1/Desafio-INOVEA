Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Configuration
Imports Telerik.Web.UI.Gantt

Partial Class Projeto
    Inherits System.Web.UI.Page

    Private ReadOnly Property StringConexao As String
        Get
            Return ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        End Get
    End Property

    Private Sub ExibirMensagem(texto As String, Optional sucesso As Boolean = True)
        lblMensagem.Visible = True
        lblMensagem.Text = texto
        lblMensagem.CssClass = If(sucesso, "mensagem sucesso", "mensagem erro")
    End Sub


    Protected Sub GridFilmes_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)

        ' 1. Aqui ele vai lá no Web.config e lê aquele link ("Data Source=.\SQLEX...")
        ' 2. Cria uma planilha vazia na memória do servidor
        Dim tabelaVazia As New DataTable()

        Using conexaoSql As New SqlConnection(StringConexao)

            ' 3. Cria a ponte oficial com o Banco SqlConnection
            Dim comandoDeBusca As String = "SELECT TOP 100 id, title, overview, release_date FROM Movies"

            ' 4. Cria e faz a conexão com a Pergunta a Conexão
            Using comandoSql As New SqlCommand(comandoDeBusca, conexaoSql)


                Using adaptador As New SqlDataAdapter(comandoSql)


                    conexaoSql.Open()

                    ' 5. Joga todas os dados que ele achou dentro planilha Vazia
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

        For Each item As Telerik.Web.UI.RadMenuItem In menu.Items
            If item.Text = "NoFilter" Then item.Text = "Sem Filtro"
            If item.Text = "Contains" Then item.Text = "Contém"
            If item.Text = "EqualTo" Then item.Text = "Igual a"
            If item.Text = "GreaterThan" Then item.Text = "Maior que"
            If item.Text = "LessThan" Then item.Text = "Menor que"
            If item.Text = "StartsWith" Then item.Text = "Começa com"
            If item.Text = "EndsWith" Then item.Text = "Termina com"
            If item.Text = "NotEqualTo" Then item.Text = "Diferente de"
            If item.Text = "GreaterThanOrEqualTo" Then item.Text = "Maior ou igual a"
            If item.Text = "LessThanOrEqualTo" Then item.Text = "Menor ou igual a"
            If item.Text = "IsEmpty" Then item.Text = "É vazio"
            If item.Text = "NotIsEmpty" Then item.Text = "Não é vazio"
            If item.Text = "IsNull" Then item.Text = "É nulo"
            If item.Text = "NotIsNull" Then item.Text = "Não é nulo"
            If item.Text = "Between" Then item.Text = "Entre"
            If item.Text = "NotBetween" Then item.Text = "Não está entre"
            If item.Text = "DoesNotContain" Then item.Text = "Não contém"
        Next
    End Sub


    'Operação de deletar'

    Protected Sub GridFilmes_DeleteCommand(sender As Object, e As GridCommandEventArgs)

        'CAPTURA FISICAMENTE APENAS DA LINHAS'
        Dim ItemClicado As GridDataItem = CType(e.Item, GridDataItem)
        Dim idDoFilme As Integer = Convert.ToInt32(ItemClicado.GetDataKeyValue("id"))

        Try
            Using conexaoSql As New SqlConnection(StringConexao)

                Dim comandoDeExclusao As String = "Delete FROM Movies Where id = @id"

                Using comandoSql As New SqlCommand(comandoDeExclusao, conexaoSql)

                    comandoSql.Parameters.Add("@id", SqlDbType.Int).Value = idDoFilme

                    conexaoSql.Open()
                    comandoSql.ExecuteNonQuery()

                End Using

            End Using

            ExibirMensagem("Filme removido com sucesso.")
        Catch ex As Exception
            ExibirMensagem("Não foi possível excluir o filme agora. Tente novamente.", False)
            e.Canceled = True
        End Try

    End Sub



    'Operação de atualização'
    Protected Sub GridFilmes_UpdateCommand(sender As Object, e As GridCommandEventArgs)

        'Seleção da linha inteira em modo de edição'
        Dim itemEditado As GridEditableItem = CType(e.Item, GridEditableItem)

        'Arrancar a ancora principal (ID original)'
        Dim idDoFilme As Integer = Convert.ToInt32(itemEditado.GetDataKeyValue("id"))

        'pegamos tudo que o usuário digitou e jogamos em uma hashtable'

        Dim Valores As New Hashtable()

        itemEditado.ExtractValues(Valores)

        If Valores("title") Is Nothing OrElse String.IsNullOrWhiteSpace(Valores("title").ToString()) Then
            ExibirMensagem("O título é obrigatório para salvar alterações.", False)
            e.Canceled = True
            Return
        End If

        Try
            Using conexaoSql As New SqlConnection(StringConexao)

                Dim comandoDeUpdate As String = "UPDATE Movies Set title = @title, overview = @overview, release_date = @release_date WHERE id = @id"

                Using comandoSql As New SqlCommand(comandoDeUpdate, conexaoSql)

                    'Parametros blindados sql injection. o comando if(Dbnull.value) salva nulo'

                    comandoSql.Parameters.Add("@id", SqlDbType.Int).Value = idDoFilme
                    comandoSql.Parameters.Add("@title", SqlDbType.NVarChar, 255).Value = Valores("title").ToString().Trim()
                    comandoSql.Parameters.Add("@overview", SqlDbType.NVarChar).Value = If(Valores("overview"), DBNull.Value)
                    comandoSql.Parameters.Add("@release_date", SqlDbType.Date).Value = If(Valores("release_date"), DBNull.Value)

                    conexaoSql.Open()
                    comandoSql.ExecuteNonQuery()

                End Using

            End Using

            ExibirMensagem("Filme atualizado com sucesso.")
        Catch ex As Exception
            ExibirMensagem("Não foi possível atualizar o filme agora.", False)
            e.Canceled = True
        End Try

    End Sub

    'Operação de criaçao do Insert/criação'

    Protected Sub GridFilmes_InsertCommand(sender As Object, e As GridCommandEventArgs)

        'capturação do form em branco'
        Dim itemNovo As GridEditableItem = CType(e.Item, GridEditableItem)

        Dim valores As New Hashtable()

        itemNovo.ExtractValues(valores)
        'extrai todas as mensagens do form'

        If valores("title") Is Nothing OrElse String.IsNullOrWhiteSpace(valores("title").ToString()) Then
            ExibirMensagem("Informe um título para cadastrar o filme.", False)
            e.Canceled = True
            Return
        End If

        Try
            Using conexaoSql As New SqlConnection(StringConexao)

                ' não ultilizar o WHERE , Nós empurramos colunas e valores brutos!'

                Dim comandoDeInsert As String = "INSERT INTO Movies (title, overview, release_date) VALUES (@title, @overview, @release_date)"

                Using comandoSql As New SqlCommand(comandoDeInsert, conexaoSql)

                    'parametros'

                    comandoSql.Parameters.Add("@title", SqlDbType.NVarChar, 255).Value = valores("title").ToString().Trim()
                    comandoSql.Parameters.Add("@overview", SqlDbType.NVarChar).Value = If(valores("overview"), DBNull.Value)
                    comandoSql.Parameters.Add("@release_date", SqlDbType.Date).Value = If(valores("release_date"), DBNull.Value)

                    conexaoSql.Open()
                    comandoSql.ExecuteNonQuery()

                End Using

            End Using

            ExibirMensagem("Filme cadastrado com sucesso.")
        Catch ex As Exception
            ExibirMensagem("Não foi possível cadastrar o filme agora.", False)
            e.Canceled = True
        End Try

    End Sub

    Protected Sub GridFilmes_ItemCommand(sender As Object, e As GridCommandEventArgs)
        If e.CommandName = "VerDetalhes" Then
            Dim itemClicado As GridDataItem = CType(e.Item, GridDataItem)
            Dim idDoFilme As Integer = Convert.ToInt32(itemClicado.GetDataKeyValue("id"))
            Response.Redirect("detalhes.aspx?id=" & idDoFilme.ToString())
        End If
    End Sub

End Class
