DECLARE SUB disp ()


'declare 3d data variables

TYPE pts
        x AS SINGLE
        y AS SINGLE
        z AS SINGLE
        beg AS INTEGER
        fin AS INTEGER
END TYPE

TYPE obj
        called AS STRING * 9
        posx AS SINGLE
        posy AS SINGLE
        posz AS SINGLE
        col AS INTEGER
        beg AS INTEGER
        fin AS INTEGER
END TYPE

CONST pi = 3.141592654#

SCREEN 7

DIM object(1 TO 100) AS obj
DIM pt(1 TO 100) AS pts
DIM link(500)

FOR lop = 1 TO 500
        link(lop) = -1
NEXT lop
FOR lop = 1 TO 100
        object(lop).called = "*********"
NEXT

'define space ship1
object(1).called = "ship1"
object(1).col = 7
object(1).posx = -5
object(1).posy = 0
object(1).posz = 0
object(1).beg = 1
object(1).fin = 13

pt(1).x = -1
pt(1).y = -1
pt(1).z = 0
pt(1).beg = 1
pt(1).fin = 2

pt(2).x = 1
pt(2).y = -1
pt(2).z = 0
pt(2).beg = 3
pt(2).fin = 5

pt(3).x = -1
pt(3).y = 1
pt(3).z = 0
pt(3).beg = 6
pt(3).fin = 7

pt(4).x = 1
pt(4).y = 1
pt(4).z = 0
pt(4).beg = 8
pt(4).fin = 9
              
pt(5).x = -1
pt(5).y = -1
pt(5).z = -.5
pt(5).beg = 10
pt(5).fin = 11

pt(6).x = 1
pt(6).y = -1
pt(6).z = -.5
pt(6).beg = 12
pt(6).fin = 13

pt(7).x = -1
pt(7).y = 1
pt(7).z = -.5
pt(7).beg = 14
pt(7).fin = 14

pt(8).x = 1
pt(8).y = 1
pt(8).z = -.6
pt(8).beg = 15
pt(8).fin = 15

pt(9).x = 2
pt(9).y = 0
pt(9).z = 0
pt(9).beg = -1
pt(9).fin = -1

pt(10).x = -1
pt(10).y = -3
pt(10).z = 0
pt(10).beg = 16
pt(10).fin = 17

pt(11).x = -1
pt(11).y = 3
pt(11).z = 0
pt(11).beg = 18
pt(11).fin = 18

pt(12).x = 0
pt(12).y = -1
pt(12).z = 0
pt(12).beg = -1
pt(12).fin = -1

pt(13).x = 0
pt(13).y = 1
pt(13).z = 0
pt(13).beg = -1
pt(13).fin = -1


link(1) = 2
link(2) = 5
link(3) = 6
link(4) = 4
link(5) = 9
link(6) = 7
link(7) = 4
link(8) = 8
link(9) = 9
link(10) = 7
link(11) = 6
link(12) = 8
link(13) = 9
link(14) = 8
link(15) = 9
link(16) = 11
link(17) = 12
link(18) = 13

object(2).called = "ground"
object(2).col = 2
object(2).posx = 100
object(2).posy = 0
object(2).posz = 5
object(2).beg = 14
object(2).fin = 17

pt(14).x = -10
pt(14).y = -10
pt(14).z = 0
pt(14).beg = 19
pt(14).fin = 20

pt(15).x = -10
pt(15).y = 10
pt(15).z = 0
pt(15).beg = 21
pt(15).fin = 21

pt(16).x = 10
pt(16).y = 10
pt(16).z = 0
pt(16).beg = 22
pt(16).fin = 22

pt(17).x = 10
pt(17).y = -10
pt(17).z = 0
pt(17).beg = -1
pt(17).fin = -1

link(19) = 15
link(20) = 17
link(21) = 16
link(22) = 17



'define initial camera position and direction
cameraposx = -10
cameraposy = -1
cameraposz = -2
camerarotx = 0
cameraroty = 0
camerarotz = -pi / 2










