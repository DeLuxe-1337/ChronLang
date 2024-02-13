grammar Chron;

program: line* EOF;

line: statement;

statement: (release | import_stmt | include_module | foreign_c | function | call | return | variable | if | while | break | continue) ';'?;

variable: IDENTIFIER '=' expression;

return: 'return' expression?;

functionBlock: (block | '?');
functionParameters: '(' (IDENTIFIER (',' IDENTIFIER)*)? ')';
functionForceName: '!';
functionForceReturn: '#return true';
functionForceGc: '#gc';
functionRename: '#name' IDENTIFIER;
function: (functionForceReturn? | functionForceGc? | functionRename?) functionForceName? IDENTIFIER '::' functionParameters? functionBlock;

block: '{' line* '}';

while: 'while' expression block;

break: 'break';
continue: 'continue';

if: 'if' expression block ifElse?;
ifElse: 'else' block;

release: 'release' expression;

import_functionParameters: '(' (IDENTIFIER (',' IDENTIFIER)*)? ')';
import_stmt: 'import' import_block;
import_function_return: '->' IDENTIFIER;
import_function: IDENTIFIER '::' import_functionParameters? import_function_return?;
import_statement: import_function;
import_block: '{' import_statement* '}';

call: IDENTIFIER callArgs  ';'?;
callArgs: '(' (expression (',' expression)*)? ')';

expression:
	constant				# constantExpr
	| IDENTIFIER			# IDENTIFIERExpr
	| call					# callExpr
	| expression op=('==' | '!=' | '>' | '<' | '<=' | '>=' | '||' | 'or' | 'and' | '&&') expression #comparatorExpr
	| expression op=('+' | '-' | '*' | '/') expression #binaryExpr
	| 'release' expression #releaseExpr
	| '(' expression ')'	# evaluateExpr;

constant: NUMBER | STRING | BOOLEAN;

TICK_BLOCK: '`' TICK_TEXT '`';
fragment TICK_TEXT: ~('`')*;

foreign_c: '~>' 'C' TICK_BLOCK;

include_module: 'include' IDENTIFIER;

NUMBER: [0-9][0-9]*;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOLEAN: 'false' | 'true';

IDENTIFIER: [a-zA-Z0-9_][a-zA-Z0-9_.]*;

WHITESPACE: (' ' | '\t' | '\r' | '\n') -> skip;
COMMENT: '//' ~[\r\n]* -> skip;