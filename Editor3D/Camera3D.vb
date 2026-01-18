Imports System.Drawing

Namespace Editor3D
    ''' <summary>
    ''' 3D Camera with position and rotation
    ''' Handles movement and view transformations
    ''' </summary>
    Public Class Camera3D
        Private Const Pi As Single = 3.141592654F

        ' Camera position
        Public PosX As Single = 0
        Public PosY As Single = -40
        Public PosZ As Single = -10

        ' Camera rotation (in radians)
        Public RotX As Single = 0  ' Pitch (look up/down)
        Public RotY As Single = 0  ' Roll
        Public RotZ As Single = 0  ' Yaw (turn left/right)

        ' Movement speed
        Public Speed As Single = 4

        ' Rotation increment
        Private Const RotationStep As Single = Pi / 12

        ''' <summary>
        ''' Reset camera to default position
        ''' </summary>
        Public Sub Reset()
            PosX = 0
            PosY = -40
            PosZ = -10
            RotX = 0
            RotY = 0
            RotZ = 0
        End Sub

        ' Movement methods
        Public Sub MoveForward()
            PosX += Speed * CSng(Math.Sin(-RotZ))
            PosY += Speed * CSng(Math.Cos(RotZ))
            PosZ += Speed * CSng(Math.Sin(RotX))
        End Sub

        Public Sub MoveBackward()
            PosX -= Speed * CSng(Math.Sin(-RotZ))
            PosY -= Speed * CSng(Math.Cos(RotZ))
            PosZ -= Speed * CSng(Math.Sin(RotX))
        End Sub

        Public Sub StrafeLeft()
            PosX -= Speed * CSng(Math.Sin(Pi / 2 - RotZ))
            PosY -= Speed * CSng(Math.Cos(Pi / 2 - RotZ))
        End Sub

        Public Sub StrafeRight()
            PosX += Speed * CSng(Math.Sin(Pi / 2 - RotZ))
            PosY += Speed * CSng(Math.Cos(Pi / 2 - RotZ))
        End Sub

        Public Sub MoveUp()
            PosZ -= Speed
        End Sub

        Public Sub MoveDown()
            PosZ += Speed
        End Sub

        ' Rotation methods
        Public Sub TurnLeft()
            RotZ += RotationStep
        End Sub

        Public Sub TurnRight()
            RotZ -= RotationStep
        End Sub

        Public Sub LookUp()
            RotX -= RotationStep
        End Sub

        Public Sub LookDown()
            RotX += RotationStep
        End Sub

        Public Sub RollLeft()
            RotY -= RotationStep
        End Sub

        Public Sub RollRight()
            RotY += RotationStep
        End Sub

        ''' <summary>
        ''' Transform a world point to camera-relative coordinates
        ''' </summary>
        Public Sub TransformPoint(worldX As Single, worldY As Single, worldZ As Single,
                                  ByRef camX As Single, ByRef camY As Single, ByRef camZ As Single)
            ' Translate to camera position
            Dim x1 = worldX - PosX
            Dim y1 = worldY - PosY
            Dim z1 = worldZ - PosZ

            ' Rotate about Z axis (yaw)
            Dim cosZ = CSng(Math.Cos(RotZ))
            Dim sinZ = CSng(Math.Sin(RotZ))
            Dim tempX = x1 * cosZ + y1 * sinZ
            Dim tempY = -x1 * sinZ + y1 * cosZ
            x1 = tempX
            y1 = tempY

            ' Rotate about X axis (pitch)
            Dim cosX = CSng(Math.Cos(RotX))
            Dim sinX = CSng(Math.Sin(RotX))
            tempY = y1 * cosX + z1 * sinX
            Dim tempZ = -y1 * sinX + z1 * cosX
            y1 = tempY
            z1 = tempZ

            ' Rotate about Y axis (roll)
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
        ''' Returns False if point is behind camera
        ''' </summary>
        Public Function ProjectToScreen(camX As Single, camY As Single, camZ As Single,
                                        ByRef screenX As Single, ByRef screenY As Single) As Boolean
            If camY <= 1 Then
                Return False
            End If

            screenX = camX / camY
            screenY = camZ / camY
            Return True
        End Function

        ''' <summary>
        ''' Calculate horizon line height based on camera pitch
        ''' </summary>
        Public Function GetHorizonHeight(screenHeight As Integer) As Integer
            Return CInt(-200 * Math.Sin(RotX) + screenHeight / 2)
        End Function

        ''' <summary>
        ''' Calculate horizon twist based on camera roll
        ''' </summary>
        Public Function GetHorizonTwist() As Integer
            Return CInt(100 * Math.Tan(RotY))
        End Function
    End Class
End Namespace
