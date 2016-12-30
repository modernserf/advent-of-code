variable password 8 allot ( byte )
variable hash 16 allot
variable source 64 allot

( *str *hashed -- )
: md5 ;

( -- *ptr [in source, after input] ; writes to source )
: read-input ;

( *ptr value -- )
: write-number-to-str ;

: number-to-hex-char ;

( *ptr -- ok? )
: ?match-hash
    dup c@ 0 =
    over 1 + c@ 0 = and
    over 2 + c@ 0 = and
    swap 3 + c@ 16 < and
;

( *ptr -- value )
: hash-value 3 + c@ ;

( val ok -- nextVal ok )
: inc-if-false if true else 1 + false then ;

( *w idx -- *w nextIdx ok? )
: ?hash-value over ?match-hash inc-if-false ;

( *write startIndex -- endIndex charNumber )
: find-hash-value
    begin
        2dup write-number-to-str
        source hash md5
        ?hash-value
    until
    swap hash-value
;

: write-to-password password + c! ;

: main
    read-input ( *write )
    1 ( *write index )
    7 0 do
        over swap ( *w *w index )
        find-hash-value ( *w index hex )
        number-to-hex-char i write-to-password ( *w index )
    loop
    drop drop
    password 8 dump
;
