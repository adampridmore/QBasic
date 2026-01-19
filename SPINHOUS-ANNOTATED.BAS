' ============================================================================
' SPINHOUS.BAS - Spinning 3D Wireframe House
' ============================================================================
' A classic QBasic program that renders a 3D wireframe house with a porch
' and rotates it in real-time. Uses simple perspective projection.
'
' Controls:
'   Z - Rotate left
'   X - Rotate right
'   Space - Stop rotation
'   K - Move house away (further from camera)
'   M - Move house closer
'   Q - Quit
' ============================================================================

' ----------------------------------------------------------------------------
' FUNCTION DECLARATIONS
' ----------------------------------------------------------------------------
DECLARE FUNCTION screenposx! (x!, y!, z!)   ' Projects 3D X coordinate to screen
DECLARE FUNCTION screenposy! (x!, y!, z!)   ' Projects 3D Z coordinate to screen Y

' ----------------------------------------------------------------------------
' CONSTANTS AND INITIALIZATION
' ----------------------------------------------------------------------------
CONST pi = 3.141592654#
SCREEN 1                        ' 320x200 CGA graphics mode
CLS
angle = 5                       ' Rotation angle in degrees per frame
dir = 0                         ' Rotation direction: -1=left, 0=stop, 1=right

' Pre-calculate rotation matrix values for the rotation angle
c# = COS(angle / 180 * pi)      ' Cosine of rotation angle
s# = SIN(angle / 180 * pi)      ' Sine of rotation angle

' Pre-calculate 90-degree rotation values (for initial orientation)
c2# = COS(pi / 2)               ' Cosine of 90 degrees
s2# = SIN(pi / 2)               ' Sine of 90 degrees

' ----------------------------------------------------------------------------
' 3D MODEL DATA STRUCTURE
' ----------------------------------------------------------------------------
' The house model is stored in a 2D array where:
'   cube(n, 1) = X coordinate of vertex n
'   cube(n, 2) = Y coordinate of vertex n (depth - used for perspective)
'   cube(n, 3) = Z coordinate of vertex n (height on screen)
'   cube(n, 4+) = Indices of connected vertices (for drawing lines)
'
' A value of -1 indicates no connection (end of connection list)
' ----------------------------------------------------------------------------

cubesize = 28                   ' Total number of vertices in the model

DIM cube(cubesize, cubesize + 3)

' Initialize all connections to -1 (no connection)
FOR lop = 1 TO cubesize
        FOR lop2 = 1 TO cubesize + 3
                cube(lop, lop2) = -1
        NEXT
NEXT

' ----------------------------------------------------------------------------
' HOUSE BASE VERTICES (Points 1-8)
' These form the main rectangular body of the house
' ----------------------------------------------------------------------------
' Point 1: Front-left-bottom corner
cube(1, 1) = 1
cube(1, 2) = 0
cube(1, 3) = 0
cube(1, 4) = 2          ' Connect to point 2
cube(1, 5) = 3          ' Connect to point 3
cube(1, 6) = 5          ' Connect to point 5

' Point 2: Front-right-bottom corner
cube(2, 1) = 2
cube(2, 2) = 0
cube(2, 3) = 0
cube(2, 4) = 6          ' Connect to point 6
cube(2, 5) = 4          ' Connect to point 4
cube(2, 6) = 20         ' Connect to porch point 20

' Point 3: Back-left-bottom corner
cube(3, 1) = 1
cube(3, 2) = 0
cube(3, 3) = 1
cube(3, 4) = 4          ' Connect to point 4
cube(3, 5) = 8          ' Connect to point 8

' Point 4: Back-right-bottom corner
cube(4, 1) = 2
cube(4, 2) = 0
cube(4, 3) = 1
cube(4, 4) = 7          ' Connect to point 7
cube(4, 5) = 14         ' Connect to porch point 14

' Point 5: Front-left-top corner
cube(5, 1) = 1
cube(5, 2) = 1
cube(5, 3) = 0
cube(5, 4) = 6          ' Connect to point 6
cube(5, 5) = 9          ' Connect to roof point 9
cube(5, 6) = 8          ' Connect to point 8

' Point 6: Front-right-top corner
cube(6, 1) = 2
cube(6, 2) = 1
cube(6, 3) = 0
cube(6, 4) = 7          ' Connect to point 7
cube(6, 5) = 10         ' Connect to roof point 10

' Point 7: Back-right-top corner
cube(7, 1) = 2
cube(7, 2) = 1
cube(7, 3) = 1
cube(7, 4) = 10         ' Connect to roof point 10
cube(7, 5) = 8          ' Connect to point 8

' Point 8: Back-left-top corner
cube(8, 1) = 1
cube(8, 2) = 1
cube(8, 3) = 1
cube(8, 4) = 9          ' Connect to roof point 9

' ----------------------------------------------------------------------------
' ROOF PEAK VERTICES (Points 9-10)
' These form the triangular roof peak
' ----------------------------------------------------------------------------
' Point 9: Left roof peak
cube(9, 1) = 1.2
cube(9, 2) = 1.5
cube(9, 3) = .5
cube(9, 4) = 10         ' Connect to right roof peak

