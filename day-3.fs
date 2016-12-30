: reload! s" day-3.fs" included ;

s" file.fs" included
: load-file s" 3.txt" slurp ;

: side?         + < ;
: bool+         if 1 else 0 then ;
: valid         { a b c }
                a b c side?
                c a b side?
                b c a side? and and bool+ ;

: start         load-file reset-line 0 ;
: finish        ." total: " . ;
: run           start begin line? while evaluate valid + repeat finish ;

( 3 x 3 )
: rotblock      { a1 a2 a3 b1 b2 b3 c1 c2 c3 } a1 b1 c1 a2 b2 c2 a3 b3 c3 ;
: valid-block   rotblock valid >r valid >r valid r> r> + + ;
: eval3         evaluate line? drop evaluate line? drop evaluate ;
: run3          start begin line? while eval3 valid-block + repeat finish ;
