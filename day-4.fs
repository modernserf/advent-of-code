: reload!   s" day-4.fs" included ;

( import: slurp reset-line line? )
s" file.fs" included
: load-file         s" 4.txt" slurp ;

( base string )
variable str
variable #str
5 constant #checksum
: =str              #str ! str ! ;
: #hash             #str @ #checksum - 7 - ;  \ length w/o checksum, digits, brackets, nl
: hash              str @ #hash ;
: hash-at           str @ + c@ ;

: _checksum         str @ #str @ + #checksum - 2 - ; \ address + length - checksum - bracket, nl
: checksum          _checksum #checksum ;
: sector-id         _checksum 4 - 3 evaluate ;

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

( generated checksum )
5 constant #comp
variable comp #comp chars allot
: =comp-at          ( val index ) chars comp + c! ;
: comp              comp #comp ;
: valid-checksum?   checksum comp compare 0 = ;

: new-max           >r drop drop r> dup freq-at ;
: freq-max          0 0 ( idx count )
                    #freqs 0 do dup i freq-at < if i new-max then loop drop ;
: make-checksum     #comp 0 do freq-max dup clear-freq-at index>char i =comp-at loop ;

( shift cypher )
variable cypherbuf 80 chars allot
: shift             dup letter? if + char>index 26 mod index>char then ;
: =cypher-at        chars cypherbuf + c! ;
: =cypher           sector-id #hash 80 min 0 do dup i hash-at shift i =cypher-at loop drop ;
: cypher            cypherbuf #hash ;
: match?            =cypher cypher s" northpole-object-storage" compare 0 = ;
: .match-sector     match? if ." sector: " sector-id . then ;

( output )
: start             load-file reset-line 0 ( valid-count ) ;
: finish            cr cr ." TOTAL: " . cr cr ;
: process           =str count-freq make-checksum valid-checksum? if sector-id + then ;
: part1             start begin line? while process repeat finish ;

: start             load-file reset-line ;
: process           =str count-freq make-checksum valid-checksum? if .match-sector then ;
: finish            cr cr ;
: part2             start begin line? while process repeat finish ;

page part2
