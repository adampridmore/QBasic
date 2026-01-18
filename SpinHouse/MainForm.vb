Imports System.Windows.Forms
Imports System.Drawing

Namespace SpinHouse
    ''' <summary>
    ''' Main form for the Spinning House application
    ''' Ported from SPINHOUS.BAS main loop (lines 180-236)
    ''' </summary>
    Public Class MainForm
        Inherits Form

        Private House As House3D
        Private RenderTimer As Timer
        Private BackBuffer As Bitmap
        Private BackGraphics As Graphics

        Public Sub New()
            InitializeComponent()
            InitializeRendering()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "Spinning House - VB.Net Port of SPINHOUS.BAS"
            ' SPINHOUS.BAS:4 - SCREEN 1 (320x200), scaled up for modern displays
            Me.ClientSize = New Size(640, 480)
            Me.DoubleBuffered = True
            Me.BackColor = Color.Black
            Me.KeyPreview = True
            Me.StartPosition = FormStartPosition.CenterScreen

            AddHandler Me.KeyDown, AddressOf MainForm_KeyDown
            AddHandler Me.KeyUp, AddressOf MainForm_KeyUp
            AddHandler Me.Paint, AddressOf MainForm_Paint
            AddHandler Me.Resize, AddressOf MainForm_Resize
            AddHandler Me.FormClosing, AddressOf MainForm_FormClosing
        End Sub

        Private Sub InitializeRendering()
            House = New House3D()
            CreateBackBuffer()

            ' SPINHOUS.BAS:182 - FOR lop = 1 TO 1000: NEXT lop (delay loop)
            ' Replaced with Timer for smooth ~60 FPS animation
            RenderTimer = New Timer()
            RenderTimer.Interval = 16
            AddHandler RenderTimer.Tick, AddressOf RenderTimer_Tick
            RenderTimer.Start()
        End Sub

        Private Sub CreateBackBuffer()
            If BackGraphics IsNot Nothing Then
                BackGraphics.Dispose()
            End If
            If BackBuffer IsNot Nothing Then
                BackBuffer.Dispose()
            End If

            If Me.ClientSize.Width > 0 AndAlso Me.ClientSize.Height > 0 Then
                BackBuffer = New Bitmap(Me.ClientSize.Width, Me.ClientSize.Height)
                BackGraphics = Graphics.FromImage(BackBuffer)
                BackGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            End If
        End Sub

        ''' <summary>
        ''' Animation tick - corresponds to SPINHOUS.BAS:180-236 main WHILE loop
        ''' </summary>
        Private Sub RenderTimer_Tick(sender As Object, e As EventArgs)
            House.Update()
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Paint handler - corresponds to SPINHOUS.BAS:183-202 rendering code
        ''' </summary>
        Private Sub MainForm_Paint(sender As Object, e As PaintEventArgs)
            If BackBuffer Is Nothing OrElse BackGraphics Is Nothing Then Return

            ' SPINHOUS.BAS:183 - CLS
            BackGraphics.Clear(Color.Black)

            ' Help text (not in original - added for usability)
            Using font As New Font("Consolas", 10)
                Using brush As New SolidBrush(Color.White)
                    BackGraphics.DrawString("Controls:", font, brush, 10, 10)
                    BackGraphics.DrawString("Z - Rotate Left", font, brush, 10, 30)
                    BackGraphics.DrawString("X - Rotate Right", font, brush, 10, 50)
                    BackGraphics.DrawString("Space - Stop Rotation", font, brush, 10, 70)
                    BackGraphics.DrawString("K - Move Away", font, brush, 10, 90)
                    BackGraphics.DrawString("M - Move Closer", font, brush, 10, 110)
                    BackGraphics.DrawString("Q/Escape - Quit", font, brush, 10, 130)
                End Using
            End Using

            ' SPINHOUS.BAS:185-202 - Render the 3D house
            House.Render(BackGraphics, Me.ClientSize.Width, Me.ClientSize.Height)

            e.Graphics.DrawImage(BackBuffer, 0, 0)
        End Sub

        ''' <summary>
        ''' Key handler - corresponds to SPINHOUS.BAS:181, 208-231
        ''' SPINHOUS.BAS used: button$ = INKEY$
        ''' </summary>
        Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs)
            Select Case e.KeyCode
                ' SPINHOUS.BAS:208 - IF button$ = "z" THEN dir = -1
                Case Keys.Z
                    House.RotationDirection = -1
                ' SPINHOUS.BAS:209 - IF button$ = "x" THEN dir = 1
                Case Keys.X
                    House.RotationDirection = 1
                ' SPINHOUS.BAS:210 - IF button$ = " " THEN dir = 0
                Case Keys.Space
                    House.RotationDirection = 0
                ' SPINHOUS.BAS:221-225 - IF button$ = "k" THEN move away
                Case Keys.K
                    House.MoveY(0.1F)
                ' SPINHOUS.BAS:227-230 - IF button$ = "m" THEN move closer
                Case Keys.M
                    House.MoveY(-0.1F)
                ' SPINHOUS.BAS:180 - WHILE NOT (button$ = "q")
                Case Keys.Q, Keys.Escape
                    Me.Close()
            End Select
        End Sub

        Private Sub MainForm_KeyUp(sender As Object, e As KeyEventArgs)
            ' Original QBasic kept rotating until Space was pressed
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
