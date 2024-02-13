@echo off

set COMPILER_PATH=clang.exe
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\gc.c Backend\object.c Backend\standard.c

del %COMMAND_LINE_ARGS%.exe

%COMPILER_PATH% -O3 -S %COMMAND_LINE_ARGS%.c %SOURCE_FILES%

%COMMAND_LINE_ARGS%.exe