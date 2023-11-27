grammar Regex;

@header { #nullable disable }

pattern: startAnchor=Caret? alternatives endAnchor=Dollar? EOF;

alternatives: term ('|' term)*;

term: factor+;

factor: atom quantifier?;

atom: '(' alternatives ')' | any /*| literal*/ | character | '[' characterRange ']';

//literal: CharacterLiteral | StringLiteral;

quantifier: zeroOrOne | zeroOrMany | oneOrMany | exactly | atLeast | between;
zeroOrOne: '?';
zeroOrMany: '*';
oneOrMany: '+';
count: Number+;
exactly: '{' count '}';
atLeast: '{' min=count ',' '}';
between: '{' min=count ',' max=count '}';

any: Period;
character: Character;
characterRange: not=Caret? rangeFactor+;
rangeFactor: rangeAtom quantifier?;
rangeAtom: range | character;
range: start=Character '-' end=Character;

Caret: '^';
Dollar: '$';
Period: '.';
//CharacterLiteral: '\'' (EscapedCharacter | ~'\'') '\'';
//StringLiteral: '"' (EscapedCharacter | ~'"')* '"';

Character: EscapedCharacter | ~('[' | ']' | '-' | '\\');

EscapedCharacter: '\\' [nrt'"[\]];

Number: [0-9];

WS: [ \t\r]+ -> skip;

COMMENT: '#' ~[\r\n]* -> skip;
