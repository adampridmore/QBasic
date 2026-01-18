Imports System.Drawing
Imports System.IO

Namespace Editor3D
    ''' <summary>
    ''' Represents a 3D point/vertex with connections to other points
    ''' </summary>
    Public Class Point3D
        Public X As Single
        Public Y As Single
        Public Z As Single
        Public LinkBegin As Integer = -2  ' -2 = unused, -1 = no links
        Public LinkEnd As Integer = -2

        Public Sub New()
        End Sub

        Public Sub New(x As Single, y As Single, z As Single)
            Me.X = x
            Me.Y = y
            Me.Z = z
            Me.LinkBegin = -1
            Me.LinkEnd = -1
        End Sub

        Public Sub New(x As Single, y As Single, z As Single, linkBegin As Integer, linkEnd As Integer)
            Me.X = x
            Me.Y = y
            Me.Z = z
            Me.LinkBegin = linkBegin
            Me.LinkEnd = linkEnd
        End Sub

        Public ReadOnly Property IsUsed As Boolean
            Get
                Return LinkBegin <> -2
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Represents a 3D object in the scene
    ''' </summary>
    Public Class Object3D
        Public Const UnusedName As String = "*********"

        Public Name As String = UnusedName
        Public PosX As Single
        Public PosY As Single
        Public PosZ As Single
        Public Color As Integer = 7
        Public PointBegin As Integer  ' Index into points array
        Public PointEnd As Integer    ' Index into points array

        Public ReadOnly Property IsUsed As Boolean
            Get
                Return Name <> UnusedName
            End Get
        End Property

        Public ReadOnly Property PointCount As Integer
            Get
                Return PointEnd - PointBegin + 1
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Contains the 3D world data and operations
    ''' Ported from EDITOR.BAS
    ''' </summary>
    Public Class World3D
        Public Const MaxObjects As Integer = 100
        Public Const MaxPoints As Integer = 100
        Public Const MaxLinks As Integer = 500

        Public Objects(MaxObjects - 1) As Object3D
        Public Points(MaxPoints - 1) As Point3D
        Public Links(MaxLinks - 1) As Integer

        Public Sub New()
            Initialize()
            CreateDefaultScene()
        End Sub

        Private Sub Initialize()
            ' Initialize all objects
            For i = 0 To MaxObjects - 1
                Objects(i) = New Object3D()
            Next

            ' Initialize all points
            For i = 0 To MaxPoints - 1
                Points(i) = New Point3D()
            Next

            ' Initialize all links to -1 (unused)
            For i = 0 To MaxLinks - 1
                Links(i) = -1
            Next
        End Sub

        Private Sub CreateDefaultScene()
            ' Create ground plane (object 0)
            Objects(0).Name = "ground"
            Objects(0).PosX = 0
            Objects(0).PosY = 0
            Objects(0).PosZ = 0
            Objects(0).Color = 14 ' Yellow
            Objects(0).PointBegin = 0
            Objects(0).PointEnd = 3

            Points(0) = New Point3D(-10, -10, 0, 0, 1)
            Points(1) = New Point3D(10, -10, 0, 2, 2)
            Points(2) = New Point3D(-10, 10, 0, 3, 3)
            Points(3) = New Point3D(10, 10, 0, -1, -1)

            Links(0) = 1
            Links(1) = 2
            Links(2) = 3
            Links(3) = 3

            ' Create tree template (object 1)
            Objects(1).Name = "tree"
            Objects(1).PosX = 8
            Objects(1).PosY = 8
            Objects(1).PosZ = 0
            Objects(1).Color = 2 ' Green
            Objects(1).PointBegin = 4
            Objects(1).PointEnd = 9

            Points(4) = New Point3D(0, 0, 0, 4, 4)
            Points(5) = New Point3D(0, 0, -2, 5, 8)
            Points(6) = New Point3D(1, 1, -0.8F, -1, -1)
            Points(7) = New Point3D(-1, 1, -0.8F, -1, -1)
            Points(8) = New Point3D(1, -1, -0.8F, -1, -1)
            Points(9) = New Point3D(-1, -1, -0.8F, -1, -1)

            Links(4) = 5
            Links(5) = 6
            Links(6) = 7
            Links(7) = 8
            Links(8) = 9

            ' Create house (object 2)
            Objects(2).Name = "house"
            Objects(2).PosX = 0
            Objects(2).PosY = 0
            Objects(2).PosZ = 0
            Objects(2).Color = 7 ' White/Gray
            Objects(2).PointBegin = 10
            Objects(2).PointEnd = 19

            ' House base vertices
            Points(10) = New Point3D(1, -1, 0, 9, 11)
            Points(11) = New Point3D(1, 1, 0, 12, 13)
            Points(12) = New Point3D(-1, 1, 0, 14, 15)
            Points(13) = New Point3D(-1, -1, 0, 16, 16)

            ' House top vertices
            Points(14) = New Point3D(1, -1, -2, 17, 19)
            Points(15) = New Point3D(1, 1, -2, 20, 21)
            Points(16) = New Point3D(-1, 1, -2, 22, 23)
            Points(17) = New Point3D(-1, -1, -2, 24, 24)

            ' Roof vertices
            Points(18) = New Point3D(-1, 0, -3, 25, 25)
            Points(19) = New Point3D(1, 0, -3, -1, -1)

            ' House links
            Links(9) = 11
            Links(10) = 13
            Links(11) = 14
            Links(12) = 12
            Links(13) = 15
            Links(14) = 13
            Links(15) = 16
            Links(16) = 17
            Links(17) = 15
            Links(18) = 17
            Links(19) = 19
            Links(20) = 16
            Links(21) = 19
            Links(22) = 17
            Links(23) = 18
            Links(24) = 18
            Links(25) = 19

            ' Create random trees (objects 3-12)
            Dim rnd As New Random(42) ' Fixed seed for reproducibility
            For i = 0 To 9
                Dim objIndex = i + 3
                Objects(objIndex).Name = "tree"
                Objects(objIndex).PointBegin = Objects(1).PointBegin
                Objects(objIndex).PointEnd = Objects(1).PointEnd
                Objects(objIndex).Color = Objects(1).Color
                Objects(objIndex).PosX = CSng(rnd.NextDouble() * 20 - 10)
                Objects(objIndex).PosY = CSng(rnd.NextDouble() * 20 - 10)
                Objects(objIndex).PosZ = 0
            Next
        End Sub

        Public Function GetObjectCount() As Integer
            Dim count = 0
            For Each obj In Objects
                If obj.IsUsed Then count += 1
            Next
            Return count
        End Function

        Public Function GetPointCount() As Integer
            Dim count = 0
            For Each pt In Points
                If pt.IsUsed Then count += 1
            Next
            Return count
        End Function

        Public Function GetLinkCount() As Integer
            Dim count = 0
            For Each link In Links
                If link <> -1 Then count += 1
            Next
            Return count
        End Function

        Public Function GetNextFreeObjectIndex() As Integer
            For i = 0 To MaxObjects - 1
                If Not Objects(i).IsUsed Then Return i
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Copy an object to the next available slot
        ''' </summary>
        Public Function CopyObject(sourceIndex As Integer) As Integer
            Dim nextIndex = GetNextFreeObjectIndex()
            If nextIndex = -1 Then Return -1

            Dim source = Objects(sourceIndex)
            Objects(nextIndex).Name = source.Name
            Objects(nextIndex).PosX = source.PosX
            Objects(nextIndex).PosY = source.PosY
            Objects(nextIndex).PosZ = source.PosZ
            Objects(nextIndex).Color = source.Color
            Objects(nextIndex).PointBegin = source.PointBegin
            Objects(nextIndex).PointEnd = source.PointEnd

            Return nextIndex
        End Function

        ''' <summary>
        ''' Delete an object and clean up unused points
        ''' </summary>
        Public Sub DeleteObject(index As Integer)
            ' Shuffle objects down
            For i = index To MaxObjects - 2
                Objects(i).Name = Objects(i + 1).Name
                Objects(i).PosX = Objects(i + 1).PosX
                Objects(i).PosY = Objects(i + 1).PosY
                Objects(i).PosZ = Objects(i + 1).PosZ
                Objects(i).Color = Objects(i + 1).Color
                Objects(i).PointBegin = Objects(i + 1).PointBegin
                Objects(i).PointEnd = Objects(i + 1).PointEnd
            Next
            Objects(MaxObjects - 1) = New Object3D()

            ' Mark unused points and compact
            CompactPoints()
        End Sub

        Private Sub CompactPoints()
            ' Find which points are in use
            Dim used(MaxPoints - 1) As Boolean

            For Each obj In Objects
                If obj.IsUsed Then
                    For i = obj.PointBegin To obj.PointEnd
                        If i >= 0 AndAlso i < MaxPoints Then
                            used(i) = True
                        End If
                    Next
                End If
            Next

            ' Clear unused points
            For i = 0 To MaxPoints - 1
                If Not used(i) Then
                    Points(i).LinkBegin = -2
                    Points(i).LinkEnd = -2
                End If
            Next
        End Sub

        ''' <summary>
        ''' Move an object by the specified amounts
        ''' </summary>
        Public Sub MoveObject(index As Integer, dx As Single, dy As Single, dz As Single)
            Objects(index).PosX += dx
            Objects(index).PosY += dy
            Objects(index).PosZ += dz
        End Sub

        ''' <summary>
        ''' Save world to a .wld file
        ''' </summary>
        Public Sub SaveWorld(filename As String)
            Using writer As New StreamWriter(filename)
                writer.WriteLine($"This is a world file called {Path.GetFileName(filename)}")
                writer.WriteLine()
                writer.WriteLine(" This is the object data! (including nulls)")

                For i = 0 To MaxObjects - 1
                    writer.WriteLine(Objects(i).Name)
                    writer.WriteLine(Objects(i).PointBegin)
                    writer.WriteLine(Objects(i).PointEnd)
                    writer.WriteLine(Objects(i).Color)
                    writer.WriteLine(Objects(i).PosX)
                    writer.WriteLine(Objects(i).PosY)
                    writer.WriteLine(Objects(i).PosZ)
                    writer.WriteLine()
                Next

                writer.WriteLine("this is th pt data")
                For i = 0 To MaxPoints - 1
                    writer.WriteLine(Points(i).X)
                    writer.WriteLine(Points(i).Y)
                    writer.WriteLine(Points(i).Z)
                    writer.WriteLine(Points(i).LinkBegin)
                    writer.WriteLine(Points(i).LinkEnd)
                    writer.WriteLine()
                Next

                writer.WriteLine("this is the link data")
                For i = 0 To MaxLinks - 1
                    writer.WriteLine(Links(i))
                Next

                writer.WriteLine("eof")
            End Using
        End Sub

        ''' <summary>
        ''' Load world from a .wld file
        ''' </summary>
        Public Function LoadWorld(filename As String) As Boolean
            Try
                Using reader As New StreamReader(filename)
                    Dim header = reader.ReadLine()
                    If Not header.StartsWith("This is a world file called ") Then
                        Return False
                    End If

                    ' Clear current world
                    Initialize()

                    reader.ReadLine() ' Empty line
                    reader.ReadLine() ' "This is the object data..."

                    For i = 0 To MaxObjects - 1
                        Objects(i).Name = reader.ReadLine()
                        Objects(i).PointBegin = Integer.Parse(reader.ReadLine())
                        Objects(i).PointEnd = Integer.Parse(reader.ReadLine())
                        Objects(i).Color = Integer.Parse(reader.ReadLine())
                        Objects(i).PosX = Single.Parse(reader.ReadLine())
                        Objects(i).PosY = Single.Parse(reader.ReadLine())
                        Objects(i).PosZ = Single.Parse(reader.ReadLine())
                        reader.ReadLine() ' Empty line
                    Next

                    reader.ReadLine() ' "this is th pt data"
                    For i = 0 To MaxPoints - 1
                        Points(i).X = Single.Parse(reader.ReadLine())
                        Points(i).Y = Single.Parse(reader.ReadLine())
                        Points(i).Z = Single.Parse(reader.ReadLine())
                        Points(i).LinkBegin = Integer.Parse(reader.ReadLine())
                        Points(i).LinkEnd = Integer.Parse(reader.ReadLine())
                        reader.ReadLine() ' Empty line
                    Next

                    reader.ReadLine() ' "this is the link data"
                    For i = 0 To MaxLinks - 1
                        Links(i) = Integer.Parse(reader.ReadLine())
                    Next
                End Using

                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Save a single object to an .obj file
        ''' </summary>
        Public Sub SaveObject(index As Integer, filename As String)
            Dim obj = Objects(index)

            Using writer As New StreamWriter(filename)
                writer.WriteLine("this is an object file")
                writer.WriteLine()
                writer.WriteLine(obj.Name)
                writer.WriteLine(obj.PointBegin)
                writer.WriteLine(obj.PointEnd)
                writer.WriteLine(obj.Color)
                writer.WriteLine(obj.PosX)
                writer.WriteLine(obj.PosY)
                writer.WriteLine(obj.PosZ)
                writer.WriteLine()
                writer.WriteLine("pt's list")
                writer.WriteLine()

                For i = obj.PointBegin To obj.PointEnd
                    writer.WriteLine(i)
                    writer.WriteLine(Points(i).X)
                    writer.WriteLine(Points(i).Y)
                    writer.WriteLine(Points(i).Z)
                    writer.WriteLine(Points(i).LinkBegin)
                    writer.WriteLine(Points(i).LinkEnd)
                    writer.WriteLine()
                Next

                writer.WriteLine("link's list")
                For i = obj.PointBegin To obj.PointEnd
                    If Points(i).LinkBegin <> -1 Then
                        For j = Points(i).LinkBegin To Points(i).LinkEnd
                            writer.WriteLine(j)
                            writer.WriteLine(Links(j))
                            writer.WriteLine()
                        Next
                    End If
                Next

                writer.WriteLine("eof")
            End Using
        End Sub
    End Class
End Namespace
