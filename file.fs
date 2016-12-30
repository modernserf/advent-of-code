( read a file into memory + tools for traversal )

( input buffer )
variable _src
variable #src

( file )
variable fh
: open      r/o open-file throw fh ! ;
: close     fh @ close-file throw ;
: read      begin here 4096 fh @ read-file throw dup allot 0= until ;
: gulp      open read close ;

: start     here _src ! ;
: finish    here _src @ - #src ! ;
: slurp     start gulp finish _src @ #src @ ;

( traversal )
variable offset
: reset-line        0 offset ! ;
: nextc             1 offset +! ;
: chars?            offset @ #src @ u< ;
: src@offset        offset @ _src @ + ;
: getc              src@offset c@ ;
: lf?               10 = ;
: nextl             begin chars? if getc nextc lf? else exit then until ;
: next-#str         offset @ nextl offset @ swap - ;
: line?             chars? dup if src@offset next-#str rot then ; ( str #str ok | 0 )
