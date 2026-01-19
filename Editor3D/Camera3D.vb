Namespace Editor3D
    ''' <summary>
    ''' 3D Camera with position and rotation
    ''' Handles movement and view transformations
    ''' Based on camera variables in EDITOR.BAS:47-58
    ''' </summary>
    Public Class Camera3D
        ' EDITOR.BAS:25 - CONST pi = 3.141592654#
        Private Const Pi As Single = 3.141592654F

        ' EDITOR.BAS:47-50 - Camera position
        Public PosX As Single = 0      ' cameraposx = 0
        Public PosY As Single = -40    ' cameraposy = -40
        Public PosZ As Single = -10    ' cameraposz = -10

        ' EDITOR.BAS:51-53 - Camera rotation (in radians)
        Public RotX As Single = 0      ' camerarotx = 0 (pitch)
        Public RotY As Single = 0      ' cameraroty = 0 (roll)
        Public RotZ As Single = 0      ' camerarotz = 0 (yaw)

        ' EDITOR.BAS:58 - speed = 4
        Public Speed As Single = 4

        ' EDITOR.BAS:951-957 - Rotation uses pi / 12 increments
        Private Const RotationStep As Single = Pi / 12

        ''' <summary>
        ''' Reset camera to default position
        ''' EDITOR.BAS:941-948 - IF button$ = "5" THEN
        ''' </summary>
        Public Sub Reset()
            PosX = 0
            PosY = -40
            PosZ = -10
            RotX = 0
            RotY = 0
            RotZ = 0
        End Sub

        ''' <summary>
        ''' Move forward relative to camera facing
        ''' EDITOR.BAS:929-933 - IF button$ = "8" THEN
        ''' </summary>
        Public Sub MoveForward()
            ' cameraposx = cameraposx + speed * SIN(-camerarotz)
            PosX += Speed * CSng(Math.Sin(-RotZ))
            ' cameraposy = cameraposy + speed * COS(camerarotz)
            PosY += Speed * CSng(Math.Cos(RotZ))
            ' cameraposz = cameraposz + speed * SIN(camerarotx)
            PosZ += Speed * CSng(Math.Sin(RotX))
        End Sub

        ''' <summary>
        ''' Move backward relative to camera facing
        ''' EDITOR.BAS:935-939 - IF button$ = "2" THEN
        ''' </summary>
        Public Sub MoveBackward()
            PosX -= Speed * CSng(Math.Sin(-RotZ))
            PosY -= Speed * CSng(Math.Cos(RotZ))
            PosZ -= Speed * CSng(Math.Sin(RotX))
        End Sub

        ''' <summary>
        ''' Strafe left
        ''' EDITOR.BAS:921-924 - IF button$ = "4" THEN
        ''' </summary>
        Public Sub StrafeLeft()
            ' cameraposx = cameraposx - speed * SIN(pi / 2 - camerarotz)
            PosX -= Speed * CSng(Math.Sin(Pi / 2 - RotZ))
            ' cameraposy = cameraposy - speed * COS(pi / 2 - camerarotz)
            PosY -= Speed * CSng(Math.Cos(Pi / 2 - RotZ))
        End Sub

        ''' <summary>
        ''' Strafe right
        ''' EDITOR.BAS:925-928 - IF button$ = "6" THEN
        ''' </summary>
        Public Sub StrafeRight()
            PosX += Speed * CSng(Math.Sin(Pi / 2 - RotZ))
            PosY += Speed * CSng(Math.Cos(Pi / 2 - RotZ))
        End Sub

        ''' <summary>
        ''' Move up (absolute)
        ''' EDITOR.BAS:919 - IF button$ = "9" THEN cameraposz = cameraposz - speed
        ''' </summary>
        Public Sub MoveUp()
            PosZ -= Speed
        End Sub

        ''' <summary>
        ''' Move down (absolute)
        ''' EDITOR.BAS:920 - IF button$ = "3" THEN cameraposz = cameraposz + speed
        ''' </summary>
        Public Sub MoveDown()
            PosZ += Speed
        End Sub

        ''' <summary>
        ''' Turn left (yaw)
        ''' EDITOR.BAS:951 - IF button$ = "," THEN camerarotz = camerarotz + pi / 12
        ''' </summary>
        Public Sub TurnLeft()
            RotZ += RotationStep
        End Sub

        ''' <summary>
        ''' Turn right (yaw)
        ''' EDITOR.BAS:952 - IF button$ = "." THEN camerarotz = camerarotz - pi / 12
        ''' </summary>
        Public Sub TurnRight()
            RotZ -= RotationStep
        End Sub

        ''' <summary>
        ''' Look up (pitch)
        ''' EDITOR.BAS:954 - IF button$ = "'" THEN camerarotx = camerarotx - pi / 12
        ''' </summary>
        Public Sub LookUp()
            RotX -= RotationStep
        End Sub

        ''' <summary>
        ''' Look down (pitch)
        ''' EDITOR.BAS:953 - IF button$ = "/" THEN camerarotx = camerarotx + pi / 12
        ''' </summary>
        Public Sub LookDown()
            RotX += RotationStep
        End Sub

        ''' <summary>
        ''' Roll left
        ''' EDITOR.BAS:957 - IF button$ = "o" THEN cameraroty = cameraroty - pi / 12
        ''' </summary>
        Public Sub RollLeft()
            RotY -= RotationStep
        End Sub

        ''' <summary>
        ''' Roll right
        ''' EDITOR.BAS:956 - IF button$ = "p" THEN cameraroty = cameraroty + pi / 12
        ''' </summary>
        Public Sub RollRight()
            RotY += RotationStep
        End Sub

        ''' <summary>
        ''' Transform a world point to camera-relative coordinates
        ''' EDITOR.BAS:974-996 - Point transformation in disp: subroutine
        ''' </summary>
        Public Sub TransformPoint(worldX As Single, worldY As Single, worldZ As Single,
                                  ByRef camX As Single, ByRef camY As Single, ByRef camZ As Single)
            ' EDITOR.BAS:975-977 - Translate object to virtual object
            Dim x1 = worldX - PosX
            Dim y1 = worldY - PosY
            Dim z1 = worldZ - PosZ

            ' EDITOR.BAS:979-983 - Rotate about Z axis (yaw)
            Dim cosZ = CSng(Math.Cos(RotZ))
            Dim sinZ = CSng(Math.Sin(RotZ))
            Dim tempX = x1 * cosZ + y1 * sinZ
            Dim tempY = -x1 * sinZ + y1 * cosZ
            x1 = tempX
            y1 = tempY

            ' EDITOR.BAS:985-990 - Rotate about X axis (pitch)
            Dim cosX = CSng(Math.Cos(RotX))
            Dim sinX = CSng(Math.Sin(RotX))
            tempY = y1 * cosX + z1 * sinX
            Dim tempZ = -y1 * sinX + z1 * cosX
            y1 = tempY
            z1 = tempZ

            ' EDITOR.BAS:992-996 - Rotate about Y axis (roll)
            Dim cosY = CSng(Math.Cos(RotY))
            Dim sinY = CSng(Math.Sin(RotY))
            tempX = x1 * cosY + z1 * sinY
            tempZ = -x1 * sinY + z1 * cosY
            x1 = tempX
            z1 = tempZ

            camX = x1
            camY = y1
            camZ = z1
        End Sub

        ''' <summary>
        ''' Project camera-space coordinates to screen coordinates
        ''' EDITOR.BAS:1023-1034 - Perspective projection
        ''' screenx1 = x1 / y1, screeny1 = z1 / y1
        ''' </summary>
        Public Function ProjectToScreen(camX As Single, camY As Single, camZ As Single,
                                        ByRef screenX As Single, ByRef screenY As Single) As Boolean
            ' EDITOR.BAS:1035 - IF y1 > 1 AND y2 > 1 THEN
            If camY <= 1 Then
                Return False
            End If

            ' EDITOR.BAS:1024-1027
            screenX = camX / camY
            screenY = camZ / camY
            Return True
        End Function

        ''' <summary>
        ''' Calculate horizon line height based on camera pitch
        ''' EDITOR.BAS:263 - horizonheight = -200 * SIN(camerarotx) + 100
        ''' </summary>
        Public Function GetHorizonHeight(screenHeight As Integer) As Integer
            Return CInt(-200 * Math.Sin(RotX) + screenHeight / 2)
        End Function

        ''' <summary>
        ''' Calculate horizon twist based on camera roll
        ''' EDITOR.BAS:264 - twist = 100 * TAN(cameraroty)
        ''' </summary>
        Public Function GetHorizonTwist() As Integer
            Return CInt(100 * Math.Tan(RotY))
        End Function
    End Class
End Namespace
