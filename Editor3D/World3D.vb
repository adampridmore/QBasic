Imports System.IO

Namespace Editor3D
    ''' <summary>
    ''' Represents a 3D point/vertex with connections to other points
    ''' EDITOR.BAS:6-12 - TYPE pts
    ''' </summary>
    Public Class Point3D
        Public X As Single          ' EDITOR.BAS:7 - x AS SINGLE
        Public Y As Single          ' EDITOR.BAS:8 - y AS SINGLE
        Public Z As Single          ' EDITOR.BAS:9 - z AS SINGLE
        Public LinkBegin As Integer = -2  ' EDITOR.BAS:10 - beg AS INTEGER (-2 = unused, -1 = no links)
        Public LinkEnd As Integer = -2    ' EDITOR.BAS:11 - fin AS INTEGER

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
    ''' EDITOR.BAS:14-22 - TYPE obj
    ''' </summary>
    Public Class Object3D
        ' EDITOR.BAS:44 - object(lop).called = "*********"
        Public Const UnusedName As String = "*********"

        Public Name As String = UnusedName  ' EDITOR.BAS:15 - called AS STRING * 9
        Public PosX As Single               ' EDITOR.BAS:16 - posx AS SINGLE
        Public PosY As Single               ' EDITOR.BAS:17 - posy AS SINGLE
        Public PosZ As Single               ' EDITOR.BAS:18 - posz AS SINGLE
        Public Color As Integer = 7         ' EDITOR.BAS:19 - col AS INTEGER
        Public PointBegin As Integer        ' EDITOR.BAS:20 - beg AS INTEGER
        Public PointEnd As Integer          ' EDITOR.BAS:21 - fin AS INTEGER

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
        ' EDITOR.BAS:29-31 - DIM statements
        Public Const MaxObjects As Integer = 100  ' EDITOR.BAS:29 - DIM object(1 TO 100) AS obj
        Public Const MaxPoints As Integer = 100   ' EDITOR.BAS:30 - DIM pt(1 TO 100) AS pts
        Public Const MaxLinks As Integer = 500    ' EDITOR.BAS:31 - DIM link(500)

        Public Objects(MaxObjects - 1) As Object3D
        Public Points(MaxPoints - 1) As Point3D
        Public Links(MaxLinks - 1) As Integer

        Public Sub New()
            Initialize()
            CreateDefaultScene()
        End Sub

        ''' <summary>
        ''' Initialize arrays - EDITOR.BAS:33-45
        ''' </summary>
        Private Sub Initialize()
            ' EDITOR.BAS:43-45 - Initialize objects
            For i = 0 To MaxObjects - 1
                Objects(i) = New Object3D()
            Next

            ' EDITOR.BAS:33-36 - Initialize points with beg/fin = -2
            For i = 0 To MaxPoints - 1
                Points(i) = New Point3D()
            Next

            ' EDITOR.BAS:39-41 - Initialize links to -1
            For i = 0 To MaxLinks - 1
                Links(i) = -1
            Next
        End Sub

        ''' <summary>
        ''' Create the default scene with ground, trees, and house
        ''' EDITOR.BAS:62-248
        ''' </summary>
        Private Sub CreateDefaultScene()
            ' EDITOR.BAS:63-98 - Create ground plane (object 1, now 0)
            Objects(0).Name = "ground"
            Objects(0).PosX = 0
            Objects(0).PosY = 0
            Objects(0).PosZ = 0
            Objects(0).Color = 14 ' Yellow
            Objects(0).PointBegin = 0
            Objects(0).PointEnd = 3

            ' EDITOR.BAS:71-93 - Ground points (pt 1-4, now 0-3)
            Points(0) = New Point3D(-10, -10, 0, 0, 1)
            Points(1) = New Point3D(10, -10, 0, 2, 2)
            Points(2) = New Point3D(-10, 10, 0, 3, 3)
            Points(3) = New Point3D(10, 10, 0, -1, -1)

            ' EDITOR.BAS:95-98 - Ground links
            Links(0) = 1
            Links(1) = 2
            Links(2) = 3
            Links(3) = 3

            ' EDITOR.BAS:101-150 - Create tree template (object 2, now 1)
            Objects(1).Name = "tree"
            Objects(1).PosX = 8
            Objects(1).PosY = 8
            Objects(1).PosZ = 0
            Objects(1).Color = 2 ' Green
            Objects(1).PointBegin = 4
            Objects(1).PointEnd = 9

            ' EDITOR.BAS:110-144 - Tree points (pt 5-10, now 4-9)
            Points(4) = New Point3D(0, 0, 0, 4, 4)
            Points(5) = New Point3D(0, 0, -2, 5, 8)
            Points(6) = New Point3D(1, 1, -0.8F, -1, -1)
            Points(7) = New Point3D(-1, 1, -0.8F, -1, -1)
            Points(8) = New Point3D(1, -1, -0.8F, -1, -1)
            Points(9) = New Point3D(-1, -1, -0.8F, -1, -1)

            ' EDITOR.BAS:146-150 - Tree links
            Links(4) = 5
            Links(5) = 6
            Links(6) = 7
            Links(7) = 8
            Links(8) = 9

            ' EDITOR.BAS:152-236 - Create house (object 3, now 2)
            Objects(2).Name = "house"
            Objects(2).PosX = 0
            Objects(2).PosY = 0
            Objects(2).PosZ = 0
            Objects(2).Color = 7 ' White/Gray
            Objects(2).PointBegin = 10
            Objects(2).PointEnd = 19

            ' EDITOR.BAS:160-182 - House base vertices (pt 11-14, now 10-13)
            Points(10) = New Point3D(1, -1, 0, 9, 11)
            Points(11) = New Point3D(1, 1, 0, 12, 13)
            Points(12) = New Point3D(-1, 1, 0, 14, 15)
            Points(13) = New Point3D(-1, -1, 0, 16, 16)

            ' EDITOR.BAS:184-206 - House top vertices (pt 15-18, now 14-17)
            Points(14) = New Point3D(1, -1, -2, 17, 19)
            Points(15) = New Point3D(1, 1, -2, 20, 21)
            Points(16) = New Point3D(-1, 1, -2, 22, 23)
            Points(17) = New Point3D(-1, -1, -2, 24, 24)

            ' EDITOR.BAS:208-218 - Roof vertices (pt 19-20, now 18-19)
            Points(18) = New Point3D(-1, 0, -3, 25, 25)
            Points(19) = New Point3D(1, 0, -3, -1, -1)

            ' EDITOR.BAS:220-236 - House links
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

            ' EDITOR.BAS:240-248 - Create random trees (objects 4-13, now 3-12)
            ' FOR lop = 1 TO 10
            Dim rnd As New Random(42) ' Fixed seed for reproducibility
            For i = 0 To 9
                Dim objIndex = i + 3
                Objects(objIndex).Name = "tree"
                Objects(objIndex).PointBegin = Objects(1).PointBegin
                Objects(objIndex).PointEnd = Objects(1).PointEnd
                Objects(objIndex).Color = Objects(1).Color
                ' EDITOR.BAS:245-246 - RND * 20 - 10
                Objects(objIndex).PosX = CSng(rnd.NextDouble() * 20 - 10)
                Objects(objIndex).PosY = CSng(rnd.NextDouble() * 20 - 10)
                Objects(objIndex).PosZ = 0
            Next
        End Sub

        ''' <summary>
        ''' Count used objects - EDITOR.BAS:529-532
        ''' </summary>
        Public Function GetObjectCount() As Integer
            Dim count = 0
            For Each obj In Objects
                If obj.IsUsed Then count += 1
            Next
            Return count
        End Function

        ''' <summary>
        ''' Count used points - EDITOR.BAS:534-539
        ''' </summary>
        Public Function GetPointCount() As Integer
            Dim count = 0
            For Each pt In Points
                If pt.IsUsed Then count += 1
            Next
            Return count
        End Function

        ''' <summary>
        ''' Count used links - EDITOR.BAS:541-545
        ''' </summary>
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
        ''' EDITOR.BAS:689-711 - IF button$ = "c" THEN
        ''' </summary>
        Public Function CopyObject(sourceIndex As Integer) As Integer
            Dim nextIndex = GetNextFreeObjectIndex()
            If nextIndex = -1 Then Return -1

            ' EDITOR.BAS:701-707
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
        ''' EDITOR.BAS:562-683 - IF button$ = "d" THEN
        ''' </summary>
        Public Sub DeleteObject(index As Integer)
            ' EDITOR.BAS:571-580 - Shuffle objects down
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

            CompactPoints()
        End Sub

        ''' <summary>
        ''' Clean up unused points - EDITOR.BAS:584-681
        ''' </summary>
        Private Sub CompactPoints()
            ' EDITOR.BAS:586-588 - Clear used array
            Dim used(MaxPoints - 1) As Boolean

            ' EDITOR.BAS:593-599 - Mark used points
            For Each obj In Objects
                If obj.IsUsed Then
                    For i = obj.PointBegin To obj.PointEnd
                        If i >= 0 AndAlso i < MaxPoints Then
                            used(i) = True
                        End If
                    Next
                End If
            Next

            ' EDITOR.BAS:601-607 - Clear unused points
            For i = 0 To MaxPoints - 1
                If Not used(i) Then
                    Points(i).LinkBegin = -2
                    Points(i).LinkEnd = -2
                End If
            Next
        End Sub

        ''' <summary>
        ''' Move an object by the specified amounts
        ''' EDITOR.BAS:801-827 - moveobject:
        ''' </summary>
        Public Sub MoveObject(index As Integer, dx As Single, dy As Single, dz As Single)
            ' EDITOR.BAS:814-816
            Objects(index).PosX += dx
            Objects(index).PosY += dy
            Objects(index).PosZ += dz
        End Sub

        ''' <summary>
        ''' Save world to a .wld file
        ''' EDITOR.BAS:324-373 - IF button$ = "s" THEN (save world)
        ''' </summary>
        Public Sub SaveWorld(filename As String)
            Using writer As New StreamWriter(filename)
                ' EDITOR.BAS:342
                writer.WriteLine($"This is a world file called {Path.GetFileName(filename)}")
                writer.WriteLine()
                ' EDITOR.BAS:344
                writer.WriteLine(" This is the object data! (including nulls)")

                ' EDITOR.BAS:345-354 - Write object data
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

                ' EDITOR.BAS:355-363 - Write point data
                writer.WriteLine("this is th pt data")
                For i = 0 To MaxPoints - 1
                    writer.WriteLine(Points(i).X)
                    writer.WriteLine(Points(i).Y)
                    writer.WriteLine(Points(i).Z)
                    writer.WriteLine(Points(i).LinkBegin)
                    writer.WriteLine(Points(i).LinkEnd)
                    writer.WriteLine()
                Next

                ' EDITOR.BAS:364-368 - Write link data
                writer.WriteLine("this is the link data")
                For i = 0 To MaxLinks - 1
                    writer.WriteLine(Links(i))
                Next

                ' EDITOR.BAS:370
                writer.WriteLine("eof")
            End Using
        End Sub

        ''' <summary>
        ''' Load world from a .wld file
        ''' EDITOR.BAS:376-447 - IF button$ = "l" THEN (load world)
        ''' </summary>
        Public Function LoadWorld(filename As String) As Boolean
            Try
                Using reader As New StreamReader(filename)
                    Dim header = reader.ReadLine()
                    ' EDITOR.BAS:398 - Check for valid file
                    If Not header.StartsWith("This is a world file called ") Then
                        Return False
                    End If

                    ' EDITOR.BAS:400-404 - Clear current world
                    Initialize()

                    reader.ReadLine() ' Empty line
                    reader.ReadLine() ' "This is the object data..."

                    ' EDITOR.BAS:407-423 - Read object data
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

                    ' EDITOR.BAS:424-437 - Read point data
                    reader.ReadLine() ' "this is th pt data"
                    For i = 0 To MaxPoints - 1
                        Points(i).X = Single.Parse(reader.ReadLine())
                        Points(i).Y = Single.Parse(reader.ReadLine())
                        Points(i).Z = Single.Parse(reader.ReadLine())
                        Points(i).LinkBegin = Integer.Parse(reader.ReadLine())
                        Points(i).LinkEnd = Integer.Parse(reader.ReadLine())
                        reader.ReadLine() ' Empty line
                    Next

                    ' EDITOR.BAS:438-442 - Read link data
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
        ''' EDITOR.BAS:450-496 - IF button$ = "o" THEN (save object)
        ''' </summary>
        Public Sub SaveObject(index As Integer, filename As String)
            Dim obj = Objects(index)

            Using writer As New StreamWriter(filename)
                ' EDITOR.BAS:462
                writer.WriteLine("this is an object file")
                writer.WriteLine()
                ' EDITOR.BAS:464-470
                writer.WriteLine(obj.Name)
                writer.WriteLine(obj.PointBegin)
                writer.WriteLine(obj.PointEnd)
                writer.WriteLine(obj.Color)
                writer.WriteLine(obj.PosX)
                writer.WriteLine(obj.PosY)
                writer.WriteLine(obj.PosZ)
                writer.WriteLine()
                ' EDITOR.BAS:472
                writer.WriteLine("pt's list")
                writer.WriteLine()

                ' EDITOR.BAS:474-482 - Write points
                For i = obj.PointBegin To obj.PointEnd
                    writer.WriteLine(i)
                    writer.WriteLine(Points(i).X)
                    writer.WriteLine(Points(i).Y)
                    writer.WriteLine(Points(i).Z)
                    writer.WriteLine(Points(i).LinkBegin)
                    writer.WriteLine(Points(i).LinkEnd)
                    writer.WriteLine()
                Next

                ' EDITOR.BAS:483-492 - Write links
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

                ' EDITOR.BAS:493
                writer.WriteLine("eof")
            End Using
        End Sub
    End Class
End Namespace