' Point 10: Right roof peak
cube(10, 1) = 1.8
cube(10, 2) = 1.5
cube(10, 3) = .5

' ----------------------------------------------------------------------------
' PORCH FRAME VERTICES (Points 11-20)
' These form the porch/doorway structure extending from the house
' ----------------------------------------------------------------------------
' Point 11: Porch inner left-bottom
cube(11, 1) = 2
cube(11, 2) = .2
cube(11, 3) = 0
cube(11, 4) = 19

' Point 12: Porch inner left-top
cube(12, 1) = 2
cube(12, 2) = .2
cube(12, 3) = 1
cube(12, 4) = 13

' Point 13: Porch outer left-top
cube(13, 1) = 3
cube(13, 2) = .2
cube(13, 3) = 1
cube(13, 4) = 15
cube(13, 5) = 14

' Point 14: Porch outer left-bottom
cube(14, 1) = 3
cube(14, 2) = 0
cube(14, 3) = 1
cube(14, 4) = 20

' Point 15: Porch door frame top-left
cube(15, 1) = 3
cube(15, 2) = .2
cube(15, 3) = .55
cube(15, 4) = 16

' Point 16: Porch door frame bottom-left
cube(16, 1) = 3
cube(16, 2) = 0
cube(16, 3) = .55

' Point 17: Porch door frame top-right
cube(17, 1) = 3
cube(17, 2) = .2
cube(17, 3) = .45
cube(17, 4) = 19
cube(17, 5) = 18

' Point 18: Porch door frame bottom-right
cube(18, 1) = 3
cube(18, 2) = 0
cube(18, 3) = .45
cube(18, 4) = 20

' Point 19: Porch inner right-top
cube(19, 1) = 3
cube(19, 2) = .2
cube(19, 3) = 0
cube(19, 4) = 20

' Point 20: Porch outer right-bottom
cube(20, 1) = 3
cube(20, 2) = 0
cube(20, 3) = 0

' ----------------------------------------------------------------------------
' WINDOW VERTICES (Points 21-24)
' A small rectangular window on the house
' ----------------------------------------------------------------------------
' Point 21: Window bottom-left
cube(21, 1) = 2
cube(21, 2) = 0
cube(21, 3) = .45
cube(21, 4) = 21        ' Self-reference (connects in loop)
cube(21, 5) = 23

' Point 22: Window bottom-right
cube(22, 1) = 2
cube(22, 2) = 0
cube(22, 3) = .55
cube(22, 4) = 24

' Point 23: Window top-left
cube(23, 1) = 2
cube(23, 2) = .3
cube(23, 3) = .45
cube(23, 4) = 24

' Point 24: Window top-right
cube(24, 1) = 2
cube(24, 2) = .3
cube(24, 3) = .55

' ----------------------------------------------------------------------------
' ORIGIN AXES MARKER (Points 25-28)
' A small XYZ axis indicator at the origin for reference
' ----------------------------------------------------------------------------
' Point 25: Origin
cube(25, 1) = 0
cube(25, 2) = 0
cube(25, 3) = 0
cube(25, 4) = 26        ' Connect to X axis end
cube(25, 5) = 27        ' Connect to Y axis end
cube(25, 6) = 28        ' Connect to Z axis end

' Point 26: X axis end
cube(26, 1) = .2
cube(26, 2) = 0
cube(26, 3) = 0

' Point 27: Y axis end
cube(27, 1) = 0
cube(27, 2) = .2
cube(27, 3) = 0

' Point 28: Z axis end
cube(28, 1) = 0
cube(28, 2) = 0
cube(28, 3) = .2

' ----------------------------------------------------------------------------
' TRANSLATE MODEL TO CENTER
' Move the house so it's centered and positioned in front of the camera
' ----------------------------------------------------------------------------
trick = 1
FOR lop = 1 TO cubesize
        cube(lop, 1) = cube(lop, 1) - 1.5   ' Center X
        cube(lop, 2) = cube(lop, 2) + 3.5   ' Push back (away from camera)
        cube(lop, 3) = cube(lop, 3) - .5    ' Center Z (vertical)
NEXT

' ----------------------------------------------------------------------------
' CALCULATE CENTER OF ROTATION
' Uses points 1 and 7 (opposite corners) to find the center
' ----------------------------------------------------------------------------
centrex = ((cube(7, 1) - cube(1, 1)) / 2) + cube(1, 1)
centrey = ((cube(7, 2) - cube(1, 2)) / 2) + cube(1, 2)
centrez = ((cube(7, 3) - cube(1, 3)) / 2) + cube(1, 3)

' ----------------------------------------------------------------------------
' INITIAL 90-DEGREE ROTATION
' Rotate the house 90 degrees around the X axis to orient it properly
' This makes the house appear upright on screen
' ----------------------------------------------------------------------------
FOR lop = 1 TO cubesize
        tempy = c2# * (cube(lop, 2) - centrey) + s2# * (cube(lop, 3) - centrez)
        tempz = -s2# * (cube(lop, 2) - centrey) + c2# * (cube(lop, 3) - centrez)
        tempy = tempy + centrey
        tempz = tempz + centrez
        cube(lop, 2) = tempy
        cube(lop, 3) = tempz
