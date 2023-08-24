grammar Expr;

gamba: expression EOF;

expression: expression ('**' | '*' | '<<' | '+' | '-'  | '^' | '&') expression #BinaryExpression
            | LPARAM expression RPARAM                              #ParenthesizedExpression
            | ('~' | '-') expression #UnaryExpression
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


