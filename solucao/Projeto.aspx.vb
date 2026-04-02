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

End Class

