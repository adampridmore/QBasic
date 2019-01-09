DECLARE FUNCTION screenposx! (x!, y!, z!)
DECLARE FUNCTION screenposy! (x!, y!, z!)
SCREEN 1

FUNCTION screenposx (x, y, z)
        screenposx = x / y
END FUNCTION

FUNCTION screenposy (x, y, z)
        screenposy = z / y
END FUNCTION

