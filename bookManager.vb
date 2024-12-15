Imports Microsoft.Data.SqlClient

Public Class BookManager
    Private connectionString As String

    ' Default constructor that sets the connection string
    Public Sub New()
        ' Update this with your actual connection string
        Me.connectionString = "Server=DESKTOP-H54BSR0\SQLEXPRESS;User ID=sa;Password=12345678;Database=Books;TrustServerCertificate=True"
    End Sub

    ' Constructor with custom connection string
    Public Sub New(connectionString As String)
        Me.connectionString = connectionString
    End Sub

    Public Function GetBookDetails(bookName As String) As Dictionary(Of String, String)
        Dim bookDetails As New Dictionary(Of String, String)

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT col_bookName, col_Author, col_Pdate, col_status FROM tbl_books WHERE col_bookName = @bookName"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@bookName", bookName)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' If the book is found
                    Dim status As Boolean = Convert.ToBoolean(reader("col_status"))
                    If Not status Then
                        bookDetails("col_status") = "0" ' Book is not available
                    Else
                        bookDetails("col_bookName") = reader("col_bookName").ToString()
                        bookDetails("col_Author") = reader("col_Author").ToString()
                        bookDetails("col_Pdate") = Convert.ToDateTime(reader("col_Pdate")).ToString("yyyy-MM-dd")
                        bookDetails("col_status") = "1" ' Book is available
                    End If
                Else
                    bookDetails("col_bookName") = "Book not found"
                    bookDetails("col_Author") = "-"
                    bookDetails("col_Pdate") = "-"
                    bookDetails("col_status") = "0"
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching book details: " & ex.Message)
        End Try

        Return bookDetails
    End Function
End Class
