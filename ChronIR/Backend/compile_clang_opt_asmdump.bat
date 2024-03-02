@echo off

set COMPILER_PATH=clang.exe
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\memory.c Backend\object.c Backend\standard.c

%COMPILER_PATH% -O3 -S %COMMAND_LINE_ARGS% %SOURCE_FILES%