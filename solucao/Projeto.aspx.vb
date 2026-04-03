Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Configuration

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

    ' Método infalível: Nós mesmos abrimos o Menu de Filtro do Telerik e forçamos o português!
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

End Class

