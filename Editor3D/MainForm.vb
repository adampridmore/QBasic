Imports System.Windows.Forms
Imports System.Drawing

Namespace Editor3D
    Public Class MainForm
        Inherits Form

        ' Core components
        Private World As World3D
        Private Camera As Camera3D
        Private Renderer As Renderer3D

        ' Rendering
        Private RenderTimer As Timer
        Private BackBuffer As Bitmap
        Private BackGraphics As Graphics

        ' Editor state
        Private HighlightedObject As Integer = -1
        Private HighlightColor As Integer = 9 ' Blue

        ' UI components
        Private StatusLabel As Label
        Private MenuStrip As MenuStrip

        Public Sub New()
            InitializeComponent()
            InitializeWorld()
            InitializeRendering()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "3D Editor - VB.Net Port of EDITOR.BAS"
            Me.ClientSize = New Size(800, 600)
            Me.DoubleBuffered = True
            Me.BackColor = Color.Black
            Me.KeyPreview = True
            Me.StartPosition = FormStartPosition.CenterScreen

            ' Create menu strip
            MenuStrip = New MenuStrip()
            MenuStrip.BackColor = Color.FromArgb(40, 40, 40)
            MenuStrip.ForeColor = Color.White

            ' File menu
            Dim fileMenu As New ToolStripMenuItem("&File")
            fileMenu.DropDownItems.Add("&Save World...", Nothing, AddressOf SaveWorld_Click)
            fileMenu.DropDownItems.Add("&Load World...", Nothing, AddressOf LoadWorld_Click)
            fileMenu.DropDownItems.Add(New ToolStripSeparator())
            fileMenu.DropDownItems.Add("Save &Object...", Nothing, AddressOf SaveObject_Click)
            fileMenu.DropDownItems.Add(New ToolStripSeparator())
            fileMenu.DropDownItems.Add("E&xit", Nothing, Sub() Me.Close())
            MenuStrip.Items.Add(fileMenu)

            ' Edit menu
            Dim editMenu As New ToolStripMenuItem("&Edit")
            editMenu.DropDownItems.Add("&Copy Object", Nothing, AddressOf CopyObject_Click)
            editMenu.DropDownItems.Add("&Delete Object", Nothing, AddressOf DeleteObject_Click)
            editMenu.DropDownItems.Add("&Move Object...", Nothing, AddressOf MoveObject_Click)
            MenuStrip.Items.Add(editMenu)

            ' View menu
            Dim viewMenu As New ToolStripMenuItem("&View")
            viewMenu.DropDownItems.Add("&List Objects", Nothing, AddressOf ListObjects_Click)
            viewMenu.DropDownItems.Add("&Highlight Mode", Nothing, AddressOf HighlightMode_Click)
            viewMenu.DropDownItems.Add("&Reset Camera", Nothing, AddressOf ResetCamera_Click)
            viewMenu.DropDownItems.Add("Change &Speed...", Nothing, AddressOf ChangeSpeed_Click)
            MenuStrip.Items.Add(viewMenu)

            ' Help menu
            Dim helpMenu As New ToolStripMenuItem("&Help")
            helpMenu.DropDownItems.Add("&Controls", Nothing, AddressOf ShowHelp_Click)
            helpMenu.DropDownItems.Add("&About", Nothing, AddressOf ShowAbout_Click)
            MenuStrip.Items.Add(helpMenu)

            Me.MainMenuStrip = MenuStrip
            Me.Controls.Add(MenuStrip)

            ' Status label
            StatusLabel = New Label()
            StatusLabel.Dock = DockStyle.Bottom
            StatusLabel.Height = 24
            StatusLabel.BackColor = Color.FromArgb(40, 40, 40)
            StatusLabel.ForeColor = Color.White
            StatusLabel.TextAlign = ContentAlignment.MiddleLeft
            StatusLabel.Text = "Ready - Press H for help"
            Me.Controls.Add(StatusLabel)

            ' Events
            AddHandler Me.KeyDown, AddressOf MainForm_KeyDown
            AddHandler Me.Paint, AddressOf MainForm_Paint
            AddHandler Me.Resize, AddressOf MainForm_Resize
            AddHandler Me.FormClosing, AddressOf MainForm_FormClosing
        End Sub

        Private Sub InitializeWorld()
            World = New World3D()
            Camera = New Camera3D()
            Renderer = New Renderer3D(World, Camera)
        End Sub

        Private Sub InitializeRendering()
            CreateBackBuffer()

            RenderTimer = New Timer()
            RenderTimer.Interval = 16 ' ~60 FPS
            AddHandler RenderTimer.Tick, AddressOf RenderTimer_Tick
            RenderTimer.Start()
        End Sub

        Private Sub CreateBackBuffer()
            BackGraphics?.Dispose()
            BackBuffer?.Dispose()

            Dim clientArea = GetClientRenderArea()
            If clientArea.Width > 0 AndAlso clientArea.Height > 0 Then
                BackBuffer = New Bitmap(clientArea.Width, clientArea.Height)
                BackGraphics = Graphics.FromImage(BackBuffer)
                BackGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            End If
        End Sub

        Private Function GetClientRenderArea() As Rectangle
            Dim top = MenuStrip.Height
            Dim bottom = StatusLabel.Height
            Return New Rectangle(0, top, Me.ClientSize.Width, Me.ClientSize.Height - top - bottom)
        End Function

        Private Sub RenderTimer_Tick(sender As Object, e As EventArgs)
            UpdateStatus()
            Me.Invalidate()
        End Sub

        Private Sub UpdateStatus()
            Dim objInfo = ""
            If HighlightedObject >= 0 AndAlso HighlightedObject < World3D.MaxObjects AndAlso World.Objects(HighlightedObject).IsUsed Then
                objInfo = $" | Selected: {World.Objects(HighlightedObject).Name} (#{HighlightedObject})"
            End If
            StatusLabel.Text = $"Pos: ({Camera.PosX:F1}, {Camera.PosY:F1}, {Camera.PosZ:F1}) | Speed: {Camera.Speed}{objInfo}"
        End Sub

        Private Sub MainForm_Paint(sender As Object, e As PaintEventArgs)
            If BackBuffer Is Nothing OrElse BackGraphics Is Nothing Then Return

            Dim renderArea = GetClientRenderArea()

            ' Clear
            BackGraphics.Clear(Color.Black)

            ' Render 3D scene
            Renderer.Render(BackGraphics, renderArea.Width, renderArea.Height, HighlightedObject, HighlightColor)

            ' Draw help overlay
            DrawHelpOverlay(BackGraphics)

            ' Copy to screen
            e.Graphics.DrawImage(BackBuffer, renderArea.X, renderArea.Y)
        End Sub

        Private Sub DrawHelpOverlay(g As Graphics)
            Using font As New Font("Consolas", 9)
                Using brush As New SolidBrush(Color.FromArgb(180, Color.White))
                    Dim y = 10
                    g.DrawString("Movement: Numpad 8/2/4/6 | Up/Down: 9/3", font, brush, 10, y)
                    y += 16
                    g.DrawString("Look: </> turn | '// pitch | O/P roll", font, brush, 10, y)
                    y += 16
                    g.DrawString("5=Reset | L=Highlight | H=Help", font, brush, 10, y)
                End Using
            End Using
        End Sub

        Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs)
            Select Case e.KeyCode
                ' Movement (numpad)
                Case Keys.NumPad8
                    Camera.MoveForward()
                Case Keys.NumPad2
                    Camera.MoveBackward()
                Case Keys.NumPad4
                    Camera.StrafeLeft()
                Case Keys.NumPad6
                    Camera.StrafeRight()
                Case Keys.NumPad9
                    Camera.MoveUp()
                Case Keys.NumPad3
                    Camera.MoveDown()
                Case Keys.NumPad5
                    Camera.Reset()

                ' Rotation
                Case Keys.Oemcomma  ' <
                    Camera.TurnLeft()
                Case Keys.OemPeriod ' >
                    Camera.TurnRight()
                Case Keys.Oem2      ' /
                    Camera.LookDown()
                Case Keys.Oem7      ' '
                    Camera.LookUp()
                Case Keys.O
                    Camera.RollLeft()
                Case Keys.P
                    Camera.RollRight()

                ' Editor functions
                Case Keys.L
                    HighlightMode_Click(Nothing, Nothing)
                Case Keys.T
                    ListObjects_Click(Nothing, Nothing)
                Case Keys.H
                    ShowHelp_Click(Nothing, Nothing)
                Case Keys.M
                    MoveObject_Click(Nothing, Nothing)
                Case Keys.C
                    ChangeSpeed_Click(Nothing, Nothing)
                Case Keys.D
                    DeleteObject_Click(Nothing, Nothing)
                Case Keys.S
                    SaveWorld_Click(Nothing, Nothing)

                Case Keys.Q, Keys.Escape
                    Me.Close()
            End Select
        End Sub

        ' Menu handlers
        Private Sub SaveWorld_Click(sender As Object, e As EventArgs)
            Using dlg As New SaveFileDialog()
                dlg.Filter = "World Files (*.wld)|*.wld"
                dlg.DefaultExt = "wld"
                If dlg.ShowDialog() = DialogResult.OK Then
                    World.SaveWorld(dlg.FileName)
                    MessageBox.Show("World saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub LoadWorld_Click(sender As Object, e As EventArgs)
            Using dlg As New OpenFileDialog()
                dlg.Filter = "World Files (*.wld)|*.wld"
                If dlg.ShowDialog() = DialogResult.OK Then
                    If World.LoadWorld(dlg.FileName) Then
                        HighlightedObject = -1
                        MessageBox.Show("World loaded successfully.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("Failed to load world file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End Using
        End Sub

        Private Sub SaveObject_Click(sender As Object, e As EventArgs)
            If HighlightedObject < 0 Then
                MessageBox.Show("Please select an object first (use Highlight mode).", "Save Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using dlg As New SaveFileDialog()
                dlg.Filter = "Object Files (*.obj)|*.obj"
                dlg.DefaultExt = "obj"
                dlg.FileName = World.Objects(HighlightedObject).Name.Trim()
                If dlg.ShowDialog() = DialogResult.OK Then
                    World.SaveObject(HighlightedObject, dlg.FileName)
                    MessageBox.Show("Object saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub CopyObject_Click(sender As Object, e As EventArgs)
            If HighlightedObject < 0 Then
                MessageBox.Show("Please select an object first (use Highlight mode).", "Copy Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim newIndex = World.CopyObject(HighlightedObject)
            If newIndex >= 0 Then
                MessageBox.Show($"Object copied to slot {newIndex}.", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("No free object slots available.", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End Sub

        Private Sub DeleteObject_Click(sender As Object, e As EventArgs)
            If HighlightedObject < 0 Then
                MessageBox.Show("Please select an object first (use Highlight mode).", "Delete Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim objName = World.Objects(HighlightedObject).Name
            If MessageBox.Show($"Delete object '{objName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                World.DeleteObject(HighlightedObject)
                HighlightedObject = -1
            End If
        End Sub

        Private Sub MoveObject_Click(sender As Object, e As EventArgs)
            If HighlightedObject < 0 Then
                MessageBox.Show("Please select an object first (use Highlight mode).", "Move Object", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim obj = World.Objects(HighlightedObject)
            Dim input = InputBox($"Move '{obj.Name}'" & vbCrLf &
                                 $"Current: X={obj.PosX}, Y={obj.PosY}, Z={obj.PosZ}" & vbCrLf &
                                 "Enter delta X,Y,Z (e.g., 1,0,-2):", "Move Object", "0,0,0")
            If Not String.IsNullOrEmpty(input) Then
                Try
                    Dim parts = input.Split(","c)
                    If parts.Length >= 3 Then
                        Dim dx = Single.Parse(parts(0).Trim())
                        Dim dy = Single.Parse(parts(1).Trim())
                        Dim dz = Single.Parse(parts(2).Trim())
                        World.MoveObject(HighlightedObject, dx, dy, dz)
                    End If
                Catch
                    MessageBox.Show("Invalid input. Use format: X,Y,Z", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub ListObjects_Click(sender As Object, e As EventArgs)
            Dim list As New System.Text.StringBuilder()
            list.AppendLine("Objects in world:")
            list.AppendLine()

            For i = 0 To World3D.MaxObjects - 1
                Dim obj = World.Objects(i)
                If obj.IsUsed Then
                    list.AppendLine($"[{i}] {obj.Name} at ({obj.PosX:F1}, {obj.PosY:F1}, {obj.PosZ:F1}) - {obj.PointCount} points")
                End If
            Next

            list.AppendLine()
            list.AppendLine($"Total: {World.GetObjectCount()} objects, {World.GetPointCount()} points, {World.GetLinkCount()} links")

            MessageBox.Show(list.ToString(), "Object List", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub HighlightMode_Click(sender As Object, e As EventArgs)
            ' Cycle through objects
            Dim startIndex = If(HighlightedObject < 0, 0, HighlightedObject + 1)

            For i = startIndex To World3D.MaxObjects - 1
                If World.Objects(i).IsUsed Then
                    HighlightedObject = i
                    Return
                End If
            Next

            ' Wrap around
            For i = 0 To startIndex - 1
                If World.Objects(i).IsUsed Then
                    HighlightedObject = i
                    Return
                End If
            Next

            HighlightedObject = -1
        End Sub

        Private Sub ResetCamera_Click(sender As Object, e As EventArgs)
            Camera.Reset()
        End Sub

        Private Sub ChangeSpeed_Click(sender As Object, e As EventArgs)
            Dim input = InputBox($"Current speed: {Camera.Speed}" & vbCrLf & "Enter new speed:", "Change Speed", Camera.Speed.ToString())
            If Not String.IsNullOrEmpty(input) Then
                Try
                    Camera.Speed = Single.Parse(input)
                Catch
                End Try
            End If
        End Sub

        Private Sub ShowHelp_Click(sender As Object, e As EventArgs)
            Dim help = "3D Editor Controls:" & vbCrLf & vbCrLf &
                       "MOVEMENT (Numpad):" & vbCrLf &
                       "  8 - Move forward" & vbCrLf &
                       "  2 - Move backward" & vbCrLf &
                       "  4 - Strafe left" & vbCrLf &
                       "  6 - Strafe right" & vbCrLf &
                       "  9 - Move up" & vbCrLf &
                       "  3 - Move down" & vbCrLf &
                       "  5 - Reset camera" & vbCrLf & vbCrLf &
                       "ROTATION:" & vbCrLf &
                       "  < (comma) - Turn left" & vbCrLf &
                       "  > (period) - Turn right" & vbCrLf &
                       "  ' - Look up" & vbCrLf &
                       "  / - Look down" & vbCrLf &
                       "  O - Roll left" & vbCrLf &
                       "  P - Roll right" & vbCrLf & vbCrLf &
                       "EDITOR:" & vbCrLf &
                       "  L - Cycle highlight" & vbCrLf &
                       "  T - List objects" & vbCrLf &
                       "  M - Move object" & vbCrLf &
                       "  D - Delete object" & vbCrLf &
                       "  C - Change speed" & vbCrLf &
                       "  S - Save world" & vbCrLf &
                       "  Q/Esc - Quit"

            MessageBox.Show(help, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub ShowAbout_Click(sender As Object, e As EventArgs)
            MessageBox.Show("3D Editor" & vbCrLf &
                           "VB.Net Port of EDITOR.BAS" & vbCrLf & vbCrLf &
                           "Original QBasic code from the DOS era" & vbCrLf &
                           "Converted to .NET Windows Forms",
                           "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub MainForm_Resize(sender As Object, e As EventArgs)
            CreateBackBuffer()
        End Sub

        Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs)
            RenderTimer?.Stop()
            RenderTimer?.Dispose()
            BackGraphics?.Dispose()
            BackBuffer?.Dispose()
        End Sub
    End Class
End Namespace
