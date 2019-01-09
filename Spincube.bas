DECLARE FUNCTION screenposx! (x!, y!, z!)
DECLARE FUNCTION screenposy! (x!, y!, z!)
CONST pi = 3.141592654#
SCREEN 1
CLS
angle = 5
c# = COS(angle / 180 * pi)
s# = SIN(angle / 180 * pi)

CLS
DIM cube(8, 11)
FOR lop = 1 TO 8
        FOR lop2 = 1 TO 5
                cube(lop, lop2 + 3) = -1
        NEXT
NEXT
cube(1, 1) = 1
cube(1, 2) = 1
cube(1, 3) = 1
cube(1, 4) = 2
cube(1, 5) = 3
cube(1, 6) = 5
cube(2, 1) = 2
cube(2, 2) = 1
cube(2, 3) = 1
cube(2, 4) = 6
cube(2, 5) = 4
cube(3, 1) = 1
cube(3, 2) = 1
cube(3, 3) = 0
cube(3, 4) = 4
cube(3, 5) = 7
cube(4, 1) = 2
cube(4, 2) = 1
cube(4, 3) = 0
cube(4, 4) = 8
cube(5, 1) = 1
cube(5, 2) = 0
cube(5, 3) = 1
cube(5, 4) = 7
cube(5, 5) = 6
cube(6, 1) = 2
cube(6, 2) = 0
cube(6, 3) = 1
cube(6, 4) = 8
cube(7, 1) = 1
cube(7, 2) = 0
cube(7, 3) = 0
cube(7, 4) = 8
cube(8, 1) = 2
cube(8, 2) = 0
FOR lop = 1 TO 8

NEXT
trick = 1

WHILE INKEY$ = ""
        trick = trick + 1
        CLS
        FOR lop = 1 TO 8
                count = 1
                x = screenposx(cube(lop, 1), cube(lop, 2), cube(lop, 3))
                y = screenposy(cube(lop, 1), cube(lop, 2), cube(lop, 3))
                WHILE (NOT (cube(lop, count + 3) = -1)) OR (count + 3 >= 8)
                        point2 = cube(lop, count + 3)
                        x1 = screenposx(cube(point2, 1), cube(point2, 2), cube(point2, 3))
                        y1 = screenposy(cube(point2, 1), cube(point2, 2), cube(point2, 3))
                        LINE (INT((x - .5) * -320), INT((y - .5) * -200))-(INT((x1 - .5) * -320), INT((y1 - .5) * -200))
                        count = count + 1

                WEND
        NEXT


        centrex = ((cube(7, 1) - cube(2, 1)) / 2) + cube(2, 1)
        centrey = ((cube(7, 2) - cube(2, 2)) / 2) + cube(2, 2)
        centrez = ((cube(7, 3) - cube(2, 3)) / 2) + cube(2, 3)
        IF trick < 60 THEN
                FOR lop = 1 TO 8
                        tempx = c# * (cube(lop, 1) - centrex) + s# * (cube(lop, 2) - centrey)
                        tempy = -s# * (cube(lop, 1) - centrex) + c# * (cube(lop, 2) - centrey)
                        tempx = tempx + centrex
                        tempy = tempy + centrey
                        cube(lop, 1) = tempx
                        cube(lop, 2) = tempy
                NEXT
        END IF
        IF trick >= 60 AND trick < 120 THEN
                FOR lop = 1 TO 8
                        tempy = c# * (cube(lop, 2) - centrey) + s# * (cube(lop, 3) - centrez)
                        tempz = -s# * (cube(lop, 2) - centrey) + c# * (cube(lop, 3) - centrez)
                        tempy = tempy + centrey
                        tempz = tempz + centrez
                        cube(lop, 2) = tempy
                        cube(lop, 3) = tempz
                NEXT
        END IF

        IF trick < 30 THEN
                FOR lop = 1 TO 8
                        cube(lop, 1) = cube(lop, 1) + .05
                NEXT
        END IF
        IF trick >= 30 AND trick < 60 THEN
                FOR lop = 1 TO 8
                        cube(lop, 2) = cube(lop, 2) + .3
                NEXT
        END IF
        IF trick >= 60 AND trick < 90 THEN
                FOR lop = 1 TO 8
                        cube(lop, 1) = cube(lop, 1) - .05
                NEXT
        END IF
        IF trick >= 90 AND trick < 120 THEN
                FOR lop = 1 TO 8
                        cube(lop, 2) = cube(lop, 2) - .3
                NEXT
        END IF

        IF trick = 120 THEN
                trick = 0
        END IF

        FOR lop = 1 TO 200: NEXT



WEND

FUNCTION screenposx (x, y, z)
        screenposx = x / y
END FUNCTION

FUNCTION screenposy (x, y, z)
        screenposy = z / y
END FUNCTION

