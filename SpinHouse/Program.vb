Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Input
Imports Avalonia.Layout
Imports Avalonia.Media
Imports Avalonia.Threading
Imports Avalonia.Themes.Fluent

Module Program
    Public Sub Main(args As String())
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args)
    End Sub

    Public Function BuildAvaloniaApp() As AppBuilder
        Return AppBuilder.Configure(Of SpinHouseApp)() _
            .UsePlatformDetect() _
            .WithInterFont() _
            .LogToTrace()
    End Function
End Module

Public Class SpinHouseApp
    Inherits Application

    Public Overrides Sub Initialize()
        Styles.Add(New FluentTheme())
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()
        If TypeOf ApplicationLifetime Is IClassicDesktopStyleApplicationLifetime Then
            Dim desktop = CType(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)
            desktop.MainWindow = New SpinHouse.MainWindow()
        End If

        MyBase.OnFrameworkInitializationCompleted()
    End Sub
End Class

Namespace SpinHouse
    ''' <summary>
    ''' Main window for the Spinning House application
    ''' Ported from SPINHOUS.BAS main loop (lines 180-236)
    ''' </summary>
    Public Class MainWindow
        Inherits Window

        Private HouseCanvas As HouseCanvas
        Private RenderTimer As DispatcherTimer

        Public Sub New()
            Title = "Spinning House - VB.Net Port of SPINHOUS.BAS"
            Width = 640
            Height = 480
            Background = Brushes.Black

            HouseCanvas = New HouseCanvas()

            Dim helpPanel As New StackPanel()
            helpPanel.Margin = New Thickness(10)
            helpPanel.VerticalAlignment = VerticalAlignment.Top
            helpPanel.HorizontalAlignment = HorizontalAlignment.Left

            Dim helpTexts() As String = {
                "Controls:",
                "Z - Rotate Left",
                "X - Rotate Right",
                "Space - Stop Rotation",
                "K - Move Away",
                "M - Move Closer",
                "Q/Escape - Quit"
            }

            For Each helpText In helpTexts
                Dim tb As New TextBlock()
                tb.Text = helpText
                tb.Foreground = Brushes.White
                tb.FontFamily = New FontFamily("Consolas")
                tb.FontSize = 12
                helpPanel.Children.Add(tb)
            Next

            Dim panel As New Panel()
            panel.Children.Add(HouseCanvas)
            panel.Children.Add(helpPanel)

            Content = panel

            InitializeRendering()
        End Sub

        Private Sub InitializeRendering()
            ' SPINHOUS.BAS:182 - FOR lop = 1 TO 1000: NEXT lop (delay loop)
            ' Replaced with Timer for smooth ~60 FPS animation
            RenderTimer = New DispatcherTimer()
            RenderTimer.Interval = TimeSpan.FromMilliseconds(16)
            AddHandler RenderTimer.Tick, AddressOf RenderTimer_Tick
            RenderTimer.Start()
        End Sub

        ''' <summary>
        ''' Animation tick - corresponds to SPINHOUS.BAS:180-236 main WHILE loop
        ''' </summary>
        Private Sub RenderTimer_Tick(sender As Object, e As EventArgs)
            HouseCanvas?.Update()
        End Sub

        ''' <summary>
        ''' Key handler - corresponds to SPINHOUS.BAS:181, 208-231
        ''' SPINHOUS.BAS used: button$ = INKEY$
        ''' </summary>
        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            MyBase.OnKeyDown(e)

            Select Case e.Key
                ' SPINHOUS.BAS:208 - IF button$ = "z" THEN dir = -1
                Case Key.Z
                    HouseCanvas?.SetRotationDirection(-1)
                ' SPINHOUS.BAS:209 - IF button$ = "x" THEN dir = 1
                Case Key.X
                    HouseCanvas?.SetRotationDirection(1)
                ' SPINHOUS.BAS:210 - IF button$ = " " THEN dir = 0
                Case Key.Space
                    HouseCanvas?.SetRotationDirection(0)
                ' SPINHOUS.BAS:221-225 - IF button$ = "k" THEN move away
                Case Key.K
                    HouseCanvas?.MoveHouse(0.1F)
                ' SPINHOUS.BAS:227-230 - IF button$ = "m" THEN move closer
                Case Key.M
                    HouseCanvas?.MoveHouse(-0.1F)
                ' SPINHOUS.BAS:180 - WHILE NOT (button$ = "q")
                Case Key.Q, Key.Escape
                    Me.Close()
            End Select
        End Sub

        Protected Overrides Sub OnClosed(e As EventArgs)
            MyBase.OnClosed(e)
            RenderTimer?.Stop()
        End Sub
    End Class

    ''' <summary>
    ''' Custom control for rendering the 3D house
    ''' </summary>
    Public Class HouseCanvas
        Inherits Control

        Private House As House3D

        Public Sub New()
            House = New House3D()
            ClipToBounds = True
        End Sub

        Public Sub Update()
            House.Update()
            InvalidateVisual()
        End Sub

        Public Sub SetRotationDirection(direction As Integer)
            House.RotationDirection = direction
        End Sub

        Public Sub MoveHouse(delta As Single)
            House.MoveY(delta)
        End Sub

        Public Overrides Sub Render(context As DrawingContext)
            MyBase.Render(context)

            ' SPINHOUS.BAS:183 - CLS (clear screen with black)
            context.FillRectangle(Brushes.Black, New Rect(0, 0, Bounds.Width, Bounds.Height))

            ' SPINHOUS.BAS:185-202 - Render the 3D house
            House.Render(context, Bounds.Width, Bounds.Height)
        End Sub
    End Class
End Namespace
