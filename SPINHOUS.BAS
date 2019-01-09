DECLARE FUNCTION screenposx! (x!, y!, z!)
DECLARE FUNCTION screenposy! (x!, y!, z!)
CONST pi = 3.141592654#
SCREEN 1
CLS
angle = 5
dir = 0

c# = COS(angle / 180 * pi)
s# = SIN(angle / 180 * pi)

c2# = COS(pi / 2)
s2# = SIN(pi / 2)



cubesize = 28



DIM cube(cubesize, cubesize + 3)
FOR lop = 1 TO cubesize
        FOR lop2 = 1 TO cubesize + 3
                cube(lop, lop2) = -1
        NEXT
NEXT
cube(1, 1) = 1
cube(1, 2) = 0
cube(1, 3) = 0
cube(1, 4) = 2
cube(1, 5) = 3
cube(1, 6) = 5
cube(2, 1) = 2
cube(2, 2) = 0
cube(2, 3) = 0
cube(2, 4) = 6
cube(2, 5) = 4
cube(2, 6) = 20
cube(3, 1) = 1
cube(3, 2) = 0
cube(3, 3) = 1
cube(3, 4) = 4
cube(3, 5) = 8
cube(4, 1) = 2
cube(4, 2) = 0
cube(4, 3) = 1
cube(4, 4) = 7
cube(4, 5) = 14
cube(5, 1) = 1
cube(5, 2) = 1
cube(5, 3) = 0
cube(5, 4) = 6
cube(5, 5) = 9
cube(5, 6) = 8
cube(6, 1) = 2
cube(6, 2) = 1
cube(6, 3) = 0
cube(6, 4) = 7
cube(6, 5) = 10
cube(7, 1) = 2
cube(7, 2) = 1
cube(7, 3) = 1
cube(7, 4) = 10
cube(7, 5) = 8
cube(8, 1) = 1
cube(8, 2) = 1
cube(8, 3) = 1
cube(8, 4) = 9
cube(9, 1) = 1.2
cube(9, 2) = 1.5
cube(9, 3) = .5
cube(9, 4) = 10
cube(10, 1) = 1.8
cube(10, 2) = 1.5
cube(10, 3) = .5
cube(11, 1) = 2
cube(11, 2) = .2
cube(11, 3) = 0
cube(11, 4) = 19
cube(12, 1) = 2
cube(12, 2) = .2
cube(12, 3) = 1
cube(12, 4) = 13
cube(13, 1) = 3
cube(13, 2) = .2
cube(13, 3) = 1
cube(13, 4) = 15
cube(13, 5) = 14
cube(14, 1) = 3
cube(14, 2) = 0
cube(14, 3) = 1
cube(14, 4) = 20
cube(15, 1) = 3
cube(15, 2) = .2
cube(15, 3) = .55
cube(15, 4) = 16
cube(16, 1) = 3
cube(16, 2) = 0
cube(16, 3) = .55
cube(17, 1) = 3
cube(17, 2) = .2
cube(17, 3) = .45
cube(17, 4) = 19
cube(17, 5) = 18
cube(18, 1) = 3
cube(18, 2) = 0
cube(18, 3) = .45
cube(18, 4) = 20
cube(19, 1) = 3
cube(19, 2) = .2
cube(19, 3) = 0
cube(19, 4) = 20
cube(20, 1) = 3
cube(20, 2) = 0
cube(20, 3) = 0
cube(21, 1) = 2
cube(21, 2) = 0
cube(21, 3) = .45
cube(21, 4) = 21
cube(21, 5) = 23
cube(22, 1) = 2
cube(22, 2) = 0
cube(22, 3) = .55
cube(22, 4) = 24
cube(23, 1) = 2
cube(23, 2) = .3
cube(23, 3) = .45
cube(23, 4) = 24
cube(24, 1) = 2
cube(24, 2) = .3
cube(24, 3) = .55
cube(25, 1) = 0
cube(25, 2) = 0
cube(25, 3) = 0
cube(25, 4) = 26
cube(25, 5) = 27
cube(25, 6) = 28
cube(26, 1) = .2
cube(26, 2) = 0
cube(26, 3) = 0
cube(27, 1) = 0
cube(27, 2) = .2
cube(27, 3) = 0
cube(28, 1) = 0
cube(28, 2) = 0
cube(28, 3) = .2








