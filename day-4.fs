: reload!   s" day-4/main.fs" included ;

( import: slurp reset-line line? )
s" ../file.fs" included
: load-file         s" day-4/input.txt" slurp ;

( base string )
variable str
variable #str
5 constant #checksum
: =str              #str ! str ! ;
: #hash             #str @ #checksum - 3 - ;  \ length w/o checksum, brackets, nl
: hash              str @ #hash ;
: hash-at           str @ + c@ ;

: _checksum         str @ #str @ + #checksum - 2 - ; \ address + length - checksum - bracket, nl
: checksum          _checksum #checksum ;
: sector-id         _checksum 4 - 3 evaluate ;

( generated checksum )
5 constant #comp
create comp #comp chars allot
: =comp-at          ( val index ) chars comp + c! ;
: comp              comp #comp ;
: valid-checksum?   checksum comp compare 0 = ;

( letter frequency )
26 constant #freqs
variable freqs #freqs cells allot
: init-freqs        freqs #freqs cells erase ;
: char>index        [char] a - ;
: index>char        [char] a + ;
: letter?           dup [char] a >= swap [char] z <= and ;
: +1!               1 swap +! ;
: 0!                0 swap ! ;

: inc-freq-at       cells freqs + +1! ;
: clear-freq-at     cells freqs + 0! ;
: freq-at           cells freqs + @ ;
: +char             dup letter? if char>index inc-freq-at else drop then ;
: .freqs            #freqs 0 do i index>char emit ." : " i freq-at . loop ;

: count-freq        init-freqs #hash 0 do i hash-at +char loop ;

: new-max           >r drop drop r> dup freq-at ;
: freq-max          0 0 ( idx count )
                    #freqs 0 do dup i freq-at < if i new-max then loop drop ;
: make-checksum     #comp 0 do freq-max dup clear-freq-at index>char i =comp-at loop ;

: .hashline         hash type ."  [" checksum type ." ] " ;
: main              load-file reset-line 0 ( valid-count )
                    begin line? while
                        =str            cr .hashline
                        count-freq
                        make-checksum   ."  [" comp type ." ] "
                        valid-checksum? if ." *" sector-id + then
                    repeat
                    cr cr ." TOTAL: " . cr cr ;

page main
