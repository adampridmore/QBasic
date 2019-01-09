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

FOR lop = 1 TO 100
        pt(lop).beg = -2
        pt(lop).fin = -2
NEXT lop


FOR lop = 1 TO 500
        link(lop) = -1
NEXT lop

FOR lop = 1 TO 100
        object(lop).called = "*********"
NEXT

'define initial camera position and direction
cameraposx = 0
cameraposy = -40
cameraposz = -10
camerarotx = 0
cameraroty = 0
camerarotz = 0


'define default highlight colour"
col = 9
speed = 4


'define initial "ground"

object(1).called = "ground"
object(1).posx = 0
object(1).posy = 0
object(1).posz = 0
object(1).col = 14
object(1).beg = 1
object(1).fin = 4

pt(1).x = -10
pt(1).y = -10
pt(1).z = 0
pt(1).beg = 1
pt(1).fin = 2

pt(2).x = 10
pt(2).y = -10
pt(2).z = 0
pt(2).beg = 3
pt(2).fin = 3

pt(3).x = -10
pt(3).y = 10
pt(3).z = 0
pt(3).beg = 4
pt(3).fin = 4

pt(4).x = 10
pt(4).y = 10
pt(4).z = 0
pt(4).beg = -1
pt(4).fin = -1

link(1) = 2
link(2) = 3
link(3) = 4
link(4) = 4


object(2).called = "tree"
object(2).posx = 8
object(2).posy = 8
object(2).posz = 0

object(2).col = 2
object(2).beg = 5
object(2).fin = 10

pt(5).x = 0
pt(5).y = 0
pt(5).z = 0
pt(5).beg = 5
pt(5).fin = 5

pt(6).x = 0
pt(6).y = 0
pt(6).z = -2
pt(6).beg = 6
pt(6).fin = 9

pt(7).x = 1
pt(7).y = 1
pt(7).z = -.8
pt(7).beg = -1
pt(7).fin = -1

pt(8).x = -1
pt(8).y = 1
pt(8).z = -.8
pt(8).beg = -1
pt(8).fin = -1

pt(9).x = 1
pt(9).y = -1
pt(9).z = -.8
pt(9).beg = -1
pt(9).fin = -1
          
pt(10).x = -1
pt(10).y = -1
pt(10).z = -.8
pt(10).beg = -1
pt(10).fin = -1

link(5) = 6
link(6) = 7
link(7) = 8
link(8) = 9
link(9) = 10

object(3).called = "house"
object(3).col = 7
object(3).beg = 11
object(3).fin = 20
object(3).posx = 0
object(3).posy = 0
object(3).posz = 0

pt(11).x = 1
pt(11).y = -1
pt(11).z = 0
pt(11).beg = 10
pt(11).fin = 12

pt(12).x = 1
pt(12).y = 1
pt(12).z = 0
pt(12).beg = 13
pt(12).fin = 14

pt(13).x = -1
pt(13).y = 1
pt(13).z = 0
pt(13).beg = 15
pt(13).fin = 16

pt(14).x = -1
pt(14).y = -1
pt(14).z = 0
pt(14).beg = 17
pt(14).fin = 17

pt(15).x = 1
pt(15).y = -1
pt(15).z = -2
pt(15).beg = 18
pt(15).fin = 20

pt(16).x = 1
pt(16).y = 1
pt(16).z = -2
pt(16).beg = 21
pt(16).fin = 22

pt(17).x = -1
pt(17).y = 1
pt(17).z = -2
pt(17).beg = 23
pt(17).fin = 24

pt(18).x = -1
pt(18).y = -1
pt(18).z = -2
pt(18).beg = 25
pt(18).fin = 25

pt(19).x = -1
pt(19).y = 0
pt(19).z = -3
pt(19).beg = 26
pt(19).fin = 26

pt(20).x = 1
pt(20).y = 0
pt(20).z = -3
pt(20).beg = -1
pt(20).fin = -1

link(10) = 12
link(11) = 14
link(12) = 15
link(13) = 13
link(14) = 16
link(15) = 14
link(16) = 17
link(17) = 18
link(18) = 16
link(19) = 18
link(20) = 20
link(21) = 17
link(22) = 20
link(23) = 18
link(24) = 19
link(25) = 19
link(26) = 20



FOR lop = 1 TO 10
        object(lop + 3).called = "tree"
        object(lop + 3).beg = object(2).beg
        object(lop + 3).fin = object(2).fin
        object(lop + 3).col = object(2).col
        object(lop + 3).posx = RND * 20 - 10
        object(lop + 3).posy = RND * 20 - 10
        object(lop + 3).posz = 0
