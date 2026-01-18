Imports System.Drawing

Namespace Editor3D
    ''' <summary>
    ''' Renders the 3D world using wireframe graphics
    ''' </summary>
    Public Class Renderer3D
        Private World As World3D
        Private Camera As Camera3D

        ' QBasic color palette mapping to .NET colors
        Private Shared ReadOnly ColorPalette() As Color = {
            Color.Black,        ' 0
            Color.DarkBlue,     ' 1
            Color.DarkGreen,    ' 2
            Color.DarkCyan,     ' 3
            Color.DarkRed,      ' 4
            Color.DarkMagenta,  ' 5
            Color.Olive,        ' 6
            Color.LightGray,    ' 7
            Color.DarkGray,     ' 8
            Color.Blue,         ' 9
            Color.Lime,         ' 10
            Color.Cyan,         ' 11
            Color.Red,          ' 12
            Color.Magenta,      ' 13
            Color.Yellow,       ' 14
            Color.White         ' 15
        }

        Public Sub New(world As World3D, camera As Camera3D)
            Me.World = world
            Me.Camera = camera
        End Sub

        ''' <summary>
        ''' Get .NET color from QBasic color index
        ''' </summary>
        Public Shared Function GetColor(index As Integer) As Color
            If index >= 0 AndAlso index < ColorPalette.Length Then
                Return ColorPalette(index)
            End If
            Return Color.White
        End Function

        ''' <summary>
        ''' Render the entire scene
        ''' </summary>
        Public Sub Render(g As Graphics, width As Integer, height As Integer, Optional highlightIndex As Integer = -1, Optional highlightColor As Integer = 9)
            ' Draw horizon line
            Dim horizonHeight = Camera.GetHorizonHeight(height)
            Dim twist = Camera.GetHorizonTwist()
            Using horizonPen As New Pen(Color.DarkGreen, 1)
                g.DrawLine(horizonPen, 0, horizonHeight + twist, width, horizonHeight - twist)
            End Using

            ' Render all objects
            For objIndex = 0 To World3D.MaxObjects - 1
                Dim obj = World.Objects(objIndex)
                If Not obj.IsUsed Then Continue For

                ' Determine color (highlight if selected)
                Dim objColor As Color
                If objIndex = highlightIndex Then
                    objColor = GetColor(highlightColor)
                Else
                    objColor = GetColor(obj.Color)
                End If

                Using pen As New Pen(objColor, 1.5F)
                    RenderObject(g, obj, pen, width, height)
                End Using
            Next
        End Sub

        Private Sub RenderObject(g As Graphics, obj As Object3D, pen As Pen, width As Integer, height As Integer)
            ' Iterate through all points in this object
            For partIndex = obj.PointBegin To obj.PointEnd
                If partIndex < 0 OrElse partIndex >= World3D.MaxPoints Then Continue For

                Dim pt = World.Points(partIndex)
                If Not pt.IsUsed Then Continue For

                ' Transform point to camera space
                Dim camX1, camY1, camZ1 As Single
                Dim worldX1 = pt.X + obj.PosX
                Dim worldY1 = pt.Y + obj.PosY
                Dim worldZ1 = pt.Z + obj.PosZ
                Camera.TransformPoint(worldX1, worldY1, worldZ1, camX1, camY1, camZ1)

                ' Skip if no links
                If pt.LinkBegin = -1 Then Continue For

                ' Draw lines to all linked points
                For linkIndex = pt.LinkBegin To pt.LinkEnd
                    If linkIndex < 0 OrElse linkIndex >= World3D.MaxLinks Then Continue For

                    Dim linkedPointIndex = World.Links(linkIndex)
                    If linkedPointIndex < 0 OrElse linkedPointIndex >= World3D.MaxPoints Then Continue For

                    Dim linkedPt = World.Points(linkedPointIndex)

                    ' Transform linked point to camera space
                    Dim camX2, camY2, camZ2 As Single
                    Dim worldX2 = linkedPt.X + obj.PosX
                    Dim worldY2 = linkedPt.Y + obj.PosY
                    Dim worldZ2 = linkedPt.Z + obj.PosZ
                    Camera.TransformPoint(worldX2, worldY2, worldZ2, camX2, camY2, camZ2)

                    ' Project both points to screen
                    Dim screenX1, screenY1, screenX2, screenY2 As Single
                    Dim visible1 = Camera.ProjectToScreen(camX1, camY1, camZ1, screenX1, screenY1)
                    Dim visible2 = Camera.ProjectToScreen(camX2, camY2, camZ2, screenX2, screenY2)

                    ' Only draw if both points are visible and in bounds
                    If visible1 AndAlso visible2 Then
                        ' Convert normalized coordinates to screen pixels
                        Dim pixelX1 = CInt((screenX1 + 0.5F) * width)
                        Dim pixelY1 = CInt((screenY1 + 0.5F) * height)
                        Dim pixelX2 = CInt((screenX2 + 0.5F) * width)
                        Dim pixelY2 = CInt((screenY2 + 0.5F) * height)

                        ' Clip to screen bounds
                        If pixelX1 >= -width AndAlso pixelX1 <= width * 2 AndAlso
                           pixelY1 >= -height AndAlso pixelY1 <= height * 2 AndAlso
                           pixelX2 >= -width AndAlso pixelX2 <= width * 2 AndAlso
                           pixelY2 >= -height AndAlso pixelY2 <= height * 2 Then
                            g.DrawLine(pen, pixelX1, pixelY1, pixelX2, pixelY2)
                        End If
                    End If
                Next
            Next
        End Sub
    End Class
End Namespace