trick = 1
FOR lop = 1 TO cubesize
        cube(lop, 1) = cube(lop, 1) - 1.5
        cube(lop, 2) = cube(lop, 2) + 3.5
        cube(lop, 3) = cube(lop, 3) - .5
NEXT



centrex = ((cube(7, 1) - cube(1, 1)) / 2) + cube(1, 1)
centrey = ((cube(7, 2) - cube(1, 2)) / 2) + cube(1, 2)
centrez = ((cube(7, 3) - cube(1, 3)) / 2) + cube(1, 3)

FOR lop = 1 TO cubesize
        tempy = c2# * (cube(lop, 2) - centrey) + s2# * (cube(lop, 3) - centrez)
        tempz = -s2# * (cube(lop, 2) - centrey) + c2# * (cube(lop, 3) - centrez)
        tempy = tempy + centrey
        tempz = tempz + centrez
        cube(lop, 2) = tempy
        cube(lop, 3) = tempz
NEXT




WHILE NOT (button$ = "q")
        button$ = INKEY$
        FOR lop = 1 TO 1000: NEXT lop
        CLS
        trick = trick + 1
        FOR lop = 1 TO cubesize
                count = 1
                x = screenposx(cube(lop, 1), cube(lop, 2), cube(lop, 3))
                y = screenposy(cube(lop, 1), cube(lop, 2), cube(lop, 3))
                IF NOT (y < -.5 OR x < -.5 OR x > .5 OR y > .5) THEN
                        WHILE (NOT (cube(lop, count + 3) = -1)) OR (count + 3 >= cubesize)
                                point2 = cube(lop, count + 3)
                                x1 = screenposx(cube(point2, 1), cube(point2, 2), cube(point2, 3))
                                y1 = screenposy(cube(point2, 1), cube(point2, 2), cube(point2, 3))
                                IF NOT (x1 < -.5 OR y1 < -.5 OR x1 > .5 OR y1 > .5) THEN
                                        LINE (INT((x + .5) * 320), INT((y + .5) * 200))-(INT((x1 + .5) * 320), INT((y1 + .5) * 200))
                                END IF

                                count = count + 1

                        WEND
                END IF
        NEXT


        centrex = ((cube(7, 1) - cube(1, 1)) / 2) + cube(1, 1)
        centrey = ((cube(7, 2) - cube(1, 2)) / 2) + cube(1, 2)
        centrez = ((cube(7, 3) - cube(1, 3)) / 2) + cube(1, 3)
        IF button$ = "z" THEN dir = -1
        IF button$ = "x" THEN dir = 1
        IF button$ = " " THEN dir = 0
        IF NOT (dir = 0) THEN
                FOR lop = 1 TO cubesize
                        tempx = c# * (cube(lop, 1) - centrex) + dir * s# * (cube(lop, 2) - centrey)
                        tempy = -dir * s# * (cube(lop, 1) - centrex) + c# * (cube(lop, 2) - centrey)
                        tempx = tempx + centrex
                        tempy = tempy + centrey
                        cube(lop, 1) = tempx
                        cube(lop, 2) = tempy
                NEXT
        END IF
        IF button$ = "k" THEN
                FOR lop = 1 TO cubesize
                        cube(lop, 2) = cube(lop, 2) + .1
                NEXT
        END IF

        IF button$ = "m" THEN
                FOR lop = 1 TO cubesize
                        cube(lop, 2) = cube(lop, 2) - .1
                NEXT
        END IF

            


WEND

FUNCTION screenposx (x, y, z)
        IF y = 0 THEN
                screenposx = -1
        ELSE
                screenposx = x / y
        END IF
END FUNCTION

FUNCTION screenposy (x, y, z)
        IF y = 0 THEN
                screenposy = -1
        ELSE
                screenposy = z / y
        END IF

END FUNCTION

