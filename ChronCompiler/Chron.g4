grammar Chron;

program: line* EOF;

line: statement;

statement:
	defer
	| linkStatic
	| release
	| import_stmt
	| include_module
	| foreign_c
	| function
	| call
	| return
	| variable
	| if
	| for
	| foreach
	| while
	| break
	| continue;

modifier: STRING ('=' STRING)?;
modifiers: '[' (modifier (',' modifier)*)? ']';

variable:
	modifiers? expression op = ('=' | '+=' | '-=' | '/=' | '*=') expression;

return: 'return' expression?;

functionBlock: block | '?';
functionParameters: '(' (IDENTIFIER (',' IDENTIFIER)*)? ')';
functionForceName: '!';

function:
	modifiers? functionForceName? IDENTIFIER '::' functionParameters? functionBlock;

block: '{' line* '}';

while: 'while' expression block;

break: 'break';
continue: 'continue';

if: 'if' expression block ifElse?;
ifElse: 'else' block;

release: 'release' (expression (',' expression)*);
defer: 'defer' statement;

foreach:
	'foreach' index = IDENTIFIER ',' value = IDENTIFIER 'in' iter = expression block;
for:
	'for' IDENTIFIER '=' start = expression ',' end = expression block;

import_functionParameters:
	'(' (IDENTIFIER (',' IDENTIFIER)*)? ')';
linkStatic: 'static' 'import' STRING;
import_stmt: 'import' import_block;
import_function_return: '->' IDENTIFIER;
import_function:
	IDENTIFIER '::' import_functionParameters? import_function_return?;
import_statement: import_function;
import_block: '{' import_statement* '}';

call: callInvoke? expression callArgs ';'?;
callArgs: '(' (expression (',' expression)*)? ')';

callInvoke: (invoke='invoke' sig=IDENTIFIER);

expression:
	constant				# constantExpr
	| IDENTIFIER			# IDENTIFIERExpr
	| expression callArgs	# callExpr
	| expression op = (
		'=='
		| '!='
		| '>'
		| '<'
		| '<='
		| '>='
		| '||'
		| 'or'
		| 'and'
		| '&&'
	) expression												# comparatorExpr
	| '!' expression											# notExpr
	| expression op = ('+' | '-' | '*' | '/' | '%') expression	# binaryExpr
	| expression '=' expression									# bindExpr
	| '<' (expression (',' expression)*)? '>'					# tableExpr
	| expression '[' expression ']'								# tableIndexExpr
	| callInvoke expression callArgs						# callInvokeExpr
	| 'release' expression										# releaseExpr
	| '(' expression ')'										# evaluateExpr;

constant: NUMBER | STRING | BOOLEAN | NIL;

TICK_BLOCK: '`' TICK_TEXT '`';
fragment TICK_TEXT: ~('`')*;

foreign_c: 'C' TICK_BLOCK;

include_module: 'include' IDENTIFIER;

NIL: 'nil';
NUMBER: '-'? [0-9][0-9]*;
STRING: ('"' (ESC | ~'"')* '"');
fragment ESC: '\\' .;
BOOLEAN: 'false' | 'true';

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_.]*;

WHITESPACE: (' ' | '\t' | '\r' | '\n') -> skip;
COMMENT: '//' ~[\r\n]* -> skip;