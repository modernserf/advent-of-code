turn(From, right, To) :-
    nextto(From, To, [north, east, south, west, north]).
turn(From, left, To) :-
    nextto(To, From, [north, east, south, west, north]).

forward((X1, Y1), north, Distance, (X1, Y2)) :- Y2 is Y1 - Distance.
forward((X1, Y1), east, Distance, (X2, Y1)) :- X2 is X1 + Distance.
forward((X1, Y1), south, Distance, (X1, Y2)) :- Y2 is Y1 + Distance.
forward((X1, Y1), west, Distance, (X2, Y1)) :- X2 is X1 - Distance.

move(travel(Turn, Distance), pos(StartDir, StartP), pos(EndDir, EndP)) :-
    turn(StartDir, Turn, EndDir),
    forward(StartP, EndDir, Distance, EndP).

taxi_distance(X, Y, Distance) :-
    abs(X, Xabs),
    abs(Y, Yabs),
    Distance is Xabs + Yabs.

%part 1
total_distance(Distance) :-
    travel_path(Path),
    foldl(move, Path, pos(north, (0, 0)), pos(_, (X, Y))),
    taxi_distance(X, Y, Distance).

%part 2
towards(X, X, X).
towards(X, Y, Z) :- X < Y, Z is X + 1.
towards(X, Y, Z) :- X > Y, Z is X - 1.

points_between((X, Y), (X, Y), []) :- !.
points_between((X1, Y1), (X2, Y2), [(X1, Y1)|Rest]) :-
    towards(X1, X2, XNext),
    towards(Y1, Y2, YNext),
    points_between((XNext, YNext), (X2, Y2), Rest).

move_with_points(pos(D1, P1), Travel, Points, pos(D2, P2)) :-
    move(Travel, pos(D1, P1), pos(D2, P2)),
    points_between(P1, P2, Points).

has_visited(P, [H|T]) :- member(P, H); has_visited(P, T).

traverse_with_state(Pos, [Travel|_], State, End) :-
    move_with_points(Pos, Travel, Between, _),
    member(End, Between),
    has_visited(End, State).
traverse_with_state(Start, [Travel|RestPath], State, End) :-
    move_with_points(Start, Travel, Between, Next),
    traverse_with_state(Next, RestPath, [Between|State], End).

total_distance_first_intersection(Distance) :-
    travel_path(Path),
    traverse_with_state(pos(north, (0, 0)), Path, [], (X, Y)),
    taxi_distance(X, Y, Distance).

% parse input
travel_path(Path) :-
    input(Str),
    split_string(Str, ',' , " ", Dirs),
    maplist(parse, Dirs, Path).

parse(Str, Travel) :-
    string_chars(Str, Chars),
    parse(Travel, Chars, _).

parse(travel(left, D)) --> ['L'], dcg_basics:integer(D).
parse(travel(right, D)) --> ['R'], dcg_basics:integer(D).

input("R3, L5, R1, R2, L5, R2, R3, L2, L5, R5, L4, L3, R5, L1, R3, R4, R1, L3, R3, L2, L5, L2, R4, R5, R5, L4, L3, L3, R4, R4, R5, L5, L3, R2, R2, L3, L4, L5, R1, R3, L3, R2, L3, R5, L194, L2, L5, R2, R1, R1, L1, L5, L4, R4, R2, R2, L4, L1, R2, R53, R3, L5, R72, R2, L5, R3, L4, R187, L4, L5, L2, R1, R3, R5, L4, L4, R2, R5, L5, L4, L3, R5, L2, R1, R1, R4, L1, R2, L3, R5, L4, R2, L3, R1, L4, R4, L1, L2, R3, L1, L1, R4, R3, L4, R2, R5, L2, L3, L3, L1, R3, R5, R2, R3, R1, R2, L1, L4, L5, L2, R4, R5, L2, R4, R4, L3, R2, R1, L4, R3, L3, L4, L3, L1, R3, L2, R2, L4, L4, L5, R3, R5, R3, L2, R5, L2, L1, L5, L1, R2, R4, L5, R2, L4, L5, L4, L5, L2, L5, L4, R5, R3, R2, R2, L3, R3, L2, L5").