'main program loop      ************************
'                       ************************   

move = 2

a = 1
WHILE NOT (INKEY$ = "q")

        CLS
        GOSUB disp
        object(1).posx = object(1).posx + move
        cameraposx = cameraposx + 1.7
        cameraposz = cameraposz - 1
        cameraposy = cameraposy - .2
        camerarotz = camerarotz + a * (pi / 100)
        a = a * .93
        camerarotx = camerarotx + pi / 100
WEND





END



'***********************************************
'***********************************************



'begining of "procedure" part of program.
'Done as such to maintain global variables.




 



disp:
'draws all the objects!
        FOR currentobj = 1 TO 100
                IF NOT (object(currentobj).called = "*********") THEN
                        FOR part = object(currentobj).beg TO object(currentobj).fin
                               
                                'translate object to virtual object
                                x1 = pt(part).x - cameraposx + object(currentobj).posx
                                y1 = pt(part).y - cameraposy + object(currentobj).posy
                                z1 = pt(part).z - cameraposz + object(currentobj).posz

                                'rotate about z axis
                                tempx = x1 * COS(camerarotz) + y1 * SIN(camerarotz)
                                tempy = -x1 * SIN(camerarotz) + y1 * COS(camerarotz)
                                x1 = tempx
                                y1 = tempy
                                       
                                'rotate about x axis

                                tempy = y1 * COS(camerarotx) + z1 * SIN(camerarotx)
                                tempz = -y1 * SIN(camerarotx) + z1 * COS(camerarotx)
                                y1 = tempy
                                z1 = tempz

                                'rotate about y axis
                                tempx = x1 * COS(cameraroty) + z1 * SIN(cameraroty)
                                tempz = -x1 * SIN(cameraroty) + z1 * COS(cameraroty)
                                x1 = tempx
                                z1 = tempz
                                IF NOT (pt(part).beg = -1) THEN
                                        FOR linknumber = pt(part).beg TO pt(part).fin
                                                'translate object to virtual object
                                               
                                                x2 = pt(link(linknumber)).x - cameraposx + object(currentobj).posx
                                                y2 = pt(link(linknumber)).y - cameraposy + object(currentobj).posy
                                                z2 = pt(link(linknumber)).z - cameraposz + object(currentobj).posz

                                                'rotate about z axis
                                                tempx = x2 * COS(camerarotz) + y2 * SIN(camerarotz)
                                                tempy = -x2 * SIN(camerarotz) + y2 * COS(camerarotz)
                                                x2 = tempx
                                                y2 = tempy
                                                      
                                                'rotate about x axis

                                                tempy = y2 * COS(camerarotx) + z2 * SIN(camerarotx)
                                                tempz = -y2 * SIN(camerarotx) + z2 * COS(camerarotx)
                                                y2 = tempy
                                                z2 = tempz

                                                'rotate about y axis
                                                tempx = x2 * COS(cameraroty) + z2 * SIN(cameraroty)
                                                tempz = -x2 * SIN(cameraroty) + z2 * COS(cameraroty)
                                                x2 = tempx
                                                z2 = tempz
                                                IF NOT (y1 = 0 OR y2 = 0) THEN
                                                        screenx1 = x1 / y1
                                                        screenx2 = x2 / y2
                                                        screeny1 = z1 / y1
                                                        screeny2 = z2 / y2
                                                ELSE
                                                        screenx1 = -1
                                                        screenx2 = -1
                                                        screeny1 = -1
                                                        screeny2 = -1
                                                       
                                                END IF
                                                IF y1 > 1 AND y2 > 1 THEN
                                                        LINE ((screenx1 + .5) * 320, (screeny1 + .5) * 200)-((screenx2 + .5) * 320, (screeny2 + .5) * 200), object(currentobj).col
                                                END IF
                                        NEXT linknumber
                                END IF
                        NEXT part
                END IF
        NEXT currentobj
       
RETURN