NEXT

' ============================================================================
' MAIN ANIMATION LOOP
' ============================================================================
WHILE NOT (button$ = "q")
        button$ = INKEY$                ' Check for keyboard input
        FOR lop = 1 TO 1000: NEXT lop   ' Delay loop (crude frame rate control)
        CLS                             ' Clear screen
        trick = trick + 1

        ' --------------------------------------------------------------------
        ' RENDER ALL VERTICES AND THEIR CONNECTIONS
        ' --------------------------------------------------------------------
        FOR lop = 1 TO cubesize
                count = 1
                ' Project current vertex to screen coordinates
                x = screenposx(cube(lop, 1), cube(lop, 2), cube(lop, 3))
                y = screenposy(cube(lop, 1), cube(lop, 2), cube(lop, 3))

                ' Skip if vertex is outside visible area
                IF NOT (y < -.5 OR x < -.5 OR x > .5 OR y > .5) THEN
                        ' Draw lines to all connected vertices
                        WHILE (NOT (cube(lop, count + 3) = -1)) OR (count + 3 >= cubesize)
                                point2 = cube(lop, count + 3)
                                ' Project connected vertex
                                x1 = screenposx(cube(point2, 1), cube(point2, 2), cube(point2, 3))
                                y1 = screenposy(cube(point2, 1), cube(point2, 2), cube(point2, 3))

                                ' Draw line if connected vertex is also visible
                                IF NOT (x1 < -.5 OR y1 < -.5 OR x1 > .5 OR y1 > .5) THEN
                                        ' Scale normalized coordinates to screen pixels
                                        ' x,y range from -0.5 to 0.5, mapped to 0-320 and 0-200
                                        LINE (INT((x + .5) * 320), INT((y + .5) * 200))-(INT((x1 + .5) * 320), INT((y1 + .5) * 200))
                                END IF

                                count = count + 1

                        WEND
                END IF
        NEXT

        ' --------------------------------------------------------------------
        ' RECALCULATE CENTER OF ROTATION
        ' (Center may shift slightly due to floating point)
        ' --------------------------------------------------------------------
        centrex = ((cube(7, 1) - cube(1, 1)) / 2) + cube(1, 1)
        centrey = ((cube(7, 2) - cube(1, 2)) / 2) + cube(1, 2)
        centrez = ((cube(7, 3) - cube(1, 3)) / 2) + cube(1, 3)

        ' --------------------------------------------------------------------
        ' HANDLE KEYBOARD INPUT
        ' --------------------------------------------------------------------
        IF button$ = "z" THEN dir = -1  ' Rotate left
        IF button$ = "x" THEN dir = 1   ' Rotate right
        IF button$ = " " THEN dir = 0   ' Stop rotation

        ' --------------------------------------------------------------------
        ' APPLY ROTATION TRANSFORMATION
        ' Rotates around the Y axis (vertical) using 2D rotation matrix
        ' --------------------------------------------------------------------
        IF NOT (dir = 0) THEN
                FOR lop = 1 TO cubesize
                        ' Apply rotation matrix around center point
                        tempx = c# * (cube(lop, 1) - centrex) + dir * s# * (cube(lop, 2) - centrey)
                        tempy = -dir * s# * (cube(lop, 1) - centrex) + c# * (cube(lop, 2) - centrey)
                        tempx = tempx + centrex
                        tempy = tempy + centrey
                        cube(lop, 1) = tempx
                        cube(lop, 2) = tempy
                NEXT
        END IF

        ' --------------------------------------------------------------------
        ' HANDLE DEPTH MOVEMENT (K/M keys)
        ' Move house closer or further from camera
        ' --------------------------------------------------------------------
        IF button$ = "k" THEN
                FOR lop = 1 TO cubesize
                        cube(lop, 2) = cube(lop, 2) + .1   ' Move away
                NEXT
        END IF

        IF button$ = "m" THEN
                FOR lop = 1 TO cubesize
                        cube(lop, 2) = cube(lop, 2) - .1   ' Move closer
                NEXT
        END IF

WEND

' ============================================================================
' PERSPECTIVE PROJECTION FUNCTIONS
' ============================================================================
' These functions convert 3D coordinates to 2D screen coordinates using
' simple perspective division. The Y coordinate (depth) is used as the
' divisor to create the perspective effect - objects further away appear
' smaller.
'
' Returns normalized coordinates in range -0.5 to 0.5
' ============================================================================

FUNCTION screenposx (x, y, z)
        IF y = 0 THEN
                screenposx = -1         ' Avoid division by zero
        ELSE
                screenposx = x / y      ' Perspective divide: X / depth
        END IF
END FUNCTION

FUNCTION screenposy (x, y, z)
        IF y = 0 THEN
                screenposy = -1         ' Avoid division by zero
        ELSE
                screenposy = z / y      ' Perspective divide: Z / depth
        END IF

END FUNCTION
