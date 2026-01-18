Imports System.Drawing

Namespace SpinHouse
    ''' <summary>
    ''' Represents a 3D point/vertex in the house model
    ''' </summary>
    Public Class Point3D
        Public X As Single
        Public Y As Single
        Public Z As Single
        Public Connections As List(Of Integer)

        Public Sub New(x As Single, y As Single, z As Single)
            Me.X = x
            Me.Y = y
            Me.Z = z
            Me.Connections = New List(Of Integer)()
        End Sub

        Public Sub New(x As Single, y As Single, z As Single, ParamArray connections() As Integer)
            Me.X = x
            Me.Y = y
            Me.Z = z
            Me.Connections = New List(Of Integer)(connections)
        End Sub
    End Class

    ''' <summary>
    ''' Contains the 3D house model data and transformation logic
    ''' Ported from SPINHOUS.BAS
    ''' </summary>
    Public Class House3D
        Private Const Pi As Single = 3.141592654F
        Private Points As List(Of Point3D)
        Private CenterX, CenterY, CenterZ As Single
        Private RotationAngle As Single = 5.0F
        Private CosAngle, SinAngle As Single

        Public RotationDirection As Integer = 0 ' -1 = left, 0 = stopped, 1 = right

        Public Sub New()
            InitializeHouse()
            CalculateRotationConstants()
        End Sub

        Private Sub CalculateRotationConstants()
            CosAngle = CSng(Math.Cos(RotationAngle / 180.0 * Pi))
            SinAngle = CSng(Math.Sin(RotationAngle / 180.0 * Pi))
        End Sub

        Private Sub InitializeHouse()
            Points = New List(Of Point3D)()

            ' House vertices from original SPINHOUS.BAS
            ' Point 0 (was 1 in QBasic - 1-indexed)
            Points.Add(New Point3D(1, 0, 0, 1, 2, 4))        ' 0
            Points.Add(New Point3D(2, 0, 0, 5, 3, 19))       ' 1
            Points.Add(New Point3D(1, 0, 1, 3, 7))           ' 2
            Points.Add(New Point3D(2, 0, 1, 6, 13))          ' 3
            Points.Add(New Point3D(1, 1, 0, 5, 8, 7))        ' 4
            Points.Add(New Point3D(2, 1, 0, 6, 9))           ' 5
            Points.Add(New Point3D(2, 1, 1, 9, 7))           ' 6
            Points.Add(New Point3D(1, 1, 1, 8))              ' 7
            Points.Add(New Point3D(1.2F, 1.5F, 0.5F, 9))     ' 8 - roof peak
            Points.Add(New Point3D(1.8F, 1.5F, 0.5F))        ' 9 - roof peak

            ' Door frame points
            Points.Add(New Point3D(2, 0.2F, 0, 18))          ' 10
            Points.Add(New Point3D(2, 0.2F, 1, 12))          ' 11
            Points.Add(New Point3D(3, 0.2F, 1, 14, 13))      ' 12
            Points.Add(New Point3D(3, 0, 1, 19))             ' 13
            Points.Add(New Point3D(3, 0.2F, 0.55F, 15))      ' 14
            Points.Add(New Point3D(3, 0, 0.55F))             ' 15
            Points.Add(New Point3D(3, 0.2F, 0.45F, 18, 17))  ' 16
            Points.Add(New Point3D(3, 0, 0.45F, 19))         ' 17
            Points.Add(New Point3D(3, 0.2F, 0, 19))          ' 18
            Points.Add(New Point3D(3, 0, 0))                 ' 19

            ' Window frame
            Points.Add(New Point3D(2, 0, 0.45F, 20, 22))     ' 20
            Points.Add(New Point3D(2, 0, 0.55F, 23))         ' 21
            Points.Add(New Point3D(2, 0.3F, 0.45F, 23))      ' 22
            Points.Add(New Point3D(2, 0.3F, 0.55F))          ' 23

            ' Origin axes marker
            Points.Add(New Point3D(0, 0, 0, 25, 26, 27))     ' 24
            Points.Add(New Point3D(0.2F, 0, 0))              ' 25
            Points.Add(New Point3D(0, 0.2F, 0))              ' 26
            Points.Add(New Point3D(0, 0, 0.2F))              ' 27

            ' Translate house to center it (from original QBasic code)
            For Each pt In Points
                pt.X = pt.X - 1.5F
                pt.Y = pt.Y + 3.5F
                pt.Z = pt.Z - 0.5F
            Next

            ' Apply initial 90-degree rotation around X axis (from original)
            Dim cos90 As Single = CSng(Math.Cos(Pi / 2))
            Dim sin90 As Single = CSng(Math.Sin(Pi / 2))

            CalculateCenter()

            For Each pt In Points
                Dim tempY = cos90 * (pt.Y - CenterY) + sin90 * (pt.Z - CenterZ)
                Dim tempZ = -sin90 * (pt.Y - CenterY) + cos90 * (pt.Z - CenterZ)
                pt.Y = tempY + CenterY
                pt.Z = tempZ + CenterZ
            Next

            CalculateCenter()
        End Sub

        Private Sub CalculateCenter()
            ' Calculate center based on points 0 and 6 (corners of the house)
            If Points.Count >= 7 Then
                CenterX = ((Points(6).X - Points(0).X) / 2) + Points(0).X
                CenterY = ((Points(6).Y - Points(0).Y) / 2) + Points(0).Y
                CenterZ = ((Points(6).Z - Points(0).Z) / 2) + Points(0).Z
            End If
        End Sub

        ''' <summary>
        ''' Update house rotation based on current direction
        ''' </summary>
        Public Sub Update()
            If RotationDirection <> 0 Then
                CalculateCenter()
                For Each pt In Points
                    Dim tempX = CosAngle * (pt.X - CenterX) + RotationDirection * SinAngle * (pt.Y - CenterY)
                    Dim tempY = -RotationDirection * SinAngle * (pt.X - CenterX) + CosAngle * (pt.Y - CenterY)
                    pt.X = tempX + CenterX
                    pt.Y = tempY + CenterY
                Next
            End If
        End Sub

        ''' <summary>
        ''' Move house closer/further (along Y axis)
        ''' </summary>
        Public Sub MoveY(delta As Single)
            For Each pt In Points
                pt.Y = pt.Y + delta
            Next
        End Sub

        ''' <summary>
        ''' Project 3D point to screen X coordinate
        ''' </summary>
        Private Function ScreenPosX(x As Single, y As Single, z As Single) As Single
            If y = 0 Then Return -1
            Return x / y
        End Function

        ''' <summary>
        ''' Project 3D point to screen Y coordinate
        ''' </summary>
        Private Function ScreenPosY(x As Single, y As Single, z As Single) As Single
            If y = 0 Then Return -1
            Return z / y
        End Function

        ''' <summary>
        ''' Render the house to the given graphics context
        ''' </summary>
        Public Sub Render(g As Graphics, width As Integer, height As Integer)
            Using pen As New Pen(Color.Cyan, 1.5F)
                For i = 0 To Points.Count - 1
                    Dim pt = Points(i)
                    Dim x = ScreenPosX(pt.X, pt.Y, pt.Z)
                    Dim y = ScreenPosY(pt.X, pt.Y, pt.Z)

                    ' Skip if point is behind camera or out of bounds
                    If y < -0.5F OrElse x < -0.5F OrElse x > 0.5F OrElse y > 0.5F Then
                        Continue For
                    End If

                    ' Draw lines to connected points
                    For Each connIndex In pt.Connections
                        If connIndex >= 0 AndAlso connIndex < Points.Count Then
                            Dim pt2 = Points(connIndex)
                            Dim x1 = ScreenPosX(pt2.X, pt2.Y, pt2.Z)
                            Dim y1 = ScreenPosY(pt2.X, pt2.Y, pt2.Z)

                            ' Skip if connected point is out of bounds
                            If x1 < -0.5F OrElse y1 < -0.5F OrElse x1 > 0.5F OrElse y1 > 0.5F Then
                                Continue For
                            End If

                            ' Convert normalized coordinates to screen coordinates
                            Dim screenX1 = CInt((x + 0.5F) * width)
                            Dim screenY1 = CInt((y + 0.5F) * height)
                            Dim screenX2 = CInt((x1 + 0.5F) * width)
                            Dim screenY2 = CInt((y1 + 0.5F) * height)

                            g.DrawLine(pen, screenX1, screenY1, screenX2, screenY2)
                        End If
                    Next
                Next
            End Using
        End Sub
    End Class
End Namespace
