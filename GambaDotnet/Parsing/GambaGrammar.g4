grammar GambaGrammar;

gamba: root EOF;

root: root ('**' | '+' | '-' | '*' | '^' | '&') root #BinaryExpression
            | LPARAM root RPARAM                              #ParenthesizedExpression
            | ('~' | '-') root #UnaryExpression
            | NUMBER #NumberExpression
            | ID #IdExpression
            ;



// Expression constructs
LPARAM      : '(';
RPARAM      : ')';
COMMA       : ',';

STRING      : ('"' ~["]* '"') | '%' STRING;
NUMBER      : [0-9]+;

ID          : (([a-zA-Z0-9]|'_')+)?;

WS          : [ \t\r\n]+ -> skip ;