NEXT







'main program loop      ************************
'                       ************************     

count = 1
WHILE NOT (button$ = "q")
        CLS
        COLOR 2
        horizonheight = -200 * SIN(camerarotx) + 100
        PRINT cameraroty
        twist = 100 * TAN(cameraroty)
        LINE (0, horizonheight + twist)-(320, horizonheight - twist)
        COLOR 7
        t = TIMER
        GOSUB disp
        t = TIMER - t
        LOCATE 1, 1
        PRINT "(h-help)(t-disp obj)(y-cre obj)(q-quit)"
        PRINT "(m-mv obj)(s - h l)(c - speed)"
        PRINT "(d-man)"
        PRINT t
        button$ = ""
        WHILE button$ = ""
                button$ = INKEY$
        WEND

        GOSUB movement

        IF button$ = "t" THEN GOSUB listobjects
        IF button$ = "y" THEN GOSUB createobject
        IF button$ = "h" THEN GOSUB help
        IF button$ = "m" THEN GOSUB moveobject
        IF button$ = "s" THEN GOSUB highlight
        IF button$ = "c" THEN GOSUB speedchange
        IF button$ = "d" THEN GOSUB manage
WEND







END
'***********************************************
'***********************************************






'begining of "procedure" part of program.
'Done as such to maintain global variables.


manage:

button$ = ""
WHILE NOT (button$ = "f")
        CLS


        PRINT "Object management"
        PRINT ""
        PRINT "(d-del object)"
        PRINT "(c-copy object)"
        PRINT "(f-finish)"
       
        IF button$ = "d" THEN
                DIM used(100)
                CLS
                PRINT "which object do you wish to delete?"
                INPUT a
                PRINT "object >"; object(lop).called;
                PRINT "is deleted"
               
                FOR lop = a TO 99
                        object(lop).called = object(lop + 1).called
                        object(lop).posz = object(lop + 1).posz
                        object(lop).posy = object(lop + 1).posy
                        object(lop).posx = object(lop + 1).posx
                        object(lop).col = object(lop + 1).col
                        object(lop).beg = object(lop + 1).beg
                        object(lop).fin = object(lop + 1).fin
                NEXT



                'clear used

                FOR lop = 1 TO 100
                        used(lop) = 0
                NEXT



                'set used to 1 for all pt's that are used
                lop = 1
                WHILE NOT (object(lop).called = "*********")
                        FOR lop2 = object(lop).beg TO object(lop).fin
                                used(lop2) = 1
                        NEXT
                        lop = lop + 1
                WEND
                
                FOR lop = 1 TO 100
                        IF used(lop) = 0 THEN
                                pt(lop).beg = -2
                                pt(lop).fin = -2
                        END IF
                NEXT lop
               

                ERASE used
        END IF

       
        IF button$ = "c" THEN
                PRINT "which object do you wish to copy?"
                INPUT a
                PRINT "object >"; object(a).called
                PRINT "will be placed into next object slot"
                lop = 1
                WHILE NOT (object(lop).called = "*********")
                        lop = lop + 1
                WEND

                nextobj = lop

                object(nextobj).called = object(a).called
                object(nextobj).posx = object(a).posx
                object(nextobj).posy = object(a).posy
                object(nextobj).posz = object(a).posz
                object(nextobj).col = object(a).col
                object(nextobj).beg = object(a).beg
                object(nextobj).fin = object(a).fin



        END IF
       
       
       
       
        button$ = ""
        WHILE button$ = ""
                button$ = INKEY$
        WEND
        
WEND

RETURN





speedchange:

CLS
PRINT "Current speed ="; speed
PRINT "What speed do you wish to move at?"
INPUT speed


RETURN




highlight:             
CLS
button$ = ""
highobject = 1

WHILE NOT (button$ = "f")
        oldcolour = object(highobject).col
        object(highobject).col = col
        CLS
        GOSUB disp
        PRINT "Highligh mode (yellow)(f - finished)"
        PRINT "(<> - different object)"
        PRINT "(c - change highlight colour)"
        PRINT "object >"; object(highobject).called
        PRINT "object number >"; highobject
        button$ = ""
        WHILE button$ = ""
                button$ = INKEY$
        WEND
        object(highobject).col = oldcolour
        IF button$ = "," AND NOT (highobject = 1) THEN highobject = highobject - 1
        IF button$ = "." AND NOT (object(highobject + 1).called = "*********") THEN highobject = highobject + 1
        IF button$ = "c" AND col < 15 THEN
                col = col + 1
        ELSE
                IF button$ = "c" AND col = 15 THEN col = 1
        END IF

