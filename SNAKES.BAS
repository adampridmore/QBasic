CLS
COLOR 15, 0
PRINT
PRINT
PRINT
PRINT "         welcome to poor man snakes!"
PRINT
PRINT "       So you want to know the keys..."
PRINT
PRINT "               left       z"
PRINT "               right      x"
PRINT "               up         k"
PRINT "               down       m"
PRINT
PRINT "Oh and if you get fed up `q` quits."
WHILE INKEY$ = ""
WEND


again = 1
WHILE again = 1
        CLS
        COLOR 15, 0
        'initial variables!
        speed = 250

        posy = 5
        posx = 10
        dirc = 1
        dead = 0
        score = 1

        FOR lop = 2 TO 62
                LOCATE 2, lop
                PRINT "="
                LOCATE 22, lop
                PRINT "="
        NEXT
        FOR lop = 2 TO 22
                LOCATE lop, 2
                PRINT "|"
                LOCATE lop, 62
                PRINT "|"
        NEXT



        'set up tail variable
        DIM tail(5000, 2)
        FOR lop = 1 TO 5000
                tail(lop, 0) = -1
                tail(lop, 1) = -1
        NEXT
        FOR lop = 1 TO 5
                tail(lop, 0) = lop
                tail(lop, 1) = 5
        NEXT
        tstart = 5
        tend = 1
        tlength = 5

        LOCATE 1, 1
        PRINT "ready??"
        WHILE INKEY$ = ""
        WEND
        LOCATE 1, 1
        PRINT "       "

        count = 1


        DO UNTIL i$ = "q" OR dead = 1
                i$ = INKEY$
               
                IF i$ = "z" AND NOT (dirc = 1) THEN dirc = 0
                IF i$ = "x" AND NOT (dirc = 0) THEN dirc = 1
                IF i$ = "k" AND NOT (dirc = 3) THEN dirc = 2
                IF i$ = "m" AND NOT (dirc = 2) THEN dirc = 3
               
                IF dirc = 0 AND posx > 1 THEN posx = posx - 1
                IF dirc = 1 AND posx < 60 THEN posx = posx + 1
                IF dirc = 2 AND posy > 1 THEN posy = posy - 1
                IF dirc = 3 AND posy < 20 THEN posy = posy + 1
                IF posx = 0 OR posx = 60 OR posy = 0 OR posy = 20 THEN
                        dead = 1
                ELSE
                        LOCATE posy + 2, posx + 2
                        PRINT "*"
                END IF
               

                'check for tail touching tail
                'IF start > tend THEN
                        FOR lop = tend TO tstart
                                IF tail(lop, 0) = posx AND tail(lop, 1) = posy THEN
                                dead = 1
                                END IF
                        NEXT
                'END IF
                IF tstart < tend THEN
                        FOR lop = 1 TO tstart
                                IF tail(lop, 0) = posx AND tail(lop, 1) = posy THEN
                                dead = 1
                                END IF
                        NEXT
                        FOR lop = tend TO 5000
                                IF tail(lop, 0) = posx AND tail(lop, 1) = posy THEN
                                dead = 1
                                END IF
                        NEXT
                END IF
               
                'change tail variable
                tail(tstart, 0) = posx
                tail(tstart, 1) = posy
                IF tstart = 5000 THEN
                        tstart = 1
                ELSE
                        tstart = tstart + 1
                END IF
                count = count + 1
                IF count \ 5 = 1 AND tlength < 998 THEN

                        tlength = tlength + 1
                        count = 0
                ELSE
                        LOCATE tail(tend, 1) + 2, tail(tend, 0) + 2
                        PRINT " "
                        tail(tend, 0) = -1
                        tail(tend, 1) = -1
                        IF tend = 5000 THEN
                                tend = 1
                        ELSE
                                tend = tend + 1
                        END IF
                END IF


                'delay!
                FOR lop = 1 TO speed
                NEXT
                IF speed > 1 THEN speed = speed - 1
                IF dirc = 2 OR dirc = 3 THEN
                        FOR lop = 1 TO 200
                        NEXT
                END IF

                


                score = score + 1

                LOCATE 1, 1
                PRINT "score :"; score
        LOOP

        LOCATE 23, 20
        PRINT ">>>>>>> you died! <<<<<<<"
        WHILE INKEY$ = ""
        WEND
        CLS
        IF NOT (i$ = "q") THEN INPUT "Again? (y/n)", an$
        IF i$ = "q" OR an$ = "n" THEN again = 0
                
WEND



