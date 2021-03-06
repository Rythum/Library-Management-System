﻿Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class returnBook

    ' Min function to return book via its accession number
    ' First search for accession number in Borrowed table. If found then set issued to false there and reduce book count by one in users table for that table
    Private Sub issueButton_Click(sender As Object, e As EventArgs) Handles issueButton.Click
        If AccNoTextBox.Text = "" Then
            MessageBox.Show("Enter correct credentials")
        ElseIf Not Regex.IsMatch(AccNoTextBox.Text, "^[0-9]+$") Then
            MessageBox.Show("Enter correct Book id")
        Else
            Dim connectionString = MainPage.connectionString
            Dim cn As OleDbConnection = New OleDbConnection(connectionString)
            Dim cmdString = "select * from Borrowed where AccNo = " & AccNoTextBox.Text & ""
            Dim cmd As OleDbCommand = New OleDbCommand(cmdString, cn)
            cn.Open()
            Dim reader As OleDbDataReader = cmd.ExecuteReader
            reader.Read()
            Dim ISBN As String = ""
            If Not reader.HasRows Then
                MessageBox.Show("Book does not exist.Please add a book first")
                Return
            ElseIf Not reader("IsIssued") Then
                MessageBox.Show("This Book is not issued")

                AccNoTextBox.Text = ""
                Return
            End If

            ISBN = reader("ISBN")
            Dim UserID As String = reader("BorrowerId")
            reader.Close()
            Dim issue_date As String = ""
            cmd.CommandText = "update Borrowed set IsIssued=False where AccNo = " & AccNoTextBox.Text & ""
            cmd.ExecuteScalar()
            cmd.CommandText = "select * from Users where UserName='" & UserID & "'"
            reader = cmd.ExecuteReader
            reader.Read()
            Dim newCount As Integer = reader("BooksIssued") - 1
            cmd.CommandText = "update Users set BooksIssued=" & newCount & " where UserName='" & UserID & "'"
            reader.Close()
            cmd.ExecuteReader()

            Dim cmdString4 As String = "select * from Books where ISBN='" & ISBN & "'"
            Dim cmd4 As OleDbCommand = New OleDbCommand(cmdString4, cn)
            cmd4.CommandText = cmdString4
            Dim reader4 As OleDbDataReader = cmd4.ExecuteReader
            reader4.Read()
            Dim remaining As Integer = reader4("Remaining")
            reader4.Close()

            remaining += 1

            Dim cmdString3 As String = "update Books set Remaining=" & remaining & " where ISBN='" & ISBN & "'"
            Dim cmd3 As OleDbCommand = New OleDbCommand(cmdString3, cn)
            cmd3.CommandText = cmdString3
            cmd3.ExecuteNonQuery()


            AccNoTextBox.Text = ""
            MessageBox.Show("Book Returned")
        End If

    End Sub

    Private Sub returnBook_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
