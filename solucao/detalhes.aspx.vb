Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration

Partial Class detalhes
    Inherits System.Web.UI.Page

    ' Esse evento dispara sozinho no exato milissegundo em que a página nasce na tela
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Not IsPostBack Then

            Dim idDoFilme As String = Request.QueryString("id")

            If Not String.IsNullOrEmpty(idDoFilme) Then
                CarregarDetalhesDoFilme(idDoFilme)
            End If

        End If
    End Sub

    Private Sub CarregarDetalhesDoFilme(idParaBuscar As String)
        Dim strConexao As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim tabelaVazia As New DataTable()

        Using conexaoSql As New SqlConnection(strConexao)

            ' O Grande Momento: SELECT * para ler todas as 11 colunas que retirei antes
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

        GridDetalhes.DataSource = tabelaVazia
        GridDetalhes.DataBind()
    End Sub
End Class
