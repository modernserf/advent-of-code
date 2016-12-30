: reload! s" day-2.fs" included ;

( import: getc lf? nextc slurp )
s" file.fs" included

: load-file s" 2.txt" slurp drop drop ;

: clamp     0 max 2 min ;

: center    1 1 ;
: key-at    3 * + 1 + ;
: up        -1 + clamp ;
: down      1 + clamp ;
: left      >r -1 + clamp r> ;
: right     >r 1 + clamp r> ;

: move      ( x y dirChar -- x2 y2 ) case
            [char] U of up endof
            [char] D of down endof
            [char] L of left endof
            [char] R of right endof
            endcase ;

: char-line getc dup lf? if drop 0 else -1 then nextc ;
: line      begin char-line while move repeat 2dup key-at . ;
: run       load-file center begin chars? while line repeat ;

page run
