: reload! s" day-1.fs" included ;

( import: getc lf? nextc slurp )
s" file.fs" included
: load-file     s" 1.txt" slurp drop drop ;

( util )
: -r>           postpone r> postpone negate  ; immediate
: range?        { val min max } val min >= val max <= and ;

( location )
variable x
variable y
: location      x @ y @ ;
: center!       0 x ! 0 y ! ;
: move!         ( dx dy ) y +! x +! ;
: total         location abs swap abs + ;

( direction )
: north         0 ;
: turn          + 4 mod ;
: left          -1 ;
: right         1 ;
: vec           ( direction distance ) >r case
                ( north ) 0 of  0  r>   endof
                ( east )  1 of  r> 0    endof
                ( south ) 2 of  0  -r>  endof
                ( west )  3 of -r> 0    endof
                endcase ;

( char parsing )
: l?            [char] L = if left else 0 then ;
: r?            [char] R = if right else 0 then ;
: turn?         getc dup l? swap r? or nextc ;

( number parsing )
: +digit        swap 10 * + ;
: digit?        [char] 0 [char] 9 range? ;
: char>num      [char] 0 - ;
: digits        0 begin getc digit? while getc char>num +digit nextc repeat ;

: match         ( direction ) turn? ?dup if turn dup digits vec move! then ;
: start         load-file center! north ;
: finish        drop ." total: " total . ;
: process       start begin chars? while match repeat finish ;

page cr process cr
