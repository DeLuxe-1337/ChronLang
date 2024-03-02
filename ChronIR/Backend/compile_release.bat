@echo off

set COMPILER_PATH=Backend\tcc\tcc.exe
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\memory.c Backend\object.c Backend\standard.c

del %COMMAND_LINE_ARGS%.exe

%COMPILER_PATH% %COMMAND_LINE_ARGS% %SOURCE_FILES%

%COMMAND_LINE_ARGS%.exe