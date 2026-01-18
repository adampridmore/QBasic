Imports System.Drawing

Namespace SpinHouse
    ''' <summary>
    ''' Represents a 3D point/vertex in the house model
    ''' Based on cube() array in SPINHOUS.BAS:21
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
        ' SPINHOUS.BAS:3 - CONST pi = 3.141592654#
        Private Const Pi As Single = 3.141592654F
        Private Points As List(Of Point3D)
        Private CenterX, CenterY, CenterZ As Single
        ' SPINHOUS.BAS:6 - angle = 5
        Private RotationAngle As Single = 5.0F
        ' SPINHOUS.BAS:9-10 - c# = COS(angle / 180 * pi), s# = SIN(angle / 180 * pi)
        Private CosAngle, SinAngle As Single

        ' SPINHOUS.BAS:7 - dir = 0
        Public RotationDirection As Integer = 0 ' -1 = left, 0 = stopped, 1 = right

        Public Sub New()
            InitializeHouse()
            CalculateRotationConstants()
        End Sub

        ' SPINHOUS.BAS:9-10
        Private Sub CalculateRotationConstants()
            CosAngle = CSng(Math.Cos(RotationAngle / 180.0 * Pi))
            SinAngle = CSng(Math.Sin(RotationAngle / 180.0 * Pi))
        End Sub

        Private Sub InitializeHouse()
            Points = New List(Of Point3D)()

            ' House vertices from SPINHOUS.BAS:27-146
            ' cube(n, 1-3) = X, Y, Z coordinates
            ' cube(n, 4+) = connected point indices
            ' Note: QBasic was 1-indexed, VB.Net is 0-indexed

            ' SPINHOUS.BAS:27-32 - Point 1 (now 0)
            Points.Add(New Point3D(1, 0, 0, 1, 2, 4))        ' 0
            ' SPINHOUS.BAS:33-38 - Point 2 (now 1)
            Points.Add(New Point3D(2, 0, 0, 5, 3, 19))       ' 1
            ' SPINHOUS.BAS:39-43 - Point 3 (now 2)
            Points.Add(New Point3D(1, 0, 1, 3, 7))           ' 2
            ' SPINHOUS.BAS:44-48 - Point 4 (now 3)
            Points.Add(New Point3D(2, 0, 1, 6, 13))          ' 3
            ' SPINHOUS.BAS:49-54 - Point 5 (now 4)
            Points.Add(New Point3D(1, 1, 0, 5, 8, 7))        ' 4
            ' SPINHOUS.BAS:55-59 - Point 6 (now 5)
            Points.Add(New Point3D(2, 1, 0, 6, 9))           ' 5
            ' SPINHOUS.BAS:60-64 - Point 7 (now 6)
            Points.Add(New Point3D(2, 1, 1, 9, 7))           ' 6
            ' SPINHOUS.BAS:65-68 - Point 8 (now 7)
            Points.Add(New Point3D(1, 1, 1, 8))              ' 7
            ' SPINHOUS.BAS:69-72 - Point 9 (now 8) - roof peak
            Points.Add(New Point3D(1.2F, 1.5F, 0.5F, 9))     ' 8
            ' SPINHOUS.BAS:73-75 - Point 10 (now 9) - roof peak
            Points.Add(New Point3D(1.8F, 1.5F, 0.5F))        ' 9

            ' Door frame points - SPINHOUS.BAS:76-112
            Points.Add(New Point3D(2, 0.2F, 0, 18))          ' 10 (was 11)
            Points.Add(New Point3D(2, 0.2F, 1, 12))          ' 11 (was 12)
            Points.Add(New Point3D(3, 0.2F, 1, 14, 13))      ' 12 (was 13)
            Points.Add(New Point3D(3, 0, 1, 19))             ' 13 (was 14)
            Points.Add(New Point3D(3, 0.2F, 0.55F, 15))      ' 14 (was 15)
            Points.Add(New Point3D(3, 0, 0.55F))             ' 15 (was 16)
            Points.Add(New Point3D(3, 0.2F, 0.45F, 18, 17))  ' 16 (was 17)
            Points.Add(New Point3D(3, 0, 0.45F, 19))         ' 17 (was 18)
            Points.Add(New Point3D(3, 0.2F, 0, 19))          ' 18 (was 19)
            Points.Add(New Point3D(3, 0, 0))                 ' 19 (was 20)

            ' Window frame - SPINHOUS.BAS:116-131
            Points.Add(New Point3D(2, 0, 0.45F, 20, 22))     ' 20 (was 21)
            Points.Add(New Point3D(2, 0, 0.55F, 23))         ' 21 (was 22)
            Points.Add(New Point3D(2, 0.3F, 0.45F, 23))      ' 22 (was 23)
            Points.Add(New Point3D(2, 0.3F, 0.55F))          ' 23 (was 24)

            ' Origin axes marker - SPINHOUS.BAS:132-146
            Points.Add(New Point3D(0, 0, 0, 25, 26, 27))     ' 24 (was 25)
            Points.Add(New Point3D(0.2F, 0, 0))              ' 25 (was 26)
            Points.Add(New Point3D(0, 0.2F, 0))              ' 26 (was 27)
            Points.Add(New Point3D(0, 0, 0.2F))              ' 27 (was 28)

            ' SPINHOUS.BAS:156-160 - Translate house to center it
            For Each pt In Points
                pt.X = pt.X - 1.5F   ' SPINHOUS.BAS:157
                pt.Y = pt.Y + 3.5F   ' SPINHOUS.BAS:158
                pt.Z = pt.Z - 0.5F   ' SPINHOUS.BAS:159
            Next

            ' SPINHOUS.BAS:168-175 - Apply initial 90-degree rotation around X axis
            ' c2# = COS(pi / 2), s2# = SIN(pi / 2)
            Dim cos90 As Single = CSng(Math.Cos(Pi / 2))
            Dim sin90 As Single = CSng(Math.Sin(Pi / 2))

            CalculateCenter()

            For Each pt In Points
                ' SPINHOUS.BAS:169-174
                Dim tempY = cos90 * (pt.Y - CenterY) + sin90 * (pt.Z - CenterZ)
                Dim tempZ = -sin90 * (pt.Y - CenterY) + cos90 * (pt.Z - CenterZ)
                pt.Y = tempY + CenterY
                pt.Z = tempZ + CenterZ
            Next

            CalculateCenter()
        End Sub

        ' SPINHOUS.BAS:164-166, 205-207 - Calculate center of house
        Private Sub CalculateCenter()
            If Points.Count >= 7 Then
                CenterX = ((Points(6).X - Points(0).X) / 2) + Points(0).X
                CenterY = ((Points(6).Y - Points(0).Y) / 2) + Points(0).Y
                CenterZ = ((Points(6).Z - Points(0).Z) / 2) + Points(0).Z
            End If
        End Sub

        ''' <summary>
        ''' Update house rotation based on current direction
        ''' SPINHOUS.BAS:211-219 - Rotation transformation
        ''' </summary>
        Public Sub Update()
            If RotationDirection <> 0 Then
                CalculateCenter()
                For Each pt In Points
                    ' SPINHOUS.BAS:213-218
                    Dim tempX = CosAngle * (pt.X - CenterX) + RotationDirection * SinAngle * (pt.Y - CenterY)
                    Dim tempY = -RotationDirection * SinAngle * (pt.X - CenterX) + CosAngle * (pt.Y - CenterY)
                    pt.X = tempX + CenterX
                    pt.Y = tempY + CenterY
                Next
            End If
        End Sub

        ''' <summary>
        ''' Move house closer/further (along Y axis)
        ''' SPINHOUS.BAS:221-230 - K/M key handlers
        ''' </summary>
        Public Sub MoveY(delta As Single)
            For Each pt In Points
                pt.Y = pt.Y + delta
            Next
        End Sub

        ''' <summary>
        ''' Project 3D point to screen X coordinate
        ''' SPINHOUS.BAS:238-243 - FUNCTION screenposx
        ''' </summary>
        Private Function ScreenPosX(x As Single, y As Single, z As Single) As Single
            If y = 0 Then Return -1
            Return x / y
        End Function

        ''' <summary>
        ''' Project 3D point to screen Y coordinate
        ''' SPINHOUS.BAS:246-252 - FUNCTION screenposy
        ''' </summary>
        Private Function ScreenPosY(x As Single, y As Single, z As Single) As Single
            If y = 0 Then Return -1
            Return z / y
        End Function

        ''' <summary>
        ''' Render the house to the given graphics context
        ''' SPINHOUS.BAS:185-202 - Main rendering loop
        ''' </summary>
        Public Sub Render(g As Graphics, width As Integer, height As Integer)
            Using pen As New Pen(Color.Cyan, 1.5F)
                ' SPINHOUS.BAS:185 - FOR lop = 1 TO cubesize
                For i = 0 To Points.Count - 1
                    Dim pt = Points(i)
                    ' SPINHOUS.BAS:187-188
                    Dim x = ScreenPosX(pt.X, pt.Y, pt.Z)
                    Dim y = ScreenPosY(pt.X, pt.Y, pt.Z)

                    ' SPINHOUS.BAS:189 - Skip if out of bounds
                    If y < -0.5F OrElse x < -0.5F OrElse x > 0.5F OrElse y > 0.5F Then
                        Continue For
                    End If

                    ' SPINHOUS.BAS:190-200 - Draw lines to connected points
                    For Each connIndex In pt.Connections
                        If connIndex >= 0 AndAlso connIndex < Points.Count Then
                            Dim pt2 = Points(connIndex)
                            ' SPINHOUS.BAS:192-193
                            Dim x1 = ScreenPosX(pt2.X, pt2.Y, pt2.Z)
                            Dim y1 = ScreenPosY(pt2.X, pt2.Y, pt2.Z)

                            ' SPINHOUS.BAS:194 - Skip if out of bounds
                            If x1 < -0.5F OrElse y1 < -0.5F OrElse x1 > 0.5F OrElse y1 > 0.5F Then
                                Continue For
                            End If

                            ' SPINHOUS.BAS:195 - LINE statement
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
