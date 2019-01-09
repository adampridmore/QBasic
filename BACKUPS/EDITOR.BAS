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
        beg AS INTEGER
        fin AS INTEGER
END TYPE



'declare common variables
COMMON pt AS pts
COMMON pts
COMMON obj
COMMON object AS obj
COMMON cameraposx
COMMON cameraposy
COMMON cameraposz
COMMON camerarotx
COMMON cameraroty
COMMON camerarotz

CONST pi = 3.141593

count = 0


SCREEN 1


DIM object(1 TO 10) AS obj
DIM pt(1 TO 100) AS pts
DIM link(500)
FOR lop = 1 TO 500
        link(lop) = -1
NEXT lop






'define initial camera position and direction
cameraposx = 0
cameraposy = -30
cameraposz = -10
camerarotx = pi / 30
cameraroty = 0
camerarotz = 0


'define initial "ground"


object(1).called = "ground"
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
     
GOSUB disp

WHILE INKEY$ = ""
WEND

END




'******************************************
'******************************************









'begining of "procedure" part of program.
'Done as such to maintain global variables.





disp:
'draws all the objects!
    
        FOR currentobj = 1 TO 1
                FOR part = object(currentobj).beg TO object(currentobj).fin
                       
                        'translate object to virtual object
                        x1 = pt(part).x - cameraposx
                        y1 = pt(part).y - cameraposy
                        z1 = pt(part).z - cameraposz

                        'rotate about z axis
                        tempx = x1 * COS(camerarotz) + y1 * SIN(camerarotz)
                        tempy = -x1 * SIN(camerarotz) + y1 * COS(cameraroty)
                        x1 = tempx
                        y1 = tempy
                               
                        'rotate about x axis

                        tempy = y1 * COS(camerarotx) + z1 * SIN(camerarotx)
                        tempz = -z1 * SIN(camerarotx) + z1 * COS(camerarotx)
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
                                       
                                        x2 = pt(link(linknumber)).x - cameraposx
                                        y2 = pt(link(linknumber)).y - cameraposy
                                        z2 = pt(link(linknumber)).z - cameraposz

                                        'rotate about z axis
                                        tempx = x2 * COS(camerarotz) + y2 * SIN(camerarotz)
                                        tempy = -x2 * SIN(camerarotz) + y2 * COS(cameraroty)
                                        x2 = tempx
                                        y2 = tempy
                                              
                                        'rotate about x axis

                                        tempy = y2 * COS(camerarotx) + z2 * SIN(camerarotx)
                                        tempz = -z2 * SIN(camerarotx) + z2 * COS(camerarotx)
                                        y2 = tempy
                                        z2 = tempz

                                        'rotate about y axis
                                        tempx = x2 * COS(cameraroty) + z2 * SIN(cameraroty)
                                        tempz = -x2 * SIN(cameraroty) + z2 * COS(cameraroty)
                                        x2 = tempx
                                        z2 = tempz
                                       
                                        screenx1 = x1 / y1
                                        screenx2 = x2 / y2
                                        screeny1 = z1 / y1
                                        screeny2 = z2 / y2
                                        'PRINT "x1 "; screenx1, " y1 ", screeny1, " x2 ", screenx2, " y2 ", screeny2
                                        'PRINT
                                        LINE ((screenx1 + .5) * 320, (screeny1 + .5) * 200)-((screenx2 + .5) * 320, (screeny2 + .5) * 200)
                                        count = count + 1
                                NEXT linknumber
                        END IF
                NEXT part
        NEXT currentobj
       
RETURN

