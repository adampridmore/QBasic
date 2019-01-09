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
        rotx AS SINGLE
        col AS INTEGER
        beg AS INTEGER
        fin AS INTEGER
END TYPE

CONST pi = 3.141592654#
scanrange = 100





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
object(1).posx = 0
object(1).posy = 50
object(1).posz = 0
object(1).rotx = 0
object(1).beg = 1
object(1).fin = 13

pt(1).x = -10
pt(1).y = -10
pt(1).z = 0
pt(1).beg = 1
pt(1).fin = 2

pt(2).x = 10
pt(2).y = -10
pt(2).z = 0
pt(2).beg = 3
pt(2).fin = 5

pt(3).x = -10
pt(3).y = 10
pt(3).z = 0
pt(3).beg = 6
pt(3).fin = 7

pt(4).x = 10
pt(4).y = 10
pt(4).z = 0
pt(4).beg = 8
pt(4).fin = 9
              
pt(5).x = -10
pt(5).y = -10
pt(5).z = -8
pt(5).beg = 10
pt(5).fin = 11

pt(6).x = 10
pt(6).y = -10
pt(6).z = -8
pt(6).beg = 12
pt(6).fin = 13

pt(7).x = -10
pt(7).y = 10
pt(7).z = -8
pt(7).beg = 14
pt(7).fin = 14

pt(8).x = 10
pt(8).y = 10
pt(8).z = -8
pt(8).beg = 15
pt(8).fin = 15

pt(9).x = 20
pt(9).y = 0
pt(9).z = -3
pt(9).beg = -1
pt(9).fin = -1

pt(10).x = -10
pt(10).y = -30
pt(10).z = 0
pt(10).beg = 16
pt(10).fin = 17

pt(11).x = -10
pt(11).y = 30
pt(11).z = 0
pt(11).beg = 18
pt(11).fin = 18

pt(12).x = 0
pt(12).y = -10
pt(12).z = 0
pt(12).beg = -1
pt(12).fin = -1

pt(13).x = 0
pt(13).y = 10
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



'define initial camera position and direction
cameraposx = -10
cameraposy = 0
cameraposz = -5
camerarotx = 0
cameraroty = 0
camerarotz = 0










'main program loop      ************************
'                       ************************



'CLS
'LOCATE 10
'PRINT "         Welcome to mech war"
'PRINT
'PRINT
'PRINT "        Death will find us all"
'GOSUB pause
'CLS

quit = 0
gamecount = 0
scanon = 0
WHILE quit = 0
        gamecount = gamecount + 1
        CLS
        PRINT "(H-help)(S-scan)(F-fire)"
        PRINT cameraposx
        PRINT cameraposy
        PRINT cameraposz
      

        IF scanon = 0 THEN GOSUB disp:
        IF scanon = 1 THEN GOSUB scan:
        
        button$ = INKEY$
        

        IF button$ = "8" OR button$ = "6" OR button$ = "4" OR button$ = "2" OR button$ = "3" OR button$ = "9" THEN GOSUB movement
        IF button$ = "s" AND scanon = 0 THEN
                scanon = 1
        ELSEIF button$ = "s" AND scanon = 1 THEN
                scanon = 0
        END IF


        IF button$ = "-" AND scanrange < 1000 THEN scanrange = scanrange * 1.1
        IF button$ = "+" AND scanrange > 10 THEN scanrange = scanrange * 1.1 ^ -1
        
        IF button$ = "f" THEN GOSUB fire
        IF button$ = "q" THEN quit = 1


WEND

END



'***********************************************
'***********************************************



'begining of "procedure" part of program.
'Done as such to maintain global variables.




movement:
        
        IF button$ = "4" THEN camerarotz = camerarotz + pi / 12
        IF button$ = "6" THEN camerarotz = camerarotz - pi / 12

        IF button$ = "8" THEN
                
                cameraposx = cameraposx + 5 * SIN(-camerarotz)
                cameraposy = cameraposy + 5 * COS(camerarotz)
        END IF
     
        IF button$ = "2" THEN
                cameraposx = cameraposx - 5 * SIN(-camerarotz)
                cameraposy = cameraposy - 5 * COS(camerarotz)
        END IF
        
        IF button$ = "3" THEN camerarotx = camerarotx + pi / 12
        IF button$ = "9" THEN camerarotx = camerarotx - pi / 12





RETURN


scan:



LOCATE (23)
PRINT "dist="; INT(scanrange); "m"

LINE (160, 100)-(160 + 20 * (1100 - scanrange) / 1000 * -SIN(-camerarotz - pi), 100 + 20 * (1100 - scanrange) / 1000 * COS(camerarotz + pi)), 15
LINE (160, 100)-(160 + 10 * (1100 - scanrange) / 1000 * -SIN(-camerarotz - 3 * pi / 4), 100 + 10 * (1100 - scanrange) / 1000 * COS(camerarotz + 3 * pi / 4)), 15
LINE (160, 100)-(160 + 10 * (1100 - scanrange) / 1000 * -SIN(-camerarotz + 3 * pi / 4), 100 + 10 * (1100 - scanrange) / 1000 * COS(camerarotz - 3 * pi / 4)), 15

FOR currentobj = 1 TO 100
        IF NOT (object(currentobj).called = "*********") THEN
                FOR part = object(currentobj).beg TO object(currentobj).fin
                        IF NOT (pt(part).beg = -1) THEN
                                FOR linknumber = pt(part).beg TO pt(part).fin
                                        x1 = pt(part).x - cameraposx + object(currentobj).posx
                                        y1 = pt(part).y - cameraposy + object(currentobj).posy
                                       
                                        x2 = pt(link(linknumber)).x - cameraposx + object(currentobj).posx
                                        y2 = pt(link(linknumber)).y - cameraposy + object(currentobj).posy
                                        x1 = -x1 / scanrange
                                        x2 = -x2 / scanrange
                                        y1 = y1 / scanrange
                                        y2 = y2 / scanrange
                                        LINE ((x1 + .5) * 320, (y1 + .5) * 200)-((x2 + .5) * 320, (y2 + .5) * 200), object(currentobj).col


                                NEXT
                        END IF
                NEXT
        END IF
NEXT










RETURN

fire:
IF scanon = 0 THEN
        FOR fireloop = 1 TO 100
                LINE (100 + RND * 20, 200 + RND * 20)-(160 + RND * 2, 100 + RND * 2), INT(fireloop / 20)
                LINE (110, 210)-(161, 101), 12
        NEXT
END IF

IF scanon = 1 THEN
        FOR fireloop = 1 TO 100
                LINE (160, 100)-((1000 - scanrange) / 100 * RND + 160 + 200 * -SIN(-camerarotz), (1000 - scanrange) / 100 * RND + 100 + 200 * COS(camerarotz)), INT(fireloop / 20)
        NEXT
END IF



RETURN





pause:
        WHILE INKEY$ = "": WEND
RETURN

 



disp:
'draws all the objects!
        

        horizonheight = -200 * SIN(camerarotx) + 100
        LINE (0, horizonheight)-(320, horizonheight), 2

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
                                                                            f
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

