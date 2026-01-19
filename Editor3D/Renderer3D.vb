Imports Avalonia
Imports Avalonia.Media

Namespace Editor3D
    ''' <summary>
    ''' Renders the 3D world using wireframe graphics
    ''' Based on EDITOR.BAS:968-1044 - disp: subroutine
    ''' </summary>
    Public Class Renderer3D
        Private World As World3D
        Private Camera As Camera3D

        ' QBasic color palette mapping to Avalonia colors
        ' EDITOR.BAS used COLOR statement with indices 0-15
        ' SCREEN 7 supported 16 colors from the EGA palette
        Private Shared ReadOnly ColorPalette() As Color = {
            Colors.Black,        ' 0
            Colors.DarkBlue,     ' 1
            Colors.DarkGreen,    ' 2 - used for trees
            Colors.DarkCyan,     ' 3
            Colors.DarkRed,      ' 4
            Colors.DarkMagenta,  ' 5
            Colors.Olive,        ' 6
            Colors.LightGray,    ' 7 - used for house
            Colors.DarkGray,     ' 8
            Colors.Blue,         ' 9 - default highlight color (EDITOR.BAS:57 - col = 9)
            Colors.Lime,         ' 10
            Colors.Cyan,         ' 11
            Colors.Red,          ' 12
            Colors.Magenta,      ' 13
            Colors.Yellow,       ' 14 - used for ground
            Colors.White         ' 15
        }

        Public Sub New(world As World3D, camera As Camera3D)
            Me.World = world
            Me.Camera = camera
        End Sub

        ''' <summary>
        ''' Get Avalonia color from QBasic color index
        ''' </summary>
        Public Shared Function GetColor(index As Integer) As Color
            If index >= 0 AndAlso index < ColorPalette.Length Then
                Return ColorPalette(index)
            End If
            Return Colors.White
        End Function

        ''' <summary>
        ''' Render the entire scene
        ''' EDITOR.BAS:261-268, 968-1044 - Main rendering code
        ''' </summary>
        Public Sub Render(context As DrawingContext, width As Integer, height As Integer, Optional highlightIndex As Integer = -1, Optional highlightColor As Integer = 9)
            ' EDITOR.BAS:262-265 - Draw horizon line
            ' COLOR 2
            ' horizonheight = -200 * SIN(camerarotx) + 100
            ' twist = 100 * TAN(cameraroty)
            ' LINE (0, horizonheight + twist)-(320, horizonheight - twist)
            Dim horizonHeight = Camera.GetHorizonHeight(height)
            Dim twist = Camera.GetHorizonTwist()
            Dim horizonPen As New Pen(New SolidColorBrush(Colors.DarkGreen), 1)
            context.DrawLine(horizonPen, New Point(0, horizonHeight + twist), New Point(width, horizonHeight - twist))

            ' EDITOR.BAS:970-1042 - Render all objects
            ' FOR currentobj = 1 TO 100
            For objIndex = 0 To World3D.MaxObjects - 1
                Dim obj = World.Objects(objIndex)
                ' EDITOR.BAS:971 - IF NOT (object(currentobj).called = "*********") THEN
                If Not obj.IsUsed Then Continue For

                ' Determine color (highlight if selected)
                ' EDITOR.BAS:767-780 - highlight mode changes object color temporarily
                Dim objColor As Color
                If objIndex = highlightIndex Then
                    objColor = GetColor(highlightColor)
                Else
                    objColor = GetColor(obj.Color)
                End If

                Dim pen As New Pen(New SolidColorBrush(objColor), 1.5)
                RenderObject(context, obj, pen, width, height)
            Next
        End Sub

        ''' <summary>
        ''' Render a single object
        ''' EDITOR.BAS:972-1040 - Inner object rendering loop
        ''' </summary>
        Private Sub RenderObject(context As DrawingContext, obj As Object3D, pen As Pen, width As Integer, height As Integer)
            ' EDITOR.BAS:972 - FOR part = object(currentobj).beg TO object(currentobj).fin
            For partIndex = obj.PointBegin To obj.PointEnd
                If partIndex < 0 OrElse partIndex >= World3D.MaxPoints Then Continue For

                Dim pt = World.Points(partIndex)
                If Not pt.IsUsed Then Continue For

                ' EDITOR.BAS:975-977 - Translate object to virtual object
                ' x1 = pt(part).x - cameraposx + object(currentobj).posx
                Dim camX1, camY1, camZ1 As Single
                Dim worldX1 = pt.X + obj.PosX
                Dim worldY1 = pt.Y + obj.PosY
                Dim worldZ1 = pt.Z + obj.PosZ
                Camera.TransformPoint(worldX1, worldY1, worldZ1, camX1, camY1, camZ1)

                ' EDITOR.BAS:997 - IF NOT (pt(part).beg = -1) THEN
                If pt.LinkBegin = -1 Then Continue For

                ' EDITOR.BAS:998 - FOR linknumber = pt(part).beg TO pt(part).fin
                For linkIndex = pt.LinkBegin To pt.LinkEnd
                    If linkIndex < 0 OrElse linkIndex >= World3D.MaxLinks Then Continue For

                    ' EDITOR.BAS:1001 - x2 = pt(link(linknumber)).x ...
                    Dim linkedPointIndex = World.Links(linkIndex)
                    If linkedPointIndex < 0 OrElse linkedPointIndex >= World3D.MaxPoints Then Continue For

                    Dim linkedPt = World.Points(linkedPointIndex)

                    ' EDITOR.BAS:1001-1003 - Transform linked point
                    Dim camX2, camY2, camZ2 As Single
                    Dim worldX2 = linkedPt.X + obj.PosX
                    Dim worldY2 = linkedPt.Y + obj.PosY
                    Dim worldZ2 = linkedPt.Z + obj.PosZ
                    Camera.TransformPoint(worldX2, worldY2, worldZ2, camX2, camY2, camZ2)

                    ' EDITOR.BAS:1023-1034 - Project to screen coordinates
                    ' IF NOT (y1 = 0 OR y2 = 0) THEN
                    '     screenx1 = x1 / y1
                    '     screeny1 = z1 / y1
                    Dim screenX1, screenY1, screenX2, screenY2 As Single
                    Dim visible1 = Camera.ProjectToScreen(camX1, camY1, camZ1, screenX1, screenY1)
                    Dim visible2 = Camera.ProjectToScreen(camX2, camY2, camZ2, screenX2, screenY2)

                    ' EDITOR.BAS:1035-1037 - Draw line if both points visible
                    ' IF y1 > 1 AND y2 > 1 THEN
                    '     LINE ((screenx1 + .5) * 320, (screeny1 + .5) * 200)-...
                    If visible1 AndAlso visible2 Then
                        ' EDITOR.BAS:1036 - Convert normalized coords to screen pixels
                        Dim pixelX1 = CInt((screenX1 + 0.5F) * width)
                        Dim pixelY1 = CInt((screenY1 + 0.5F) * height)
                        Dim pixelX2 = CInt((screenX2 + 0.5F) * width)
                        Dim pixelY2 = CInt((screenY2 + 0.5F) * height)

                        ' Clip to reasonable screen bounds
                        If pixelX1 >= -width AndAlso pixelX1 <= width * 2 AndAlso
                           pixelY1 >= -height AndAlso pixelY1 <= height * 2 AndAlso
                           pixelX2 >= -width AndAlso pixelX2 <= width * 2 AndAlso
                           pixelY2 >= -height AndAlso pixelY2 <= height * 2 Then
                            ' EDITOR.BAS:1036 - LINE (...), object(currentobj).col
                            context.DrawLine(pen, New Point(pixelX1, pixelY1), New Point(pixelX2, pixelY2))
                        End If
                    End If
                Next
            Next
        End Sub
    End Class
End Namespace
