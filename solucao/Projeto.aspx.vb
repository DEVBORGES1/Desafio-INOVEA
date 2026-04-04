Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Configuration
Imports Telerik.Web.UI.Gantt

Partial Class Projeto
    Inherits System.Web.UI.Page


    Protected Sub GridFilmes_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)

        ' 1. Aqui ele vai lá no Web.config e lê aquele link ("Data Source=.\SQLEX...")
        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        ' 2. Cria uma planilha vazia na memória do servidor
        Dim tabelaVazia As New DataTable()

        Using conexaoSql As New SqlConnection(strConexao)

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

        Dim idDoFilme As String = ItemClicado.GetDataKeyValue("id").ToString()

        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using conexaoSql As New SqlConnection(strConexao)

            Dim comandoDeExclusao As String = "Delete FROM Movies Where id = @id"

            Using comandoSql As New SqlCommand(comandoDeExclusao, conexaoSql)

                comandoSql.Parameters.AddWithValue("@id", idDoFilme)

                conexaoSql.Open()
                comandoSql.ExecuteNonQuery()

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

            Dim comandoDeUpdate As String = "UPDATE Movies Set title = @title, overview = @overview, release_date = @release_date WHERE id = @id"

            Using comandoSql As New SqlCommand(comandoDeUpdate, conexaoSql)

                'Parametros blindados sql injection. o comando if(Dbnull.value) salva nulo'

                comandoSql.Parameters.AddWithValue("@id", idDoFilme)
                comandoSql.Parameters.AddWithValue("@title", If(Valores("title"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@overview", If(Valores("overview"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@release_date", If(Valores("release_date"), DBNull.Value))

                conexaoSql.Open()


                comandoSql.ExecuteNonQuery()

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

            ' não ultilizar o WHERE , Nós empurramos colunas e valores brutos!'

            Dim comandoDeInsert As String = "INSERT INTO Movies (id, title, overview, release_date) VALUES (@id, @title, @overview, @release_date)"

            Using comandoSql As New SqlCommand(comandoDeInsert, conexaoSql)

                'parametros'

                comandoSql.Parameters.AddWithValue("@id", If(valores("id"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@title", If(valores("title"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@overview", If(valores("overview"), DBNull.Value))
                comandoSql.Parameters.AddWithValue("@release_date", If(valores("release_date"), DBNull.Value))

                conexaoSql.Open()

                comandoSql.ExecuteNonQuery()

            End Using

        End Using

    End Sub

End Class