WEND




RETURN






moveobject:
CLS
PRINT "Which object do you wish to move?"
INPUT "(object Number)"; a
PRINT object(a).called
PRINT "X - "; object(a).posx
PRINT "Y - "; object(a).posy
PRINT "Z - "; object(a).posz
PRINT "How much do you wish to move it?"
PRINT
INPUT "X > ", xmove
INPUT "Y > ", ymove
INPUT "Z > ", zmove
object(a).posx = object(a).posx + xmove
object(a).posy = object(a).posy + ymove
object(a).posz = object(a).posz + zmove
PRINT "new position"
PRINT
PRINT "X - "; object(a).posx
PRINT "Y - "; object(a).posy
PRINT "Z - "; object(a).posz
WHILE INKEY$ = "": WEND




RETURN



help:

CLS
SCREEN 2

PRINT
PRINT
PRINT "<                        -       turn left"
PRINT ">                        -       turn right"
PRINT "@                        -       look up"
PRINT "/                        -       look down"
PRINT "o                        -       roll left"
PRINT "p                        -       roll right"
PRINT
PRINT
PRINT "key pad keys;"
PRINT "         left arrow      -       slide left"
PRINT "         right arrow     -       slide right"
PRINT "         up arrow        -       move forwards(relative)"
PRINT "         down arrow      -       move backwars(relative)"
PRINT "         pg_up           -       move up(absolute)"
PRINT "         pd_dn           -       move down(absolute)"
PRINT
WHILE INKEY$ = "": WEND

SCREEN 7


RETURN








listobjects:

CLS
lop = 1
button$ = ""
WHILE NOT (button$ = "f")
        IF button$ = "," AND NOT (lop = 1) THEN lop = lop - 1
        IF button$ = "." AND NOT (object(lop + 1).called = "*********") THEN lop = lop + 1
        CLS
        PRINT "object "; lop
        PRINT ; "called "; object(lop).called
        PRINT "colour "; object(lop).col
        PRINT "position"
        PRINT "       X - "; object(lop).posx
        PRINT "       Y - "; object(lop).posy
        PRINT "       Z - "; object(lop).posz
        PRINT "No. of points "; object(lop).fin - object(lop).beg + 1
        button$ = ""
        LOCATE 15
        PRINT "(f - finish)(<> - different objects)"
              
        WHILE button$ = ""
                button$ = INKEY$
        WEND



WEND


RETURN





createobject:
       LOCATE 1, 1
       PRINT SPACE$(40)
       LOCATE 1, 1
       PRINT "you request has been noted...."
       WHILE INKEY$ = "": WEND

RETURN





movement:

IF button$ = "9" THEN cameraposz = cameraposz - speed
IF button$ = "3" THEN cameraposz = cameraposz + speed
IF button$ = "4" THEN
        cameraposx = cameraposx - speed * SIN(pi / 2 - camerarotz)
        cameraposy = cameraposy - speed * COS(pi / 2 - camerarotz)
END IF
IF button$ = "6" THEN
        cameraposx = cameraposx + speed * SIN(pi / 2 - camerarotz)
        cameraposy = cameraposy + speed * COS(pi / 2 - camerarotz)
END IF
IF button$ = "8" THEN
        cameraposx = cameraposx + speed * SIN(-camerarotz)
        cameraposy = cameraposy + speed * COS(camerarotz)
        cameraposz = cameraposz + speed * SIN(camerarotx)
END IF
             
IF button$ = "2" THEN
        cameraposx = cameraposx - speed * SIN(-camerarotz)
        cameraposy = cameraposy - speed * COS(camerarotz)
        cameraposz = cameraposz - speed * SIN(camerarotx)
END IF

IF button$ = "5" THEN
        cameraposx = 0
        cameraposy = -40
        cameraposz = -10
        camerarotx = 0
        cameraroty = 0
        camerarotz = 0

END IF
            
IF button$ = "," THEN camerarotz = camerarotz + pi / 12
IF button$ = "." THEN camerarotz = camerarotz - pi / 12
IF button$ = "/" THEN camerarotx = camerarotx + pi / 12
IF button$ = "'" THEN camerarotx = camerarotx - pi / 12
                       
IF button$ = "p" THEN cameraroty = cameraroty + pi / 12
IF button$ = "o" THEN cameraroty = cameraroty - pi / 12

RETURN




 



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

