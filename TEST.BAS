DECLARE FUNCTION bob! (a!)

CLS

DIM a(1 TO 10)
FOR lop = 1 TO 10
        a(lop) = lop
        PRINT a(lop)
NEXT


bob (1)

FUNCTION bob (a)

PRINT "function value"; a

    OPEN "TEST.DAT" FOR OUTPUT AS #1
    PRINT #1, USING "##.###  "; 12.12345
    CLOSE
    OPEN "TEST.DAT" FOR INPUT AS #1
    INPUT #1, a$
    PRINT a$
    PRINT "This is a line"; 1
    PRINT "This is a line",
    PRINT 2


END FUNCTION


