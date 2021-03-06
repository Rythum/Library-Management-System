﻿Imports System.Data.OleDb
Imports System.IO
Imports System.Data.SqlClient

Public Class ItemsCheckedOut_UserControl

    ' Function to show checked out books
    Private Sub ItemsCheckedOut_UserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cn As OleDbConnection = New OleDbConnection(MainPage.connectionString)
        cn.Open()

        Dim selectString As String = "SELECT * FROM Borrowed WHERE BorrowerId = '" & StudentLogin.UserName & "' and IsIssued=True"
        Dim cmd As OleDbCommand = New OleDbCommand(selectString, cn)
        Dim reader As OleDbDataReader = cmd.ExecuteReader()

        Dim boxWidth As Integer = (Panel1.Width / 3) - 20
        Dim boxHeight As Integer = 200
        Dim nextX As Integer = 0
        Dim nextY As Integer = 0

        While reader.Read
            Dim selectString2 As String = "SELECT * FROM Books WHERE ISBN = '" & reader("ISBN") & "'"
            Dim cmd2 As OleDbCommand = New OleDbCommand(selectString2, cn)
            Dim reader2 As OleDbDataReader = cmd2.ExecuteReader()

            If reader2.Read Then

                'Dim backPicBox As New PictureBox

                Dim fullPath As String = System.IO.Path.GetFullPath(Application.StartupPath & "\..\..\Resources\") & reader2("BookImage")

                Dim Titlelabel As New LinkLabel
                Dim Authorlabel As New Label
                Dim Publisherlabel As New Label

                Dim Locationlabel As New Label
                Dim Remaininglabel As New Label
                Dim pictureBox As New PictureBox

                Titlelabel.Size = New Size(140, 20)
                Authorlabel.Size = New Size(140, 20)
                Publisherlabel.Size = New Size(140, 20)
                Locationlabel.Size = New Size(140, 20)
                pictureBox.Size = New Size(150, 150)
                'backPicBox.Size = New Size(boxWidth - 10, boxHeight)

                pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
                Titlelabel.Tag = reader2("ISBN")
                pictureBox.Tag = fullPath
                pictureBox.ImageLocation = fullPath
                Titlelabel.Text = "Title: " & reader2("Title")
                Authorlabel.Text = "Author: " & reader2("Author")
                Publisherlabel.Text = "Publisher: " & reader2("Publisher")
                Locationlabel.Text = "Location: " & reader2("Location")

                pictureBox.Location = New Point(nextX, nextY)
                Titlelabel.Location = New Point(nextX + 160, nextY)
                Authorlabel.Location = New Point(nextX + 160, nextY + 25)
                Publisherlabel.Location = New Point(nextX + 160, nextY + 50)
                Locationlabel.Location = New Point(nextX + 160, nextY + 75)
                'backPicBox.Location = New Point(nextX, nextY)

                Titlelabel.Font = New Font("Times New Roman", 11, FontStyle.Regular)
                Authorlabel.Font = New Font("Times New Roman", 11, FontStyle.Regular)
                Publisherlabel.Font = New Font("Times New Roman", 11, FontStyle.Regular)
                Locationlabel.Font = New Font("Times New Roman", 11, FontStyle.Regular)

                'Titlelabel.ForeColor = Color.White
                'Authorlabel.ForeColor = Color.White
                'Publisherlabel.ForeColor = Color.White
                'Locationlabel.ForeColor = Color.White

                'backPicBox.BackColor = Color.MidnightBlue
                'Titlelabel.BackColor = Color.MidnightBlue
                'Authorlabel.BackColor = Color.MidnightBlue
                'Publisherlabel.BackColor = Color.MidnightBlue
                'Locationlabel.BackColor = Color.MidnightBlue

                'backPicBox.SendToBack()

                Titlelabel.AutoEllipsis = True
                Authorlabel.AutoEllipsis = True
                Publisherlabel.AutoEllipsis = True
                Locationlabel.AutoEllipsis = True

                Me.Panel1.Controls.Add(Titlelabel)
                Me.Panel1.Controls.Add(pictureBox)
                Me.Panel1.Controls.Add(Authorlabel)
                Me.Panel1.Controls.Add(Publisherlabel)
                Me.Panel1.Controls.Add(Locationlabel)
                'Me.Panel1.Controls.Add(backPicBox)
                AddHandler Titlelabel.Click, AddressOf Titlelabel_click

                nextX += boxWidth
                If nextX > 2 * (Panel1.Width / 3) Then
                    nextX = 0
                    nextY += boxHeight
                End If

            End If

        End While

        cn.Close()
    End Sub

    ' Show book details by clicking on title 
    ' The form opened will allow to rate that particular book
    Private Sub Titlelabel_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim frm As New RatingBookDetails
        frm.passISBN = sender.tag
        frm.ShowDialog()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class