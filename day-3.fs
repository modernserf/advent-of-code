s" file.fs" included
s" 3.txt" slurp constant #src constant _src

( rotate a 3x3 matrix )
: rotblock { a1 a2 a3 b1 b2 b3 c1 c2 c3 } a1 b1 c1 a2 b2 c2 a3 b3 c3 ;

: reload! s" day-3.fs" included ;

: 3dup      >r 2dup r@ -rot r> ;
: side?     + < ;
: valid?    3dup side? >r rot
            3dup side? >r rot
            side? r> r> and and ;

variable count
: cinit     0 count ! ;
: inc       1 count +! ;
: count     count @ ;
: inc-valid valid? if inc then ;

14 constant len
variable offset
: oinit     0 offset ! ;
: nextl     len offset +! ;
: chars?    offset @ #src u< ;
: line      _src offset @ + len ;

( -- a b c ok | 0 )
: load      line evaluate nextl ;
: parse     chars? if load -1 else 0 then ;
: parse3    chars? if load load load -1 else 0 then ;

: run       oinit cinit begin parse while inc-valid repeat count . ;
: inc-block rotblock inc-valid inc-valid inc-valid ;
: run3      oinit cinit begin parse3 while inc-block repeat count . ;
