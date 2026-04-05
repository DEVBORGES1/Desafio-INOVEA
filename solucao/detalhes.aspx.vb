Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration

Partial Class detalhes
    Inherits System.Web.UI.Page

    ' Esse evento dispara sozinho no exato milissegundo em que a página nasce na tela
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Só foca no banco caso a pessoa esteja entrando na página de forma fresca
        If Not IsPostBack Then

            ' 1. Pesca o ID invisível que o nosso Javascript atirou lá em cima na Barra de URL
            Dim idDoFilme As String = Request.QueryString("id")

            ' Proteção: Se a pessoa tentar acessar a tela solta, sem ID nenhum, o sistema ignora.
            If Not String.IsNullOrEmpty(idDoFilme) Then
                CarregarDetalhesDoFilme(idDoFilme)
            End If

        End If
    End Sub

    Private Sub CarregarDetalhesDoFilme(idParaBuscar As String)
        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim tabelaVazia As New DataTable()

        Using conexaoSql As New SqlConnection(strConexao)

            ' O Grande Momento: SELECT * para ler todas as 11 colunas que escondemos antes
            Dim comandoDeBusca As String = "SELECT * FROM Movies WHERE id = @id"

            Using comandoSql As New SqlCommand(comandoDeBusca, conexaoSql)

                ' Parâmetro Tipado sempre blindado contra SQL Injection
                comandoSql.Parameters.AddWithValue("@id", idParaBuscar)

                Using adaptador As New SqlDataAdapter(comandoSql)
                    conexaoSql.Open()
                    adaptador.Fill(tabelaVazia) ' Despeja a única linha achada do filme na memória
                End Using
            End Using
        End Using

        ' Pegamos a panela com o filme e servimos no nosso novo Grid vazio da tela HTML!
        GridDetalhes.DataSource = tabelaVazia
        GridDetalhes.DataBind()
    End Sub
End Class
