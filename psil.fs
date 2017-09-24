: reload!       s" psil.fs" included ;

( types )
0 constant nil
1 constant type-number
2 constant type-double
3 constant type-str
4 constant type-cons
5 constant type-func
6 constant type-nfunc

: +header       1 cells + ;

( todo : refcount here )
: box           +header allocate throw tuck ! ;
: box~          free throw ;
: %type         dup if @ then ;

( nil: considered harmful )
: .nil          drop ." ()";
: nil?          0 = ;

( number:       [ header | number ] )
: %n!           +header ! ;
: %n@           +header @ ;
: %n            type-number 1 cells box tuck %n! ;
: %n.           %n@ 0 <# #s #> type ;

( double:       [ header | number | _ ] )
: %d!           +header 2! ;
: %d@           +header 2@ ;
: %d            type-double 2 cells box tuck %d! ;
: %d.           %d@ <# #s #> type ;

( str           [ header | #str | *str ] )
: %s!           +header 2! ;
: %s@           +header 2@ ;
: %strlen       +header @ ;
: %s            type-str 2 cells box dup >r %s! r> ;
: .quot         [char] " emit ;
: %s.           .quot %s@ type .quot ;

( cons:         [ header | *car | *cdr ] )
: %car          +header @ ;
: %car!         +header ! ;
: %cdr          +header 1 cells + @ ;
: %cdr!         +header 1 cells + ! ;
: %cons         ( cdr car ) type-cons 2 cells box tuck %car! tuck %cdr! ;
: %cons?        %type type-cons = ;

variable %.     ( hoisted %. definition )
: set-%.        %. ! ;
: %.            %. @ execute ;
: %cdr.         %cdr dup nil? if drop else dup %cons? if space %. else ."  . " %. then then ;
: %cons.        dup %car %. %cdr. ;

( func:         [ header | *xt ] )
: %f!           +header ! ;
: %call         +header @ execute ;
: %f            type-func 1 cells box tuck %f! ;
: %f.           drop ." <func> " ;

( named func :  [ header | *xt | #str | *str ] )
: %fname!       1 cells + +header 2! ;
: %fname        1 cells + +header 2@ ;
: %nf           type-nfunc 3 cells box dup >r %f! r@ %fname! r> ;
: %nf.          %fname type ;

( eval )
: into-stack    begin dup %cons? while dup >r %car r> %cdr repeat drop ;
: %eval         ( list ) dup >r %cdr into-stack r> %car %call ;

( print )
: %.            dup %type case
                nil         of .nil endof
                type-number of %n. endof
                type-double of %d. endof
                type-str    of %s. endof
                type-cons   of %cons. endof
                type-func   of %f. endof
                type-nfunc  of %nf. endof
                endcase ;
' %. set-%.

: .[            ." (" ;
: ].            ." )" ;
: %.            dup %cons? if .[ space %cons. space ]. else %. then ;

: %+            ( *a *b ) %n@ swap %n@ + %n ;
s" +" ' %+ %nf constant %+

( list literals )
variable #[
: ]#            nil begin swap dup #[ <> while %cons repeat drop ;
