Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Input
Imports Avalonia.Interactivity
Imports Avalonia.Layout
Imports Avalonia.Media
Imports Avalonia.Threading
Imports Avalonia.Platform.Storage

Namespace Editor3D
    ''' <summary>
    ''' Main window for the 3D Editor application
    ''' Ported from EDITOR.BAS main program loop (lines 256-298)
    ''' </summary>
    Public Class MainWindow
        Inherits Window

        ' Core components
        Private World As World3D
        Private Camera As Camera3D
        Private Renderer As Renderer3D

        ' Rendering
        Private RenderTimer As DispatcherTimer
        Private RenderCanvas As EditorCanvas

        ' EDITOR.BAS:57 - col = 9 (default highlight colour)
        Private HighlightedObject As Integer = -1
        Private HighlightColor As Integer = 9 ' Blue

        ' UI components
        Private StatusLabel As TextBlock

        Public Sub New()
            InitializeComponent()
            InitializeWorld()
            InitializeRendering()
        End Sub

        Private Sub InitializeComponent()
            Title = "3D Editor - VB.Net Port of EDITOR.BAS"
            ' EDITOR.BAS:27 - SCREEN 7 (320x200), scaled up for modern displays
            Width = 800
            Height = 600
            Background = Brushes.Black

            ' Create main layout
            Dim mainGrid As New Grid()
            mainGrid.RowDefinitions.Add(New RowDefinition(GridLength.Auto))
            mainGrid.RowDefinitions.Add(New RowDefinition(GridLength.Star))
            mainGrid.RowDefinitions.Add(New RowDefinition(GridLength.Auto))

            ' Create menu bar
            Dim menuBar As New Menu()
            menuBar.Background = New SolidColorBrush(Color.FromRgb(40, 40, 40))

            ' File menu - EDITOR.BAS:312-518 save: subroutine
            Dim fileMenu As New MenuItem()
            fileMenu.Header = "_File"
            fileMenu.Foreground = Brushes.White

            Dim saveWorldItem As New MenuItem()
            saveWorldItem.Header = "_Save World..."
            AddHandler saveWorldItem.Click, AddressOf SaveWorld_Click
            fileMenu.Items.Add(saveWorldItem)

            Dim loadWorldItem As New MenuItem()
            loadWorldItem.Header = "_Load World..."
            AddHandler loadWorldItem.Click, AddressOf LoadWorld_Click
            fileMenu.Items.Add(loadWorldItem)

            fileMenu.Items.Add(New Separator())

            Dim saveObjectItem As New MenuItem()
            saveObjectItem.Header = "Save _Object..."
            AddHandler saveObjectItem.Click, AddressOf SaveObject_Click
            fileMenu.Items.Add(saveObjectItem)

            fileMenu.Items.Add(New Separator())

            Dim exitItem As New MenuItem()
            exitItem.Header = "E_xit"
            AddHandler exitItem.Click, Sub() Me.Close()
            fileMenu.Items.Add(exitItem)

            menuBar.Items.Add(fileMenu)

            ' Edit menu - EDITOR.BAS:526-742 manage: subroutine
            Dim editMenu As New MenuItem()
            editMenu.Header = "_Edit"
            editMenu.Foreground = Brushes.White

            Dim copyItem As New MenuItem()
            copyItem.Header = "_Copy Object"
            AddHandler copyItem.Click, AddressOf CopyObject_Click
            editMenu.Items.Add(copyItem)

            Dim deleteItem As New MenuItem()
            deleteItem.Header = "_Delete Object"
            AddHandler deleteItem.Click, AddressOf DeleteObject_Click
            editMenu.Items.Add(deleteItem)

            Dim moveItem As New MenuItem()
            moveItem.Header = "_Move Object..."
            AddHandler moveItem.Click, AddressOf MoveObject_Click
            editMenu.Items.Add(moveItem)

            menuBar.Items.Add(editMenu)

            ' View menu
            Dim viewMenu As New MenuItem()
            viewMenu.Header = "_View"
            viewMenu.Foreground = Brushes.White

            Dim listItem As New MenuItem()
            listItem.Header = "_List Objects"
            AddHandler listItem.Click, AddressOf ListObjects_Click
            viewMenu.Items.Add(listItem)

            Dim highlightItem As New MenuItem()
            highlightItem.Header = "_Highlight Mode"
            AddHandler highlightItem.Click, AddressOf HighlightMode_Click
            viewMenu.Items.Add(highlightItem)

            Dim resetItem As New MenuItem()
            resetItem.Header = "_Reset Camera"
            AddHandler resetItem.Click, AddressOf ResetCamera_Click
            viewMenu.Items.Add(resetItem)

            Dim speedItem As New MenuItem()
            speedItem.Header = "Change _Speed..."
            AddHandler speedItem.Click, AddressOf ChangeSpeed_Click
            viewMenu.Items.Add(speedItem)

            menuBar.Items.Add(viewMenu)

            ' Help menu - EDITOR.BAS:831-859 help: subroutine
            Dim helpMenu As New MenuItem()
            helpMenu.Header = "_Help"
            helpMenu.Foreground = Brushes.White

            Dim controlsItem As New MenuItem()
            controlsItem.Header = "_Controls"
            AddHandler controlsItem.Click, AddressOf ShowHelp_Click
            helpMenu.Items.Add(controlsItem)

            Dim aboutItem As New MenuItem()
            aboutItem.Header = "_About"
            AddHandler aboutItem.Click, AddressOf ShowAbout_Click
            helpMenu.Items.Add(aboutItem)

            menuBar.Items.Add(helpMenu)

            Grid.SetRow(menuBar, 0)
            mainGrid.Children.Add(menuBar)

            ' Create render canvas
            RenderCanvas = New EditorCanvas()
            Grid.SetRow(RenderCanvas, 1)
            mainGrid.Children.Add(RenderCanvas)

            ' Status label (replaces EDITOR.BAS:270-273 LOCATE/PRINT statements)
            StatusLabel = New TextBlock()
            StatusLabel.Background = New SolidColorBrush(Color.FromRgb(40, 40, 40))
            StatusLabel.Foreground = Brushes.White
            StatusLabel.Padding = New Thickness(5)
            StatusLabel.Text = "Ready - Press H for help"
            Grid.SetRow(StatusLabel, 2)
            mainGrid.Children.Add(StatusLabel)

            Content = mainGrid
        End Sub

        Private Sub InitializeWorld()
            World = New World3D()
            Camera = New Camera3D()
            Renderer = New Renderer3D(World, Camera)
            RenderCanvas.SetRenderer(Renderer, HighlightedObject, HighlightColor)
        End Sub

        Private Sub InitializeRendering()
            ' Replaces EDITOR.BAS:276-278 - WHILE button$ = "" / WEND polling loop
            RenderTimer = New DispatcherTimer()
            RenderTimer.Interval = TimeSpan.FromMilliseconds(16) ' ~60 FPS
            AddHandler RenderTimer.Tick, AddressOf RenderTimer_Tick
            RenderTimer.Start()
        End Sub

        ''' <summary>
        ''' Main loop tick - corresponds to EDITOR.BAS:259-290
        ''' </summary>
        Private Sub RenderTimer_Tick(sender As Object, e As EventArgs)
            UpdateStatus()
            RenderCanvas.SetRenderer(Renderer, HighlightedObject, HighlightColor)
            RenderCanvas.InvalidateVisual()
        End Sub

        Private Sub UpdateStatus()
            Dim objInfo = ""
            If HighlightedObject >= 0 AndAlso HighlightedObject < World3D.MaxObjects AndAlso World.Objects(HighlightedObject).IsUsed Then
                objInfo = $" | Selected: {World.Objects(HighlightedObject).Name} (#{HighlightedObject})"
            End If
            StatusLabel.Text = $"Pos: ({Camera.PosX:F1}, {Camera.PosY:F1}, {Camera.PosZ:F1}) | Speed: {Camera.Speed}{objInfo}"
        End Sub

        ''' <summary>
        ''' Key handler - corresponds to EDITOR.BAS:280-289 and movement: subroutine (917-958)
        ''' EDITOR.BAS used: button$ = INKEY$
        ''' </summary>
        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            MyBase.OnKeyDown(e)

            Select Case e.Key
                ' EDITOR.BAS:917-958 - movement: subroutine
                ' Movement (numpad) - EDITOR.BAS:919-939
                Case Key.NumPad8
                    Camera.MoveForward()   ' EDITOR.BAS:929-933
                Case Key.NumPad2
                    Camera.MoveBackward()  ' EDITOR.BAS:935-939
                Case Key.NumPad4
                    Camera.StrafeLeft()    ' EDITOR.BAS:921-924
                Case Key.NumPad6
                    Camera.StrafeRight()   ' EDITOR.BAS:925-928
                Case Key.NumPad9
                    Camera.MoveUp()        ' EDITOR.BAS:919
                Case Key.NumPad3
                    Camera.MoveDown()      ' EDITOR.BAS:920
                Case Key.NumPad5
                    Camera.Reset()         ' EDITOR.BAS:941-948

                ' Rotation - EDITOR.BAS:951-957
                Case Key.OemComma          ' < - EDITOR.BAS:951
                    Camera.TurnLeft()
                Case Key.OemPeriod         ' > - EDITOR.BAS:952
                    Camera.TurnRight()
                Case Key.Oem2              ' / - EDITOR.BAS:953
                    Camera.LookDown()
                Case Key.OemQuotes         ' ' - EDITOR.BAS:954
                    Camera.LookUp()
                Case Key.O                 ' EDITOR.BAS:957
                    Camera.RollLeft()
                Case Key.P                 ' EDITOR.BAS:956
                    Camera.RollRight()

                ' Editor functions - EDITOR.BAS:282-289
                Case Key.L                 ' EDITOR.BAS:286 - IF button$ = "l" THEN GOSUB highlight
                    HighlightMode_Click(Nothing, Nothing)
                Case Key.T                 ' EDITOR.BAS:282 - IF button$ = "t" THEN GOSUB listobjects
                    ListObjects_Click(Nothing, Nothing)
                Case Key.H                 ' EDITOR.BAS:284 - IF button$ = "h" THEN GOSUB help
                    ShowHelp_Click(Nothing, Nothing)
                Case Key.M                 ' EDITOR.BAS:285 - IF button$ = "m" THEN GOSUB moveobject
                    MoveObject_Click(Nothing, Nothing)
                Case Key.C                 ' EDITOR.BAS:287 - IF button$ = "c" THEN GOSUB speedchange
                    ChangeSpeed_Click(Nothing, Nothing)
                Case Key.D                 ' EDITOR.BAS:288 - IF button$ = "d" THEN GOSUB manage
                    DeleteObject_Click(Nothing, Nothing)
                Case Key.S                 ' EDITOR.BAS:289 - IF button$ = "s" THEN GOSUB save
                    SaveWorld_Click(Nothing, Nothing)

                ' EDITOR.BAS:260 - WHILE NOT (button$ = "Q")
                Case Key.Q, Key.Escape
                    Me.Close()
            End Select
        End Sub

        ''' <summary>
        ''' Save world - EDITOR.BAS:324-373
        ''' </summary>
        Private Async Sub SaveWorld_Click(sender As Object, e As RoutedEventArgs)
            Dim storage = StorageProvider
            Dim result = Await storage.SaveFilePickerAsync(New FilePickerSaveOptions() With {
                .Title = "Save World",
                .DefaultExtension = "wld",
                .FileTypeChoices = New List(Of FilePickerFileType) From {
                    New FilePickerFileType("World Files") With {.Patterns = New List(Of String) From {"*.wld"}}
                }
            })

            If result IsNot Nothing Then
                World.SaveWorld(result.Path.LocalPath)
                Await ShowMessageAsync("Save", "World saved successfully.")
            End If
        End Sub

        ''' <summary>
        ''' Load world - EDITOR.BAS:376-447
        ''' </summary>
        Private Async Sub LoadWorld_Click(sender As Object, e As RoutedEventArgs)
            Dim storage = StorageProvider
            Dim result = Await storage.OpenFilePickerAsync(New FilePickerOpenOptions() With {
                .Title = "Load World",
                .AllowMultiple = False,
                .FileTypeFilter = New List(Of FilePickerFileType) From {
                    New FilePickerFileType("World Files") With {.Patterns = New List(Of String) From {"*.wld"}}
                }
            })

            If result IsNot Nothing AndAlso result.Count > 0 Then
                If World.LoadWorld(result(0).Path.LocalPath) Then
                    HighlightedObject = -1
                    Await ShowMessageAsync("Load", "World loaded successfully.")
                Else
                    Await ShowMessageAsync("Error", "Failed to load world file.")
                End If
            End If
        End Sub

        ''' <summary>
        ''' Save object - EDITOR.BAS:450-496
        ''' </summary>
        Private Async Sub SaveObject_Click(sender As Object, e As RoutedEventArgs)
            If HighlightedObject < 0 Then
                Await ShowMessageAsync("Save Object", "Please select an object first (use Highlight mode).")
                Return
            End If

            Dim storage = StorageProvider
            Dim result = Await storage.SaveFilePickerAsync(New FilePickerSaveOptions() With {
                .Title = "Save Object",
                .DefaultExtension = "obj",
                .SuggestedFileName = World.Objects(HighlightedObject).Name.Trim(),
                .FileTypeChoices = New List(Of FilePickerFileType) From {
                    New FilePickerFileType("Object Files") With {.Patterns = New List(Of String) From {"*.obj"}}
                }
            })

            If result IsNot Nothing Then
                World.SaveObject(HighlightedObject, result.Path.LocalPath)
                Await ShowMessageAsync("Save", "Object saved successfully.")
            End If
        End Sub

        ''' <summary>
        ''' Copy object - EDITOR.BAS:689-711
        ''' </summary>
        Private Async Sub CopyObject_Click(sender As Object, e As RoutedEventArgs)
            If HighlightedObject < 0 Then
                Await ShowMessageAsync("Copy Object", "Please select an object first (use Highlight mode).")
                Return
            End If

            Dim newIndex = World.CopyObject(HighlightedObject)
            If newIndex >= 0 Then
                Await ShowMessageAsync("Copy", $"Object copied to slot {newIndex}.")
            Else
                Await ShowMessageAsync("Copy", "No free object slots available.")
            End If
        End Sub

        ''' <summary>
        ''' Delete object - EDITOR.BAS:562-683
        ''' </summary>
        Private Async Sub DeleteObject_Click(sender As Object, e As RoutedEventArgs)
            If HighlightedObject < 0 Then
                Await ShowMessageAsync("Delete Object", "Please select an object first (use Highlight mode).")
                Return
            End If

            Dim objName = World.Objects(HighlightedObject).Name
            ' Simple confirmation - in a real app you'd use a proper dialog
            World.DeleteObject(HighlightedObject)
            HighlightedObject = -1
            Await ShowMessageAsync("Delete", $"Object '{objName}' deleted.")
        End Sub

        ''' <summary>
        ''' Move object - EDITOR.BAS:801-827 moveobject: subroutine
        ''' </summary>
        Private Async Sub MoveObject_Click(sender As Object, e As RoutedEventArgs)
            If HighlightedObject < 0 Then
                Await ShowMessageAsync("Move Object", "Please select an object first (use Highlight mode).")
                Return
            End If

            ' EDITOR.BAS:803-808 - Display current position
            Dim obj = World.Objects(HighlightedObject)
            Dim input = Await ShowInputAsync("Move Object",
                $"Move '{obj.Name}'" & vbCrLf &
                $"Current: X={obj.PosX}, Y={obj.PosY}, Z={obj.PosZ}" & vbCrLf &
                "Enter delta X,Y,Z (e.g., 1,0,-2):",
                "0,0,0")

            If Not String.IsNullOrEmpty(input) Then
                Dim parseError As Boolean = False
                Try
                    Dim parts = input.Split(","c)
                    If parts.Length >= 3 Then
                        ' EDITOR.BAS:814-816
                        Dim dx = Single.Parse(parts(0).Trim())
                        Dim dy = Single.Parse(parts(1).Trim())
                        Dim dz = Single.Parse(parts(2).Trim())
                        World.MoveObject(HighlightedObject, dx, dy, dz)
                    Else
                        parseError = True
                    End If
                Catch
                    parseError = True
                End Try
                If parseError Then
                    Await ShowMessageAsync("Error", "Invalid input. Use format: X,Y,Z")
                End If
            End If
        End Sub

        ''' <summary>
        ''' List objects - EDITOR.BAS:868-898 listobjects: subroutine
        ''' </summary>
        Private Async Sub ListObjects_Click(sender As Object, e As RoutedEventArgs)
            Dim list As New System.Text.StringBuilder()
            list.AppendLine("Objects in world:")
            list.AppendLine()

            ' EDITOR.BAS:877-884 - Display object info
            For i = 0 To World3D.MaxObjects - 1
                Dim obj = World.Objects(i)
                If obj.IsUsed Then
                    list.AppendLine($"[{i}] {obj.Name} at ({obj.PosX:F1}, {obj.PosY:F1}, {obj.PosZ:F1}) - {obj.PointCount} points")
                End If
            Next

            list.AppendLine()
            list.AppendLine($"Total: {World.GetObjectCount()} objects, {World.GetPointCount()} points, {World.GetLinkCount()} links")

            Await ShowMessageAsync("Object List", list.ToString())
        End Sub

        ''' <summary>
        ''' Highlight mode - EDITOR.BAS:761-794 highlight: subroutine
        ''' Cycles through objects with <> keys in original
        ''' </summary>
        Private Sub HighlightMode_Click(sender As Object, e As RoutedEventArgs)
            ' EDITOR.BAS:781-782 - Cycle through objects
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

        Private Sub ResetCamera_Click(sender As Object, e As RoutedEventArgs)
            Camera.Reset()
        End Sub

        ''' <summary>
        ''' Change speed - EDITOR.BAS:748-756 speedchange: subroutine
        ''' </summary>
        Private Async Sub ChangeSpeed_Click(sender As Object, e As RoutedEventArgs)
            ' EDITOR.BAS:751-753
            Dim input = Await ShowInputAsync("Change Speed", $"Current speed: {Camera.Speed}" & vbCrLf & "Enter new speed:", Camera.Speed.ToString())
            If Not String.IsNullOrEmpty(input) Then
                Try
                    Camera.Speed = Single.Parse(input)
                Catch
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Show help - EDITOR.BAS:831-859 help: subroutine
        ''' </summary>
        Private Async Sub ShowHelp_Click(sender As Object, e As RoutedEventArgs)
            ' EDITOR.BAS:838-853 - Help text
            Dim helpText = "3D Editor Controls:" & vbCrLf & vbCrLf &
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

            Await ShowMessageAsync("Help", helpText)
        End Sub

        Private Async Sub ShowAbout_Click(sender As Object, e As RoutedEventArgs)
            Await ShowMessageAsync("About", "3D Editor" & vbCrLf &
                           "VB.Net Port of EDITOR.BAS" & vbCrLf & vbCrLf &
                           "Original QBasic code from the DOS era" & vbCrLf &
                           "Converted to .NET with Avalonia UI")
        End Sub

        Private Async Function ShowMessageAsync(title As String, message As String) As Task
            Dim dialog As New Window()
            dialog.Title = title
            dialog.Width = 400
            dialog.Height = 300
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner

            Dim panel As New DockPanel()
            panel.Margin = New Thickness(10)

            Dim textBox As New TextBox()
            textBox.Text = message
            textBox.IsReadOnly = True
            textBox.TextWrapping = TextWrapping.Wrap
            textBox.AcceptsReturn = True
            textBox.VerticalAlignment = VerticalAlignment.Stretch
            DockPanel.SetDock(textBox, Dock.Top)

            Dim okButton As New Button()
            okButton.Content = "OK"
            okButton.HorizontalAlignment = HorizontalAlignment.Center
            okButton.Margin = New Thickness(0, 10, 0, 0)
            okButton.Padding = New Thickness(20, 5, 20, 5)
            AddHandler okButton.Click, Sub() dialog.Close()
            DockPanel.SetDock(okButton, Dock.Bottom)

            panel.Children.Add(okButton)
            panel.Children.Add(textBox)
            dialog.Content = panel

            Await dialog.ShowDialog(Me)
        End Function

        Private Async Function ShowInputAsync(title As String, prompt As String, defaultValue As String) As Task(Of String)
            Dim result As String = Nothing
            Dim dialog As New Window()
            dialog.Title = title
            dialog.Width = 350
            dialog.Height = 200
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner

            Dim panel As New StackPanel()
            panel.Margin = New Thickness(10)

            Dim promptLabel As New TextBlock()
            promptLabel.Text = prompt
            promptLabel.TextWrapping = TextWrapping.Wrap
            promptLabel.Margin = New Thickness(0, 0, 0, 10)
            panel.Children.Add(promptLabel)

            Dim inputBox As New TextBox()
            inputBox.Text = defaultValue
            inputBox.Margin = New Thickness(0, 0, 0, 10)
            panel.Children.Add(inputBox)

            Dim buttonPanel As New StackPanel()
            buttonPanel.Orientation = Orientation.Horizontal
            buttonPanel.HorizontalAlignment = HorizontalAlignment.Center

            Dim okButton As New Button()
            okButton.Content = "OK"
            okButton.Padding = New Thickness(20, 5, 20, 5)
            okButton.Margin = New Thickness(0, 0, 10, 0)
            AddHandler okButton.Click, Sub()
                                           result = inputBox.Text
                                           dialog.Close()
                                       End Sub
            buttonPanel.Children.Add(okButton)

            Dim cancelButton As New Button()
            cancelButton.Content = "Cancel"
            cancelButton.Padding = New Thickness(20, 5, 20, 5)
            AddHandler cancelButton.Click, Sub() dialog.Close()
            buttonPanel.Children.Add(cancelButton)

            panel.Children.Add(buttonPanel)
            dialog.Content = panel

            Await dialog.ShowDialog(Me)
            Return result
        End Function

        Protected Overrides Sub OnClosed(e As EventArgs)
            MyBase.OnClosed(e)
            RenderTimer?.Stop()
        End Sub
    End Class

    ''' <summary>
    ''' Custom control for rendering the 3D scene
    ''' </summary>
    Public Class EditorCanvas
        Inherits Control

        Private Renderer As Renderer3D
        Private HighlightedObject As Integer = -1
        Private HighlightColor As Integer = 9

        Public Sub New()
            ClipToBounds = True
        End Sub

        Public Sub SetRenderer(renderer As Renderer3D, highlightedObject As Integer, highlightColor As Integer)
            Me.Renderer = renderer
            Me.HighlightedObject = highlightedObject
            Me.HighlightColor = highlightColor
        End Sub

        Public Overrides Sub Render(context As DrawingContext)
            MyBase.Render(context)

            ' EDITOR.BAS:261 - CLS
            context.FillRectangle(Brushes.Black, New Rect(0, 0, Bounds.Width, Bounds.Height))

            If Renderer IsNot Nothing Then
                ' Draw help overlay (not in original)
                DrawHelpOverlay(context)

                ' EDITOR.BAS:268 - GOSUB disp
                Renderer.Render(context, CInt(Bounds.Width), CInt(Bounds.Height), HighlightedObject, HighlightColor)
            End If
        End Sub

        Private Sub DrawHelpOverlay(context As DrawingContext)
            Dim typeface As New Typeface("Consolas")
            Dim brush As New SolidColorBrush(Color.FromArgb(180, 255, 255, 255))

            Dim y As Double = 10
            DrawText(context, "Movement: Numpad 8/2/4/6 | Up/Down: 9/3", typeface, brush, 10, y)
            y += 16
            DrawText(context, "Look: </> turn | '// pitch | O/P roll", typeface, brush, 10, y)
            y += 16
            DrawText(context, "5=Reset | L=Highlight | H=Help", typeface, brush, 10, y)
        End Sub

        Private Sub DrawText(context As DrawingContext, textContent As String, typeface As Typeface, brush As IBrush, x As Double, y As Double)
            Dim formattedText As New FormattedText(textContent, Globalization.CultureInfo.CurrentCulture,
                                                    FlowDirection.LeftToRight, typeface, 12, brush)
            context.DrawText(formattedText, New Point(x, y))
        End Sub
    End Class
End Namespace
