@echo off

set COMPILER_PATH=Backend\tcc\tcc.exe
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\memory.c Backend\object.c Backend\standard.c

del compiled.exe

%COMPILER_PATH% -DCHRON_DEBUG %COMMAND_LINE_ARGS% %SOURCE_FILES% -o compiled.exe

compiled.exe