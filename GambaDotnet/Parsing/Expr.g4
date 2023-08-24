grammar Expr;

gamba: expression EOF;

expression: ('~' | '-') expression #UnaryExpression
            | expression ('**' | '*' | '<<' | '+' | '-' | '|' | '^' | '&') expression #BinaryExpression
            | LPARAM expression RPARAM                              #ParenthesizedExpression
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


