grammar Expr;

gamba: expression EOF;

expression:   LPARAM expression RPARAM                              #ParenthesizedExpression
            | expression ('**') expression #PowExpression
            | ('~'|'~') expression #NegativeOrNegationExpression
            | expression ('*') expression #MulExpression
            | expression ('+'|'-') expression #AddOrSubExpression
            | expression ('<<') expression #ShiftExpression
            | expression ('&') expression #AndExpression
            | expression ('^') expression #XorExpression
            | expression ('|') expression #OrExpression
            | NUMBER #NumberExpression
            | ID #IdExpression
            ;



// Expression constructs
LPARAM      : '(';
RPARAM      : ')';
COMMA       : ',';


STRING      : ('"' ~["]* '"') | '%' STRING;
NUMBER      : '0x'? [0-9]+;

ID          : (([a-zA-Z0-9]|'_')+)?;

WS          : [ \t\r\n]+ -> skip ;


