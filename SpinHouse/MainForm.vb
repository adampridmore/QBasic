Imports System.Windows.Forms
Imports System.Drawing

Namespace SpinHouse
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
            Me.ClientSize = New Size(640, 480)
            Me.DoubleBuffered = True
            Me.BackColor = Color.Black
            Me.KeyPreview = True
            Me.StartPosition = FormStartPosition.CenterScreen

            ' Set up form events
            AddHandler Me.KeyDown, AddressOf MainForm_KeyDown
            AddHandler Me.KeyUp, AddressOf MainForm_KeyUp
            AddHandler Me.Paint, AddressOf MainForm_Paint
            AddHandler Me.Resize, AddressOf MainForm_Resize
            AddHandler Me.FormClosing, AddressOf MainForm_FormClosing
        End Sub

        Private Sub InitializeRendering()
            House = New House3D()

            ' Create back buffer for smooth rendering
            CreateBackBuffer()

            ' Set up animation timer (~60 FPS)
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

        Private Sub RenderTimer_Tick(sender As Object, e As EventArgs)
            ' Update house rotation
            House.Update()

            ' Request repaint
            Me.Invalidate()
        End Sub

        Private Sub MainForm_Paint(sender As Object, e As PaintEventArgs)
            If BackBuffer Is Nothing OrElse BackGraphics Is Nothing Then Return

            ' Clear back buffer
            BackGraphics.Clear(Color.Black)

            ' Draw controls help text
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

            ' Render the 3D house
            House.Render(BackGraphics, Me.ClientSize.Width, Me.ClientSize.Height)

            ' Copy back buffer to screen
            e.Graphics.DrawImage(BackBuffer, 0, 0)
        End Sub

        Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs)
            Select Case e.KeyCode
                Case Keys.Z
                    House.RotationDirection = -1
                Case Keys.X
                    House.RotationDirection = 1
                Case Keys.Space
                    House.RotationDirection = 0
                Case Keys.K
                    House.MoveY(0.1F)
                Case Keys.M
                    House.MoveY(-0.1F)
                Case Keys.Q, Keys.Escape
                    Me.Close()
            End Select
        End Sub

        Private Sub MainForm_KeyUp(sender As Object, e As KeyEventArgs)
            ' Optional: stop rotation on key release (uncomment if desired)
            ' If e.KeyCode = Keys.Z OrElse e.KeyCode = Keys.X Then
            '     House.RotationDirection = 0
            ' End If
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
