Imports System.Data
Imports MySql.Data.MySqlClient

Public Class DMSQL
    Public Shared conn As MySqlConnection
    Public Shared status As Boolean = False

    ' Run any SQL you build
    Private Shared Sub RunSql(sql As String)
        Try
            If status Then
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.ExecuteNonQuery()
                End Using
            End If
        Catch
        End Try
    End Sub

    ' 1) Connect to MySQL server
    Public Shared Sub Connect(db As String, ip As String, port As String, user As String, pass As String)
        Try
            conn = New MySqlConnection(
                $"server={ip};port={port};user={user};password={pass};database={db};")
            conn.Open()
            status = True
        Catch
            status = False
        End Try
    End Sub
    Public Shared Sub Disconnect()
        Try
            If status AndAlso conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch
        Finally
            status = False
        End Try
    End Sub

    ' 2) Create a new table (auto-ID only)
    Public Shared Sub CreateTableSimple(tableName As String)
        RunSql($"CREATE TABLE IF NOT EXISTS `{tableName}` (ID INT AUTO_INCREMENT PRIMARY KEY);")
    End Sub

    ' 3) Add a new field of a given VB type
    Public Shared Sub AddFieldSimple(tableName As String, fieldName As String, vbType As String)
        Dim sqlType As String
        Select Case vbType.ToLower()
            Case "string" : sqlType = "VARCHAR(255)"
            Case "integer" : sqlType = "INT"
            Case "double" : sqlType = "DOUBLE"
            Case "date" : sqlType = "DATETIME"
            Case "boolean" : sqlType = "TINYINT(1)"
            Case "decimal" : sqlType = "DECIMAL(18,2)"
            Case Else : sqlType = "TEXT"
        End Select

        RunSql($"ALTER TABLE `{tableName}` ADD COLUMN `{fieldName}` {sqlType};")
    End Sub

    ' 4) Insert one piece of data into one field
    Public Shared Sub InsertSimple(tableName As String, fieldName As String, value As String)
        RunSql($"INSERT INTO `{tableName}` (`{fieldName}`) VALUES ('{value}');")
    End Sub

    ' 5) Update one field by record ID
    Public Shared Sub UpdateSimple(tableName As String, fieldName As String, newValue As String, id As Integer)
        RunSql($"UPDATE `{tableName}` SET `{fieldName}` = '{newValue}' WHERE ID = {id};")
    End Sub

    ' 6) Delete one record by ID
    Public Shared Sub DeleteSimple(tableName As String, id As Integer)
        RunSql($"DELETE FROM `{tableName}` WHERE ID = {id};")
    End Sub

    ' 7) Lookup a single cell and return it as String
    Public Shared Function GetValueSimple(tableName As String, fieldName As String, id As Integer) As String
        Try
            If Not status Then Return String.Empty
            Dim sql = $"SELECT `{fieldName}` FROM `{tableName}` WHERE ID = {id} LIMIT 1;"
            Using cmd As New MySqlCommand(sql, conn)
                Dim result = cmd.ExecuteScalar()
                Return If(result Is Nothing OrElse IsDBNull(result), String.Empty, result.ToString())
            End Using
        Catch
            Return String.Empty
        End Try
    End Function

    ' ————————————————
    ' DataGridView Helpers
    ' ————————————————

    ' 1) View-only, hide ID
    Public Shared Sub ViewNoID(tableName As String, dgv As DataGridView, txtSearch As TextBox)
        SetupGrid(tableName, dgv, txtSearch, showID:=False, editable:=False, btnSave:=Nothing)
    End Sub

    ' 2) View-only, show ID
    Public Shared Sub ViewWithID(tableName As String, dgv As DataGridView, txtSearch As TextBox)
        SetupGrid(tableName, dgv, txtSearch, showID:=True, editable:=False, btnSave:=Nothing)
    End Sub

    ' 3) Editable, hide ID (requires a Save button)
    Public Shared Sub EditNoID(tableName As String, dgv As DataGridView, txtSearch As TextBox, btnSave As Button)
        SetupGrid(tableName, dgv, txtSearch, showID:=False, editable:=True, btnSave:=btnSave)
    End Sub

    ' 4) Editable, show ID (requires a Save button)
    Public Shared Sub EditWithID(tableName As String, dgv As DataGridView, txtSearch As TextBox, btnSave As Button)
        SetupGrid(tableName, dgv, txtSearch, showID:=True, editable:=True, btnSave:=btnSave)
    End Sub

    ' Internal common logic
    Private Shared Sub SetupGrid(tableName As String,
                                 dgv As DataGridView,
                                 txtSearch As TextBox,
                                 showID As Boolean,
                                 editable As Boolean,
                                 btnSave As Button)
        Dim adapter = New MySqlDataAdapter($"SELECT * FROM `{tableName}`;", conn)
        Dim table = New DataTable()
        adapter.Fill(table)

        Dim source = New BindingSource()
        source.DataSource = table
        dgv.DataSource = source

        If Not showID AndAlso dgv.Columns.Contains("ID") Then
            dgv.Columns("ID").Visible = False
        End If

        dgv.ReadOnly = Not editable
        If editable AndAlso dgv.Columns.Contains("ID") Then
            dgv.Columns("ID").ReadOnly = True
        End If

        If editable AndAlso btnSave IsNot Nothing Then
            AddHandler btnSave.Click, Sub()
                                          Dim builder = New MySqlCommandBuilder(adapter)
                                          adapter.Update(table)
                                          table.AcceptChanges()
                                      End Sub
        End If

        AddHandler txtSearch.TextChanged, Sub()
                                              If txtSearch.Text = "" Then
                                                  source.RemoveFilter()
                                              Else
                                                  Dim filters = New List(Of String)
                                                  For Each col As DataColumn In table.Columns
                                                      If col.DataType Is GetType(String) Then
                                                          filters.Add($"`{col.ColumnName}` LIKE '%{txtSearch.Text}%'")
                                                      End If
                                                  Next
                                                  source.Filter = String.Join(" OR ", filters)
                                              End If
                                          End Sub
    End Sub

End Class
