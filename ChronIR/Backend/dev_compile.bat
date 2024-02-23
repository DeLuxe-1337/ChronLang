@echo off

set COMPILER_PATH=clang
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=gc.c object.c standard.c

del %COMMAND_LINE_ARGS%.exe

%COMPILER_PATH% %COMMAND_LINE_ARGS%.c %SOURCE_FILES%

a.exe